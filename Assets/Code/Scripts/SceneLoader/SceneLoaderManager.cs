using Assets.Scripts.DataPersistence;
using Assets.Scripts.DataPersistence.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SceneLoaderManager : MonoBehaviour, ISceneLoader
    {
        public string scene;

        public void Load()
        {
            SceneController.Instance.LoadSceneControlled(scene);
        }

    }
}
