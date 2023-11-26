﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] GameObject dialogBox;
        
        [SerializeField] Text dialogText;
        
        [SerializeField] int lettersPerSecond;

        Dialog dialog;

        int currentLine = 0;

        bool isTyping;

        public event Action onShowDialog;
        
        public event Action onHideDialog;

        public static DialogManager _instance;

        public static DialogManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("DialogManager not instantiated.");
                }

                return _instance;
            }
        }

        private void Awake()
        {
            // Any class can use the dialog manager
            // Gets exposed to everything
            // Careful with dependencies!
            // Singleton class!
            _instance = this;
        }

        public IEnumerator ShowDialog(Dialog dialog)
        {
            yield return new WaitForEndOfFrame(); // ensures that current frame gets rendered
            onShowDialog?.Invoke();
            this.dialog = dialog;
            dialogBox.SetActive(true);
            StartCoroutine(TypeDialog(dialog.Lines[0]));
        }

        public void HandleUpdate()
        {
            if (Input.GetKeyDown(KeyCode.T) && !isTyping)
            {
                ++currentLine;
                if (currentLine < dialog.Lines.Count) 
                {
                    StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
                }
                else
                {
                    dialogBox.SetActive(false);
                    currentLine = 0;
                    onHideDialog?.Invoke();
                }
            }
        }

        public IEnumerator TypeDialog(string line)
        {
            isTyping = true;
            dialogText.text = "";
            foreach (var letter in line.ToCharArray())
            {
                dialogText.text += letter;
                yield return new WaitForSeconds(1f/lettersPerSecond);
            }
            isTyping = false;
        }
    }
}
