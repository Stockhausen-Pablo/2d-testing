using UnityEngine;

namespace Assets.Scripts.SceneLoader
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
