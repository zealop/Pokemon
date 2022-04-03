using Battle;
using Data;
using Move;

public class Counter : MoveDamage
{
    private const int MULTIPLIER = 2;
    public override DamageDetail Apply(BattleUnit source, BattleUnit target)
    {
        var counter = (VolatileCounter)source.Volatiles[VolatileID.Counter];
        int damage = counter.StoredDamage * MULTIPLIER;

        return new DamageDetail(damage);
    }
}