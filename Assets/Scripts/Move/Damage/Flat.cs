using Battle;
using Sirenix.Serialization;

namespace Move.Damage
{
    public class Flat : MoveDamage
    {
        [OdinSerialize] private int damage;

        public Flat(int damage)
        {
            this.damage = damage;
        }
        public override DamageDetail Apply(Unit source, Unit target)
        {
            return new DamageDetail(damage);
        }
    }
}