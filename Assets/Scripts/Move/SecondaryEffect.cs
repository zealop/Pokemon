using Battle;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Move
{
    public class SecondaryEffect : MoveComponent
    {
        [SerializeField] private readonly float chance;
        [OdinSerialize] private readonly MoveEffect effect;
        [SerializeField] private readonly bool isSelf;
        
        public SecondaryEffect(float chance, MoveEffect effect, bool isSelf = false)
        {
            this.chance = chance;
            this.isSelf = isSelf;
            this.effect = effect;
        }
        
        public void Apply(Unit source, Unit target)
        {
            if (Random.value <= chance)
            {
                effect.Apply(source, isSelf ? source : target);
            }
        }
    }
}
