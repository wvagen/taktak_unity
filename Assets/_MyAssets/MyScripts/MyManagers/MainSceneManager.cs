using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using TMPro;

namespace com.mkadmi {
    public class MainSceneManager : MonoBehaviour
    {
        [SerializeField]
        private RtlText _helloTxt;

        [SerializeField]
        private TextMeshProUGUI _virtualCoins;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        void Init()
        {
            _helloTxt.text = string.Format("Hello {0} !", User.Instance().Name);
            _helloTxt.active = false;
            _virtualCoins.text = User.Instance().VirtualCoins.ToString();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
