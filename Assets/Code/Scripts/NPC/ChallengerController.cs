using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ChallengerController : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Challenger interaction");
        }
    }
}
