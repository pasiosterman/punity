using UnityEngine;

namespace PUnity.Utils
{
    public static class KeycodeUtilities
    {
        public static readonly KeyCode[] NumKeys = new KeyCode[]
        {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
        KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7,
        KeyCode.Alpha8, KeyCode.Alpha9
        };

        public static readonly KeyCode[] NumKeysWithNumbad = new KeyCode[]
        {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2,
        KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2,
        KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5,
        KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8,
        KeyCode.Keypad9
        };

        public static int GetNumkeyPressed()
        {
            for (int i = 0; i < NumKeys.Length; i++)
            {
                if (Input.GetKeyDown(NumKeys[i]))
                    return i;
            }
            return -1;
        }
    } 
}
