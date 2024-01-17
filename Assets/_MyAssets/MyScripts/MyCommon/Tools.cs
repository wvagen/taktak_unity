using System;
using System.Threading.Tasks;
using UnityEngine;

namespace com.mkadmi
{
    public class Tools
    {

        public static ulong expNeededToReachLevel(int levelToReach)
            {
             if (levelToReach == 0) return 0;
            ulong xpNeeded = (1000 * (ulong)Mathf.Pow(2, levelToReach - 1));
            return xpNeeded;
            }

        public static short levelReached(ulong expReached)
        {
            short reachedLevel = 1;

            for (short i = 1; i < UserSettings.MAX_LEVEL; i++)
            {
                if (expReached < expNeededToReachLevel(i))
                {
                    reachedLevel = i;
                    break;
                }
            }
            return reachedLevel;
        }

        public static async Task<Sprite> Fetch_User_Img(string path)
        {
            Sprite photoImg;
            try {
                var bytes = await SB_Client.Instance().Storage.From(UserSettings.USER_FILES_PATH).Download(path, null);
                photoImg = BytesToSprite(bytes);
            }
            catch (Exception e)
            {
                photoImg = Resources.Load("MyDefaultSprites/photo") as Sprite;
                Debug.LogError("Error: "+ e.Message + " on Path: " + path);
            }
            return photoImg;
        }

        static Sprite BytesToSprite(byte[] imageBytes)
        {
            // Create a new Texture2D
            Texture2D texture = new Texture2D(2, 2);

            // Load image data into the texture
            texture.LoadImage(imageBytes);

            // Create a Sprite using the Texture2D
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            return sprite;
        }
    }
}
