using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "New statusPokemon ")]
public class StatusSprite : SerializedScriptableObject
{
    [SerializeField] Dictionary<StatusID, Sprite> sprites;
    public Dictionary<StatusID, Sprite> Sprites => sprites;

}
