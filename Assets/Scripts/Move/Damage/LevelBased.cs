using Battle;

namespace Move.Damage
{
    public class LevelBased : MoveDamage
    {
        public override DamageDetail Apply(BattleUnit source, BattleUnit target)
        {
            return new DamageDetail(source.Level);
        }
    }
}