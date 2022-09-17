using Battle;
using Data;

namespace Move.Damage
{
    public class Counter : MoveDamage
    {
        private const int MULTIPLIER = 2;
        public override DamageDetail Apply(Unit source, Unit target)
        {
            var counter = (Data.Volatile.Counter)source.Volatiles[VolatileID.Counter];
            int damage = counter.storedDamage * MULTIPLIER;

            return new DamageDetail(damage);
        }
    }
}