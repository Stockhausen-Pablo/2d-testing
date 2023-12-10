using Assets.Scripts.DataPersistence.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.DataPersistence
{
    public interface IDataPersistence
    {
        void LoadData(GameData gameData);

        void SaveData(GameData gameData);
    }
}
