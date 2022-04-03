using Battle;

namespace Move.Damage
{
    public class SuperFang : MoveDamage
    {
        private const int MULTIPLIER = 2;
        public override DamageDetail Apply(BattleUnit source, BattleUnit target)
        {
            int damage = source.Hp / MULTIPLIER;

            return new DamageDetail(damage);
        }
    }
}