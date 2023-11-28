using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.DataPersistence.Data;
using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public class FileDataHandler
    {
        private string dataDirPath = "";

        private string dataFileName = "";

        private bool useEncryption = false;

        private readonly string encryptionCodeWord = "9sU5-z,6DdvuGmDBZwtK";

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
            this.useEncryption = useEncryption;
        }

        public GameData Load()
        {
            // use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadedData = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    // load the serialized data from the file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // optionally decrypt data
                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    // deserialize the data from JSON back to the C# object
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                } 
                catch (Exception ex)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + ex);
                }
            }
            return loadedData;
        }

        public void Save(GameData data)
        {
            // use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            try
            {
                // create the directory the file will be written to if it doesn't exist already
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // serialize the C# game data object into JSON
                string dataToStore = JsonUtility.ToJson(data, true);
                
                // optionally encrypt data
                if (useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                // write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + ex);
            }

        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }
            return modifiedData;
        }
    }
}
