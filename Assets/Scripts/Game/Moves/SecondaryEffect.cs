using Game.Battles;
using Sirenix.Serialization;
using Random = UnityEngine.Random;

namespace Game.Moves
{
    public class SecondaryEffect : IMoveComponent
    {
        [OdinSerialize] private readonly float chance;
        [OdinSerialize] private readonly IMoveEffect effect;
        [OdinSerialize] private readonly bool isSelf;

        public float Chance => chance;
        public IMoveEffect Effect => effect;

        public bool IsSelf => isSelf;

        public SecondaryEffect(float chance, IMoveEffect effect, bool isSelf = false)
        {
            this.chance = chance;
            this.isSelf = isSelf;
            this.effect = effect;
        }

        public void Apply(MoveBuilder move, Unit source, Unit target)
        {
            if (Random.value > chance) return;

            effect.Apply(move, source, isSelf ? source : target);
        }
    }
}