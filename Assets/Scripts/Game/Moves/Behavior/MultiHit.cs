using System.Collections.Generic;
using System.Linq;
using Game.Battles;
using Sirenix.Serialization;
using Utils;

namespace Game.Moves.Behavior
{
    public class MultiHit : Default
    {
        private static readonly Dictionary<int, int> DefaultTable = new()
        {
            { 2, 7 }, { 3, 7 }, { 4, 3 }, { 5, 3 }
        };

        [OdinSerialize] private readonly Dictionary<int, int> hitTable = DefaultTable;
        protected virtual int HitCount => WeightedRng.Roll(hitTable);

        protected override void ApplyDamage(MoveBuilder move, Unit source, Unit target)
        {
            foreach (var _ in Enumerable.Range(1, HitCount))
            {
                base.ApplyDamage(move, source, target);
            }
        }
    }
}