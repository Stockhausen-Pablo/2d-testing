using Assets.Scripts.DataPersistence.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

namespace Assets.Scripts.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        private GameData gameData;

        private List<IDataPersistence> dataPersistenceObjects;

        public static DataPersistenceManager Instance { get; private set; }


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void NewGame()
        {
            this.gameData = new GameData();
        }

        public void LoadGame()
        {
            // TODO - Load any saved data from a file using the data handler
            // if no data can be loaded, initialize to a new game
            if (this.gameData == null)
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                NewGame();
            }

            // TODO - push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }

            Debug.Log("Loaded player move count = " +  gameData.playerMoved);

        }

        public void SaveGame()
        {
            // TODO - pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }

            Debug.Log("Saved player move count = " + gameData.playerMoved);

            // TODO - save that data to a file using the data handler
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}
