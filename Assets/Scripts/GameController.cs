using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum GameState { FreeRoam, Dialog, Loading };

    public class GameController : MonoBehaviour
    {
        // SerializeField exposes the player controller (script) to the inspector
        [SerializeField] PlayerController playerController;

        GameState state;

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
            SceneManager.LoadSceneAsync("MainScene");
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            //lambda function
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
        }

        private void Update()
        {
            if (state == GameState.FreeRoam)
            {
                playerController.HandleUpdate();
            } else if (state == GameState.Dialog)
            {
                DialogManager.Instance.HandleUpdate();
            } else if (state == GameState.Loading)
            {
                // do sth
            }
        }
    }
}
