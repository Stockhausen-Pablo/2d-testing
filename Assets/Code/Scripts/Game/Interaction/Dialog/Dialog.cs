using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class Dialog
    {
        [SerializeField] List<string> lines;

        public List<string> Lines
        {
            get { return lines;  }
        }

    }
}
