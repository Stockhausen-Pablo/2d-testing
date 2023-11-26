using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.DataPersistence.Data
{
    [Serializable]
    public class GameData
    {
        public int playerMoved;

        // the values defined in this constructor will be the default values
        // the game starts with when there's no data to load
        public GameData()
        {
            playerMoved = 0;
        }
    }
}
