using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.mkadmi
{
    public class AlertCanvas : MonoBehaviour
    {
        [SerializeField]
        private UIContainer loadingPanel;

        static AlertCanvas _instance = null;

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public static AlertCanvas Instance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AlertCanvas>();
            }

            return _instance;
        }

        public void Loading(bool isShowing)
        {
            if (isShowing) {
                loadingPanel.Show();
            }
            else
            {
                loadingPanel.Hide();
            }
        }

        public void Load_Scene(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        IEnumerator LoadSceneCoroutine(string sceneName_Path)
        {
            yield return null;
            Loading(true);
            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName_Path);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
            Loading(false);
        }
    }
}
