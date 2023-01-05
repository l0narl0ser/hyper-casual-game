using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Gyroscope = UnityEngine.Gyroscope;

namespace Core
{
    public class Application : MonoBehaviour
    {
        private static Application _instance;

        public static Application Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindObjectOfType<Application>();

                return _instance;
            }
        }

        private void Awake()
        {
            Context.Instance.GetSnapshotManager().Load();
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Exit()
        {
            Context.Instance.GetSnapshotManager().Save();
            UnityEngine.Application.Quit();
        }
    }
}