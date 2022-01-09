using System;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class SecondaryEffect : MoveComponent
{
    [SerializeField] private float chance;
    [SerializeField] private bool isSelf;
    [OdinSerialize] private MoveEffect effect;

    public void Apply(BattleUnit source, BattleUnit target)
    {
        if (Random.value <= chance)
        {
            effect.Apply(source, isSelf ? source : target);
        }
    }
}
