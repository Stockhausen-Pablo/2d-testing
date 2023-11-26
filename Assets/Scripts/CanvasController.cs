using UnityEngine;

namespace Assets.Scripts
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one CanvasController in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
