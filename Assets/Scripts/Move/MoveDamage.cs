using Battle;
using Move;

public abstract class MoveDamage : MoveComponent
{
    public abstract DamageDetail Apply(Unit source, Unit target);
}