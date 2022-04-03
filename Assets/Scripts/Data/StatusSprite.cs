using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using Util;

//[CreateAssetMenu(fileName = "Pokemon", menuName = "New status sprite ")]
namespace Data
{
    public class StatusSprite : SerializedSingletonScriptableObject<StatusSprite>
    {
        [OdinSerialize] private Dictionary<StatusID, Sprite> sprites;
        public Dictionary<StatusID, Sprite> Sprites => sprites;
    }
}
