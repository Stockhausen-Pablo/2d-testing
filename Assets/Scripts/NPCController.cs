using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class NPCController : MonoBehaviour, IInteractable
    {
        [SerializeField] Dialog dialog;
        public void Interact()
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }
    }
}
