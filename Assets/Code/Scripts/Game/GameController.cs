using Assets.Scripts.SceneLoader;
using UnityEngine;

namespace Assets.Scripts
{
    public enum GameState { FreeRoam, Dialog };

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
        }

        private void Update()
        {   
            // Check if controll inputs were executed
            this.CheckForInputs();

            // handle update based on current state
            switch (state)
            {
                case GameState.FreeRoam:
                    playerController.HandleUpdate();
                    break;
                case GameState.Dialog:
                    DialogManager.Instance.HandleUpdate();
                    break;
                default:
                    Debug.LogError("No valid game state provided to the game manager");
                    break;
            }
        }

        private void CheckForInputs()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneController.Instance.LoadMainMenuScene();
            }
        }
    }
}
