using Sirenix.Serialization;

namespace Move.Behaviour
{
    public class FixedHit : MultiHit
    {
        [OdinSerialize] private int count;
        protected override int HitCount => count;

        public FixedHit(int count)
        {
            this.count = count;
        }
    }
}