using Assets.Scripts.DataPersistence.SerializableTypes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DataPersistence.Data
{
    [Serializable]
    public class GameData
    {
        public long lastUpdated;

        public Vector3 playerPos;

        public string scene;

        public SerializableDictionary<string, bool> conversations;

        // the values defined in this constructor will be the default values
        // the game starts with when there's no data to load
        public GameData()
        {
            playerPos = Vector3.zero;
            scene = "MainScene";
            conversations = new SerializableDictionary<string, bool>();
        }

        public int GetPercentageComplete()
        {
            // TODO - figure out a way to calculate the percentage completed
            // maybe based on conversations or mini-events
            return 25;
        }
    }
}
