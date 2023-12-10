using Assets.Scripts.DataPersistence;
using Assets.Scripts.DataPersistence.Data;
using Assets.Scripts.Static;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SceneLoader
{
    public enum SceneState { Loading, Loaded, MainMenu };

    public class SceneController : MonoBehaviour, IDataPersistence
    {
        SceneState state = SceneState.MainMenu;

        private string sceneToLoad = SceneName.MainMenu;

        private string currentScene = SceneName.MainMenu;

        private string sceneToSave = SceneName.StartScene;

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
                this.LoadSceneControlled(scene);
            };

        }

        private void Update()
        {
            // handle attempts to load a one scene multiple times
            if (String.Equals(this.currentScene, sceneToLoad))
            {
                return;
            }
            else if (!String.Equals(this.currentScene, sceneToLoad))
            {
                switch (state)
                {
                    case SceneState.Loading:
                        // load scene
                        this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this.sceneToLoad));
                        // update scene controller states
                        this.currentScene = sceneToLoad;
                        this.state = SceneState.Loaded;
                        break;
                    case SceneState.Loaded:
                        // scene is loaded - nothing has to be done
                        break;
                    case SceneState.MainMenu:
                        // save the game anytime before loading a new scene
                        DataPersistenceManager.Instance.SaveGame();
                        // load the main menu scene
                        //SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
                        this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this.sceneToLoad));
                        this.currentScene = sceneToLoad;
                        break;
                    default:
                        Debug.LogError("No valid scene state provided to the scene controller");
                        break;
                }
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
            this.sceneToLoad = SceneName.MainMenu;
            this.sceneToSave = this.currentScene;
            this.state = SceneState.MainMenu;   
        }

        public void LoadSceneControlled(string scene)
        {
            this.sceneToLoad = scene;
            this.state = SceneState.Loading;
        }

        public void LoadData(GameData gameData)
        {
            //this.LoadSceneControlled(gameData.scene);
            return;
        }

        public void SaveData(GameData gameData)
        {
            gameData.scene = this.sceneToSave;
        }
    }
}
