using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
    public class Menu : MonoBehaviour
    {
        [Header("First Selected Button")]
        [SerializeField] private Button firstSelected;

        protected virtual void OnEnable()
        {
            SetFirstSelected(firstSelected);
        }

        public void SetFirstSelected(Button firstSelectedButton)
        {
            firstSelectedButton.Select();
        }
    }
}
