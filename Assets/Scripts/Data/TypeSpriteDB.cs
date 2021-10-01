using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "New statusPokemon ")]
public class TypeSpriteDB : SerializedScriptableObject
{
    [SerializeField] Dictionary<PokemonType, TypeSprite> data;
    public Dictionary<PokemonType, TypeSprite> Data => data;
}

[System.Serializable]
public class TypeSprite
{
    [LabelWidth(50)]
    [PreviewField(100)]
    [SerializeField] Sprite icon;
    [LabelWidth(50)]
    [PreviewField(100)]
    [SerializeField] Sprite card;
    [LabelWidth(50)]
    [SerializeField] Color color;


    public Sprite Icon => icon;
    public Sprite Card => card;
    public Color Color => color;
}
