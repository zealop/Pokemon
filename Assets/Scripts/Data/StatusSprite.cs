using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

//[CreateAssetMenu(fileName = "Pokemon", menuName = "New statusPokemon ")]
public class StatusSprite : SerializedScriptableObject
{
    [OdinSerialize] private Dictionary<StatusID, Sprite> sprites;
    public Dictionary<StatusID, Sprite> Sprites => sprites;
}
