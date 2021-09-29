using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusID { PSN, BRN, SLP, PRZ, FRZ, TOX }
public class StatusCondition
{
    public virtual StatusID ID { get; }
    public BattleUnit Unit { get; set; }
    public static StatusCondition Create(StatusID id)
    {
        return id switch
        {
            StatusID.PSN => new StatusPoison(),
            StatusID.BRN => new StatusBurn(),
            StatusID.SLP => new StatusSleep(),
            StatusID.PRZ => new StatusParalyze(),
            StatusID.FRZ => new StatusFreeze(),
            StatusID.TOX => new StatusToxic(),
            _ => null,
        };
    }
    public static float CatchBonus(StatusCondition condition)
    {
        if (condition is null) return 1;

        return condition.ID switch
        {
            StatusID.PSN => 1.5f,
            StatusID.BRN => 1.5f,
            StatusID.SLP => 2f,
            StatusID.PRZ => 2f,
            StatusID.FRZ => 1.5f,
            StatusID.TOX => 1.5f,
            _ => 1f,
        };
    }


    public virtual IEnumerator OnStart()
    {
        yield return null;
    }
    public virtual IEnumerator OnEnd()
    {
        yield return null;
    }
}

public class StatusPoison : StatusCondition
{
    public override StatusID ID { get => StatusID.PSN; }

    static float damage = 1 / 8f;


    public override IEnumerator OnStart()
    {

        Unit.OnTurnEndList.Add(PoisonDamage);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is poisoned!");
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnTurnEndList.Remove(PoisonDamage);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is cured of its poison!");
    }

    IEnumerator PoisonDamage()
    {
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is hurt by poison!");
        yield return Unit.TakeDamage(Mathf.FloorToInt(Unit.MaxHP * damage));
    }
}

public class StatusBurn : StatusCondition
{
    public override StatusID ID { get => StatusID.BRN; }

    static float damage = 1 / 16f;

    public override IEnumerator OnStart()
    {
        Unit.AttackerModList.Add(BurnMod);
        Unit.OnTurnEndList.Add(BurnDamage);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is burned!");
    }
    public override IEnumerator OnEnd()
    {
        Unit.AttackerModList.Remove(BurnMod);
        Unit.OnTurnEndList.Remove(BurnDamage);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is cured of its burn!");
    }

    IEnumerator BurnDamage()
    {
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is hurt by its burn!");
        yield return Unit.TakeDamage(Mathf.FloorToInt(Unit.MaxHP * damage));
    }

    float BurnMod(MoveBase move, BattleUnit target)
    {
        if (move.Category == MoveCategory.Physical)
            return 0.5f;

        return 1f;
    }
}
public class StatusSleep : StatusCondition
{
    public override StatusID ID { get => StatusID.SLP; }


    int counter;

    public StatusSleep()
    {
        counter = Random.Range(1, 4);
    }
    public StatusSleep(int value)
    {
        counter = value;
    }
    public override IEnumerator OnStart()
    {
        Unit.OnBeforeMoveList.Add(SleepCheck);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} fell asleep!");
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnBeforeMoveList.Remove(SleepCheck);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} woke up!");
    }

    IEnumerator SleepCheck()
    {
        if (counter > 0)
        {
            counter--;
            Unit.CanMove = false;
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is fast asleep!");
            yield break;
        }

        yield return Unit.RemoveStatusCondition();

    }
}

public class StatusParalyze : StatusCondition
{
    public override StatusID ID { get => StatusID.PRZ; }


    static float speed = 0.5f;
    static float chance = 0.25f;

    public override IEnumerator OnStart()
    {
        Unit.OnBeforeMoveList.Add(ParalyzeCheck);
        Unit.SpeedModList.Add(ParalyzeSlow);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is paralyzed!");
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnBeforeMoveList.Remove(ParalyzeCheck);
        Unit.SpeedModList.Remove(ParalyzeSlow);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is cured of its paralysis!");
    }

    IEnumerator ParalyzeCheck()
    {
        if (Random.value < chance)
        {
            Unit.CanMove = false;
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is fully paralyzed!");
            yield break;
        }
    }

    float ParalyzeSlow()
    {
        return speed;
    }
}

public class StatusFreeze : StatusCondition
{
    public override StatusID ID { get => StatusID.FRZ; }


    static float thaw = 0.2f;

    public override IEnumerator OnStart()
    {
        Unit.OnBeforeMoveList.Add(FreezeCheck);
        Unit.OnHitList.Add(FireHit);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is frozen solid!");
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnBeforeMoveList.Remove(FreezeCheck);
        Unit.OnHitList.Remove(FireHit);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} thawed out!");
    }

    IEnumerator FreezeCheck()
    {
        if (Random.value < thaw)
        {
            yield return Unit.RemoveStatusCondition();
            yield break;
        }

        Unit.CanMove = false;
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is frozen solid!");
    }

    IEnumerator FireHit(MoveBase move, BattleUnit source, int damage)
    {
        if (move.Type == PokemonType.Fire)
            yield return Unit.RemoveStatusCondition();
    }
}

public class StatusToxic : StatusCondition
{
    public override StatusID ID { get => StatusID.TOX; }


    static float damage = 1 / 16f;
    int counter;

    public override IEnumerator OnStart()
    {
        counter = 1;
        Unit.OnTurnEndList.Add(PoisonDamage);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is badly poisoned!");
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnTurnEndList.Add(PoisonDamage);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is cured of its poison!");
    }

    IEnumerator PoisonDamage()
    {
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is hurt by poison!");
        yield return Unit.TakeDamage(Mathf.FloorToInt(Unit.MaxHP * damage * counter));
    }
}