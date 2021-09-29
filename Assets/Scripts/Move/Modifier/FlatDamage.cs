using System.Collections;
using UnityEngine;

public class FlatDamage : MoveModifier
{
    [SerializeField] int damage;
    [SerializeField] bool scaleLevel;
    public override void ModifyMove()
    {
        _base.Damage = Flat;
    }

    int Flat(BattleUnit source, BattleUnit target)
    {

        return scaleLevel ? source.Level : damage;
    }
}

public class CounterDamage : MoveModifier
{
    [SerializeField] int multiplier = 2;
    public override void ModifyMove()
    {
        _base.Prepare = PrepareCounter;
        _base.Damage = Counter;
    }

    int Counter(BattleUnit source, BattleUnit target)
    {
        VolatileCounter counter = (VolatileCounter)source.Volatiles[VolatileID.Counter];

        return counter.StoredDamage * multiplier;

    }
    IEnumerator PrepareCounter(BattleUnit source, BattleUnit target)
    {
        yield return source.AddVolatileCondition(new VolatileCounter());
    }
}

public class Psywave : MoveModifier
{
    public override void ModifyMove()
    {
        _base.Damage = Fluctuate;
    }

    int Fluctuate(BattleUnit source, BattleUnit target)
    {
        int r = Random.Range(50, 151);

        return Mathf.Max(Mathf.FloorToInt(source.Level * r / 100), 1);
    }

}

public class SuperFang : MoveModifier
{
    public override void ModifyMove()
    {
        _base.Damage = Halved;
    }

    int Halved(BattleUnit source, BattleUnit target)
    {
        int damage = target.HP / 2;

        return Mathf.Max(damage, 1);
    }

}