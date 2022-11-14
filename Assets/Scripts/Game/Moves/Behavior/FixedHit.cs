using Sirenix.Serialization;

namespace Game.Moves.Behavior
{
    public class FixedHit : MultiHit
    {
        [OdinSerialize] private int hitCount;
        protected override int HitCount => hitCount;
    }
}