using System.Collections.Generic;
using UnityEngine;

namespace aa
{
    public class DialogueParse : MonoBehaviour
    {
        private static Dictionary<string, TalkData[]> DialoueDictionary =
                         new Dictionary<string, TalkData[]>();

        public static TalkData[] GetDialogue(string eventName)
        {
            return TalkDictionary[eventName];
        }
    }
}