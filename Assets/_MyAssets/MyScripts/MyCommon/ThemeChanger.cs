using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace com.mkadmi {
    public class ThemeChanger : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> _frameSprites;

        [SerializeField]
        private List<Color> _textColors;

        [SerializeField]
        private List<Image> _frames;

        [SerializeField]
        private List<TextMeshProUGUI> _texts;

        public void ChangeTheme()
        {
            switch (UserSettings.Instance().THEME_NAME)
            {
                case "light":
                    foreach (Image img in _frames) img.sprite = _frameSprites[0];
                    foreach (TextMeshProUGUI txt in _texts) txt.color = _textColors[0];
                    break;

                case "dark":
                    foreach (Image img in _frames) img.sprite = _frameSprites[1];
                    foreach (TextMeshProUGUI txt in _texts) txt.color = _textColors[1];
                    break;
            }
        }

    }
}
