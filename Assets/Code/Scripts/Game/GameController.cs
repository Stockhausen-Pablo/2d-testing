using Assets.Scripts.DataPersistence;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum GameState { FreeRoam, Dialog, MainMenu };

    public class GameController : MonoBehaviour
    {
        // SerializeField exposes the player controller (script) to the inspector
        [SerializeField] PlayerController playerController;

        GameState state;

        private string sceneToLoad;

        private string currentScene;

        public static GameController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one GameController in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            // dialog actions
            DialogManager.Instance.onShowDialog += () =>
            {
                state = GameState.Dialog;
            };
            DialogManager.Instance.onHideDialog += () =>
            {
                if (state == GameState.Dialog) {
                    state = GameState.FreeRoam;
                }
            };

            // player controller actions
            playerController.onGameQuitToMainMenu += () =>
            {
                //SceneManager.LoadScene("MainMenu");
                //state = GameState.MainMenu;
                SceneController.Instance.LoadMainMenuScene();
                //DeactivateGameComponents();

            };
        }

        private void Update()
        {
            switch (state)
            {
                case GameState.FreeRoam:
                    playerController.HandleUpdate();
                    break;
                case GameState.Dialog:
                    DialogManager.Instance.HandleUpdate();
                    break;
                case GameState.MainMenu:
                    break;
                default:
                    Debug.LogError("No valid game state provided to the game manager");
                    break;
            }
        }

        private void ActivateGameComponents()
        {
            playerController.gameObject.SetActive(true);
            //SceneManager.LoadScene("MainScene");
        }

        private void DeactivateGameComponents()
        {
            playerController.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
