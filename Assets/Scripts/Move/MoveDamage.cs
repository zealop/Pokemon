using Battle;
using Move;

public abstract class MoveDamage : MoveComponent
{
    public abstract DamageDetail Apply(BattleUnit source, BattleUnit target);
}