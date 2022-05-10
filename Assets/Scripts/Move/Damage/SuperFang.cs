using Battle;

namespace Move.Damage
{
    public class SuperFang : MoveDamage
    {
        private const int MULTIPLIER = 2;
        public override DamageDetail Apply(Unit source, Unit target)
        {
            int damage = source.Hp / MULTIPLIER;

            return new DamageDetail(damage);
        }
    }
}