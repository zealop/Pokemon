using System.Collections.Generic;
using Data.Condition;
using Sirenix.Serialization;
using UnityEngine;
using Utils;

//[CreateAssetMenu(fileName = "Pokemon", menuName = "New status sprite ")]
namespace Data
{
    public class StatusSprite : SerializedSingletonScriptableObject<StatusSprite>
    {
        [OdinSerialize] private Dictionary<StatusID, Sprite> sprites;
        public Dictionary<StatusID, Sprite> Sprites => sprites;
    }
}
