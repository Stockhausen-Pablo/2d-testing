using Assets.Scripts.DataPersistence;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
    public class MainMenu : Menu
    {
        [Header("Menu Navigation")]
        [SerializeField] private SaveSlotsMenu saveSlotsMenu;

        [Header("Menu Buttons")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button quitGameButton;

        private void Start()
        {
            DisableButtonsDependingOnData();
        }

        private void DisableButtonsDependingOnData()
        {
            if (!DataPersistenceManager.Instance.HasGameData())
            {
                continueGameButton.interactable = false;
                loadGameButton.interactable = false;
            }
        }

        public void OnNewGameClicked()
        {
            Debug.Log("New Game clicked");
            saveSlotsMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked()
        {
            Debug.Log("Load Game clicked");
            saveSlotsMenu.ActivateMenu(true);
            this.DeactivateMenu();
        }

        public void OnContinueGameClicked()
        {
            DisableMenuButtons();
            // save the game anytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            // load the next scene - which will in turn load the game because of 
            // OnSceneLoaded() in the DataPersistenceManager
            // notify the game controller that a new game is being loaded
            // todo - change to saved scene name
            DataPersistenceManager.Instance.LoadPersistedScene();
        }

        public void OnQuitGameClicked()
        {
            Application.Quit();
        }

        private void DisableMenuButtons()
        {
            newGameButton.interactable = false;
            continueGameButton.interactable = false;
            quitGameButton.interactable = false;
        }

        public void ActivateMenu()
        {
            this.gameObject.SetActive(true);
            DisableButtonsDependingOnData();
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}
