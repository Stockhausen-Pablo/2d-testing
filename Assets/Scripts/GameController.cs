﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum GameState { FreeRoam, Dialog, Loading };

    public class GameController : MonoBehaviour
    {
        private static GameController gameController;

        // SerializeField exposes the player controller (script) to the inspector
        [SerializeField] PlayerController playerController;

        GameState state;

        private void Awake()
        {
            if (gameController == null)
            {
                gameController = this;
                SceneManager.LoadScene(1);
                DontDestroyOnLoad(gameObject);
            }
            else if (gameController != this)
            {
                Destroy(gameObject);
            }
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