using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UPersian.Components;

namespace com.mkadmi
{
    public class AlertCanvas : MonoBehaviour
    {

        class Custom_SnackBar
        {
            public int SnackBarType;
            public string SnackBarContent;

            public Custom_SnackBar(string SnackBarContent, int SnackBarType)
            {
                this.SnackBarType = SnackBarType;
                this.SnackBarContent = SnackBarContent;
            }
        }

        [SerializeField]
        private UIContainer loadingPanel;

        [SerializeField]
        private UIContainer _SnackBar;
        [SerializeField]
        private Image _SnackBarBG;
        [SerializeField]
        private RtlText _SnackBarContent;
        [SerializeField]
        private List<Sprite> _SnackbarBGSparites;
        [SerializeField]
        private List<Custom_SnackBar> _SnackBarQueue = new List<Custom_SnackBar>();


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

        public void Show_SnackBar(string title, int type = 0)
        {
            _SnackBarQueue.Add(new Custom_SnackBar(title, type));
            Play_SackBar_Anim();
        }

        public void Play_SackBar_Anim()
        {
            if (_SnackBarQueue.Count > 0 && _SnackBar.isHidden)
            {
                _SnackBarContent.text = _SnackBarQueue[0].SnackBarContent;
                _SnackBarBG.sprite = _SnackbarBGSparites[_SnackBarQueue[0].SnackBarType];
                _SnackBar.Show();
                _SnackBarQueue.RemoveAt(0);
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
