using Assets.Scripts.DataPersistence;
using Assets.Scripts.DataPersistence.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class NPCController : MonoBehaviour, IInteractable, IDataPersistence
    {
        [SerializeField] Dialog dialog;

        [SerializeField] private static string id = "NPC-MainScene-25acc0f30e51";

        private bool talkedTo; // Just for testing purpose, persist conversation history
        
        [ContextMenu("generate guid for id")]
        private void GenerateGuid()
        {
            id = System.Guid.NewGuid().ToString();
        }

        public void Interact()
        {
            talkedTo = true;
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }

        public void LoadData(GameData gameData)
        {
            gameData.conversations.TryGetValue(id, out talkedTo);
            if (talkedTo)
            {
                Debug.Log("I already talked to this npc.");
            }
        }

        public void SaveData(GameData gameData)
        {
            if (gameData.conversations.ContainsKey(id))
            {
                gameData.conversations.Remove(id);
            }
            gameData.conversations.Add(id, talkedTo);
        }
    }
}
