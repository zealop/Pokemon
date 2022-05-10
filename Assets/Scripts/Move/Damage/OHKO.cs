using Battle;

namespace Move.Damage
{
    public class OHKO : MoveDamage
    {
        public override DamageDetail Apply(Unit source, Unit target)
        {
            return new DamageDetail(target.MaxHp);
        }
    }
}