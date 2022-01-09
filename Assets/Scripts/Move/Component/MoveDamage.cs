using UnityEngine;
public abstract class MoveDamage : MoveComponent
{
    public abstract DamageDetail Apply(BattleUnit source, BattleUnit target);
}
public class DefaultDamage : MoveDamage
{
    private const string CritMessage = "A critical hit!";
    private const string NotEffectiveMessage = "It's super effective!";
    private const string SuperEffectiveMessage = "It's not very effective!";
    private const float CritMod = 1.5f;
    public override DamageDetail Apply(BattleUnit source, BattleUnit target)
    {
        float critMod = 1f;
        if(Random.value <= MoveCritical.CritChance(move.CritStage, source, target))
        {
            critMod = CritMod;
        }

        float randMod = Random.Range(0.85f, 1f);
        float typeMod = TypeChart.GetEffectiveness(move.Type, target.Types);
        float STAB = source.Types.Contains(move.Type) ? 1.5f : 1f;

        //int attack = move.Attack(source, target);
        int attack = source.Attack;
        //int defense = move.Defense(source, target);
        int defense = target.Defense;

        float a = (2 * source.Level + 10) / 250f;
        float d = a * move.Power * ((float)attack / defense) + 2;

        float attackMod = source.AttackerMod(move, target);
        float defenseMod = target.DefenderMod(move, source);

        int damage = Mathf.FloorToInt(d * critMod * randMod * typeMod * attackMod * defenseMod);

        var detail = new DamageDetail(damage);

        if (critMod > 1f)
            detail.Messages.Add(CritMessage);
        if (typeMod > 1f)
            detail.Messages.Add(NotEffectiveMessage);
        if (typeMod < 1f)
            detail.Messages.Add(SuperEffectiveMessage);

        return detail;
    }
}


public class FlatMoveDamage : MoveDamage
{
    [SerializeField] private int damage;
    public override DamageDetail Apply(BattleUnit source, BattleUnit target)
    {
        return new DamageDetail(damage);
    }
}
public class LevelMoveDamage : MoveDamage
{
    public override DamageDetail Apply(BattleUnit source, BattleUnit target)
    {
        return new DamageDetail(source.Level);
    }
}
public class CounterMoveDamage : MoveDamage
{
    private const int MULTIPLIER = 2;
    public override DamageDetail Apply(BattleUnit source, BattleUnit target)
    {
        VolatileCounter counter = (VolatileCounter)source.Volatiles[VolatileID.Counter];
        int damage = counter.StoredDamage * MULTIPLIER;

        return new DamageDetail(damage);
    }
}
public class PsyWaveMoveDamage : MoveDamage
{
    private const float MIN = 0.5f;
    private const float MAX = 1.5f;
    public override DamageDetail Apply(BattleUnit source, BattleUnit target)
    {
        float r = Random.Range(MIN, MAX);
        int damage = Mathf.Max(Mathf.FloorToInt(source.Level * r), 1);

        return new DamageDetail(damage);
    }
}
public class SuperFangMoveDamage : MoveDamage
{
    private const int MULTIPLIER = 2;
    public override DamageDetail Apply(BattleUnit source, BattleUnit target)
    {
        int damage = source.HP / MULTIPLIER;

        return new DamageDetail(damage);
    }
}