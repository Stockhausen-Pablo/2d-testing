using Assets.Scripts.DataPersistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum SceneState { Loading, Loaded, MainMenu };

    public class SceneController : MonoBehaviour
    {
        SceneState state = SceneState.MainMenu;

        private string sceneToLoad = "MainMenu";

        private string currentScene = "MainMenu";

        private AsyncOperation _asyncOperation;

        public static SceneController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one SceneController in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            // data persistence actions
            DataPersistenceManager.Instance.onGameLoad += (string scene) =>
            {
                Debug.Log("Scene controller noticed onGameLoad");
                this.sceneToLoad = scene;
                //SceneManager.LoadScene("GameScene");
                //SceneManager.LoadScene(scene);
                //ActivateGameComponents();;
                state = SceneState.Loading;
            };

        }

        private void Update()
        {
            switch (state)
            {
                case SceneState.Loading:
                    // handle attempts to load a one scene multiple times
                    if (String.Equals(this.currentScene, sceneToLoad))
                    {
                        Debug.LogError("Tried to load the current scene again");
                        break;
                    }
                    // load scene
                    this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this.sceneToLoad));
                    // update scene controller fields
                    this.currentScene = sceneToLoad;
                    this.state = SceneState.Loaded;
                    break;
                case SceneState.Loaded:
                    // scene is loaded - nothing has to be done
                    break;
                case SceneState.MainMenu:
                    if (String.Equals(this.currentScene, sceneToLoad))
                    {
                        break;
                    }
                    // save the game anytime before loading a new scene
                    DataPersistenceManager.Instance.SaveGame();
                    // deactivate game components
                    //DeactivateGameComponents();
                    // load the main menu scene
                    SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
                    this.currentScene = sceneToLoad;
                    break;
                default:
                    Debug.LogError("No valid scene state provided to the scene controller");
                    break;
            }
        }

        private IEnumerator LoadSceneAsyncProcess(string sceneName)
        {
            // Begin to load the Scene you have specified.
            this._asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            // Don't let the Scene activate until you allow it to.
            this._asyncOperation.allowSceneActivation = true;

            while (!this._asyncOperation.isDone)
            {
                Debug.Log($"[scene]:{sceneName} [load progress]: {this._asyncOperation.progress}");

                yield return null;
            }
        }


        public void LoadMainMenuScene()
        {
            this.sceneToLoad = "MainMenu";
            this.state = SceneState.MainMenu;   
        }

        public void LoadSceneControlled(string scene)
        {
            this.sceneToLoad = scene;
            this.state = SceneState.Loading;
        }
    }
}
