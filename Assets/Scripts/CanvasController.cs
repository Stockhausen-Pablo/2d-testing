using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class CanvasController : MonoBehaviour
    {
        private static CanvasController canvasController;

        private void Awake()
        {
            if (canvasController == null)
            {
                canvasController = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (canvasController != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
