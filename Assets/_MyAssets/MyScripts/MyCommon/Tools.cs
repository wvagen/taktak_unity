using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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

        public static void Fetch_User_Img(string path, Image imgRef)
        {
            //Sprite photoImg;
            Texture2D loadingSpr, errorSpr;
            loadingSpr = Resources.Load("MyDefaultSprites/loading") as Texture2D;
            errorSpr = Resources.Load("MyDefaultSprites/error") as Texture2D;

            try
            {
                string fullPath = SB_Client.Instance().Storage.From(UserSettings.USER_FILES_PATH).GetPublicUrl(path);
                Davinci.get().load(fullPath).setLoadingPlaceholder(loadingSpr)
                            .setErrorPlaceholder(errorSpr).into(imgRef).start();
                //var bytes = await .Download(path, null);
                //photoImg = BytesToSprite(bytes);
            }
            catch (Exception e)
            {
                //photoImg = Resources.Load("MyDefaultSprites/photo") as Sprite;
                Debug.LogError("Error: "+ e.Message + " on Path: " + path);
            }
            //return photoImg;
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
