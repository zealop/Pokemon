using Battle;

namespace Move.Damage
{
    public class OHKO : MoveDamage
    {
        public override DamageDetail Apply(BattleUnit source, BattleUnit target)
        {
            return new DamageDetail(target.MaxHp);
        }
    }
}