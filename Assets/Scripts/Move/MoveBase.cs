using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "Move", menuName = "New move")]
public class MoveBase : SerializedScriptableObject
{
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] MoveCategory category;
    [SerializeField] MoveTarget target;

    [SerializeField] int pp;
    [SerializeField] int power;
    [SerializeField] int accuracy;


    [System.NonSerialized, OdinSerialize]
    [SerializeField] List<MoveEffect> effects = new List<MoveEffect>();

    [System.NonSerialized, OdinSerialize]
    [SerializeField] List<MoveModifier> modifiers = new List<MoveModifier>();

    public string Name { get => name; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public MoveCategory Category { get => category; }

    public MoveTarget Target { get => target; }


    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }

    static float[] critTable = { 1 / 24f, 1 / 8f, 1 / 2f, 1f };

    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, int> Power;

    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, int> Priority = (s, t) => 0;

    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, int> CritStage = (s, t) => 0;

    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, int> Damage;
    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, int> Attack;
    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, int> Defense;
    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, float> HitChance;
    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, bool> ExtraImmunityCheck = (s, t) => false;


    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, IEnumerator> DamageBehaviour;

    [System.NonSerialized]
    public List<System.Func<BattleUnit, BattleUnit, IEnumerator>> SecondaryEffectList = new List<System.Func<BattleUnit, BattleUnit, IEnumerator>>();


    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, IEnumerator> OnFail;

    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, IEnumerator> Prepare;

    [System.NonSerialized]
    public System.Func<int, BattleUnit, BattleUnit, IEnumerator> Recoil;

    [System.NonSerialized]
    public System.Func<BattleUnit, BattleUnit, int> HitCount = (s, t) => 1;
    private void OnEnable()
    {
        Damage = DefaultDamageCalculation;
        Attack = DefaultAttackCalculation;
        Defense = DefaulDefenseCalculation;
        HitChance = DefaultHitChanceCalculation;

        Power = (s, t) => power;

        DamageBehaviour = DefaultDamageBehaviour;

        modifiers?.ForEach(e => e.LoadEffect(this));
        effects?.ForEach(e => e.LoadEffect(this));

    }

    public IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if (Category != MoveCategory.Status)
        {
            yield return DamageBehaviour(source, target);
            foreach (var e in effects)
            {
                yield return e.Run(source, target);
            }
        }

        else
        {
            yield return Behaviour(source, target);
        }
    }
    public IEnumerator DefaultDamageBehaviour(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        yield return dialogBox.TypeDialog($"{source.Name} used {Name}");
        source.LastUsedMove.PP--;

        if (ImmunityCheck(source, target))
        {
            yield return dialogBox.TypeDialog($"It doesn't affect {target.Name}!");
            yield return OnFail?.Invoke(source, target);
            yield break;
        }


        yield return AccuracyCheck(source, target);
        if (source.EndTurn)
        {
            source.EndTurn = false;
            yield return OnFail?.Invoke(source, target);
            yield break;
        }

        int count = HitCount(source, target);
        for (int i = 0; i < count; i++)
        {
            int damage = Damage(source, target);

            yield return target.TakeDamage(damage);

            yield return target.OnHit(this, source, damage);

            foreach (var e in SecondaryEffectList)
            {
                yield return e(source, target);
            }

            yield return Recoil?.Invoke(damage, source, target);
        }



        if (count > 1)
            yield return dialogBox.TypeDialog($"Hit {count} time(s)!!");

    }
    public IEnumerator Behaviour(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        yield return dialogBox.TypeDialog($"{source.Name} used {Name}");
        source.LastUsedMove.PP--;

        yield return AccuracyCheck(source, target);
        if (source.EndTurn)
        {
            source.EndTurn = false;
            yield break;
        }


        foreach (var e in effects)
        {
            yield return e.Run(source, target);
        }
    }

    //true if target immune
    bool ImmunityCheck(BattleUnit source, BattleUnit target)
    {
        bool extra = ExtraImmunityCheck(source, target);

        if (extra) return extra;

        float typeMod = TypeChart.GetEffectiveness(type, target.Types);

        if (typeMod == 0)
            return true;

        return false;
    }

    IEnumerator AccuracyCheck(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        bool invulnerable = target.Invulnerability(this, source);

        if (target.ByPassAccuracyCheck(this, source))
            yield break;

        if (invulnerable || (Random.value >= HitChance(source, target) && accuracy != 0))
        {
            yield return dialogBox.TypeDialog($"{source.Name} missed!");
            source.EndTurn = true;
        }
    }

    int DefaultDamageCalculation(BattleUnit source, BattleUnit target)
    {
        int damage = 0;

        float critMod = Random.value < CritRate(source, target) ? 1.5f : 1f;
        float randMod = Random.Range(0.85f, 1f);
        float typeMod = TypeChart.GetEffectiveness(type, target.Types);
        float STAB = source.Types.Contains(type) ? 1.5f : 1f;

        int attack = Attack(source, target);
        int defense = Defense(source, target);

        float a = (2 * source.Level + 10) / 250f;
        float d = a * Power(source, target) * ((float)attack / defense) + 2;

        float attackMod = source.AttackerMod(this, target);
        float defenseMod = target.DefenderMod(this, source);

        damage = Mathf.FloorToInt(d * critMod * randMod * typeMod * attackMod * defenseMod);

        if (critMod > 1f)
            BattleSystem.Instance.MessageQueue.Enqueue("A critical hit!");
        if (typeMod > 1f)
            BattleSystem.Instance.MessageQueue.Enqueue("It's super effective!");
        if (typeMod < 1f)
            BattleSystem.Instance.MessageQueue.Enqueue("It's not very effective!");
        //if (typeMod == 0)
        //    system.MessageQueue.Enqueue($"It doesn't affect {target.Name}!");

        return damage;
    }

    float DefaultHitChanceCalculation(BattleUnit source, BattleUnit target)
    {
        float acc = accuracy / 100f * source.Accuracy * target.Evasion;

        return acc;
    }
    int DefaultAttackCalculation(BattleUnit source, BattleUnit target)
    {
        return category == MoveCategory.Physical ? source.Attack : source.SpAttack;
    }
    int DefaulDefenseCalculation(BattleUnit source, BattleUnit target)
    {
        return category == MoveCategory.Physical ? target.Defense : target.SpDefense;
    }

    float CritRate(BattleUnit source, BattleUnit target)
    {
        int stage = Mathf.Clamp(CritStage(source, target) + source.CritStage, 0, 3);

        return critTable[stage];
    }
}



public enum MoveCategory
{
    Physical, Special, Status
}

public enum MoveTarget
{
    Foe, Self
}
