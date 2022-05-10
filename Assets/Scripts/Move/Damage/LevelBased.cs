using Battle;

namespace Move.Damage
{
    public class LevelBased : MoveDamage
    {
        public override DamageDetail Apply(Unit source, Unit target)
        {
            return new DamageDetail(source.Level);
        }
    }
}