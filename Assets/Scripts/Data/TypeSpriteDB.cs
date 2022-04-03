using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using Util;

[CreateAssetMenu(fileName = "Pokemon", menuName = "New statusPokemon ")]
public class TypeSpriteDB : SerializedSingletonScriptableObject<TypeSpriteDB>
{
    [OdinSerialize] private Dictionary<PokemonType, TypeSprite> data;
    public Dictionary<PokemonType, TypeSprite> Data => data;
}

[System.Serializable]
public class TypeSprite
{
    [LabelWidth(50)]
    [PreviewField(100)]
    [SerializeField]
    private Sprite icon;
    [LabelWidth(50)]
    [PreviewField(100)]
    [SerializeField]
    private Sprite card;
    [LabelWidth(50)]
    [SerializeField]
    private Color color;


    public Sprite Icon => icon;
    public Sprite Card => card;
    public Color Color => color;
}
