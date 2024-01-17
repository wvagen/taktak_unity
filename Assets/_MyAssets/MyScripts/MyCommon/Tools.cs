
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

    }
}
