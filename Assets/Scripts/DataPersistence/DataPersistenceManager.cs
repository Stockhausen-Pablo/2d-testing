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
        [Header("File Storage Config")]
        [SerializeField] private string fileName;

        [SerializeField] private bool useEncryption;

        private GameData gameData;

        private List<IDataPersistence> dataPersistenceObjects;

        private FileDataHandler fileDataHandler;

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
            this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void NewGame()
        {
            this.gameData = new GameData();
        }

        public void LoadGame()
        {
            // load any saved data from a file using the data handler
            this.gameData = fileDataHandler.Load();

            // if no data can be loaded, initialize to a new game
            if (this.gameData == null)
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                NewGame();
            }

            // push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }

        public void SaveGame()
        {
            // pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }

            // save that data to a file using the data handler
            fileDataHandler.Save(gameData);
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
