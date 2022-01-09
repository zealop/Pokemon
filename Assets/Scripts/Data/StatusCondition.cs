using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusID
{
    PSN,
    BRN,
    SLP,
    PRZ,
    FRZ,
    TOX
}

public abstract class StatusCondition
{
    public abstract StatusID ID { get; }
    protected readonly BattleUnit unit;
    protected static Queue<IEnumerator> AnimationQueue => BattleManager.Instance.AnimationQueue;
    protected static BattleDialogBox DialogBox => BattleManager.Instance.DialogBox;

    protected StatusCondition(BattleUnit unit)
    {
        this.unit = unit;
    }

    public static StatusCondition Create(StatusID id, BattleUnit unit)
    {
        return id switch
        {
            StatusID.PSN => new StatusPoison(unit),
            StatusID.BRN => new StatusBurn(unit),
            StatusID.SLP => new StatusSleep(unit),
            StatusID.PRZ => new StatusParalyze(unit),
            StatusID.FRZ => new StatusFreeze(unit),
            StatusID.TOX => new StatusToxic(unit),
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

    public abstract void OnStart();
    public abstract void OnEnd();
}

public class StatusPoison : StatusCondition
{
    public override StatusID ID => StatusID.PSN;
    private const float DamageModifier = 1 / 8f;

    public StatusPoison(BattleUnit unit) : base(unit)
    {
    }

    public override void OnStart()
    {
        unit.OnTurnEndList.Add(PoisonDamage);

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is poisoned!"));
    }

    public override void OnEnd()
    {
        unit.OnTurnEndList.Remove(PoisonDamage);

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is cured of its poison!"));
    }

    private void PoisonDamage()
    {
        int damage = Mathf.FloorToInt(unit.MaxHP * DamageModifier);
        unit.TakeDamage(damage, $"{unit.Name} is hurt by poison!");
    }
}

public class StatusBurn : StatusCondition
{
    public override StatusID ID => StatusID.BRN;
    private const float DamageModifier = 1 / 16f;
    private const float AttackModifier = 1 / 2f;

    public StatusBurn(BattleUnit unit) : base(unit)
    {
    }

    public override void OnStart()
    {
        Restart();

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is burned!"));
    }

    public override void OnEnd()
    {
        unit.AttackerModList.Remove(BurnMod);
        unit.OnTurnEndList.Remove(BurnDamage);

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is cured of its burn!"));
    }

    private void BurnDamage()
    {
        int damage = Mathf.FloorToInt(unit.MaxHP * DamageModifier);
        unit.TakeDamage(damage, $"{unit.Name} is hurt by its burn!");
    }

    private static float BurnMod(MoveBase move, BattleUnit target)
    {
        return move.Category == MoveCategory.Physical ? AttackModifier : 1f;
    }

    public virtual void Restart()
    {
        unit.AttackerModList.Add(BurnMod);
        unit.OnTurnEndList.Add(BurnDamage);
    }
}

public class StatusSleep : StatusCondition
{
    public override StatusID ID => StatusID.SLP;
    private int counter;

    public StatusSleep(BattleUnit unit) : this(Random.Range(1, 4), unit)
    {
    }

    public StatusSleep(int value, BattleUnit unit) : base(unit)
    {
        counter = value;
    }

    public override void OnStart()
    {
        Restart();

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} fell asleep!"));
    }

    public override void OnEnd()
    {
        unit.OnBeforeMoveList.Remove(SleepCheck);

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} woke up!"));
    }

    private void SleepCheck()
    {
        if (counter > 0)
        {
            counter--;
            unit.CanMove = false;

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is fast asleep!"));
        }
        else
        {
            unit.RemoveStatusCondition();
        }
    }

    public virtual void Restart()
    {
        unit.OnBeforeMoveList.Add(SleepCheck);
    }
}

public class StatusParalyze : StatusCondition
{
    public override StatusID ID => StatusID.PRZ;
    private const float SpeedModifier = 0.5f;
    private const float ParalyzeChance = 0.25f;

    public StatusParalyze(BattleUnit unit) : base(unit)
    {
    }

    public override void OnStart()
    {
        Restart();

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is paralyzed!"));
    }

    public override void OnEnd()
    {
        unit.OnBeforeMoveList.Remove(ParalyzeCheck);
        unit.SpeedModList.Remove(ParalyzeSlow);

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is cured of its paralysis!"));
    }

    private void ParalyzeCheck()
    {
        if (Random.value <= ParalyzeChance)
        {
            unit.CanMove = false;

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is fully paralyzed!"));
        }
    }

    private float ParalyzeSlow()
    {
        return SpeedModifier;
    }

    public virtual void Restart()
    {
        unit.OnBeforeMoveList.Add(ParalyzeCheck);
        unit.SpeedModList.Add(ParalyzeSlow);
    }
}

public class StatusFreeze : StatusCondition
{
    public override StatusID ID => StatusID.FRZ;
    private const float ThawChance = 0.2f;

    public StatusFreeze(BattleUnit unit) : base(unit)
    {
    }

    public override void OnStart()
    {
        Restart();

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is frozen solid!"));
    }

    public override void OnEnd()
    {
        unit.OnBeforeMoveList.Remove(FreezeCheck);
        unit.OnHitList.Remove(FireHit);

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} thawed out!"));
    }

    private void FreezeCheck()
    {
        if (Random.value <= ThawChance)
        {
            unit.RemoveStatusCondition();
        }
        else
        {
            unit.CanMove = false;
            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is frozen solid!"));
        }
    }

    private void FireHit(MoveBase move, BattleUnit source, int damage)
    {
        if (move.Type == PokemonType.Fire)
        {
            unit.RemoveStatusCondition();
        }
    }

    public virtual void Restart()
    {
        unit.OnBeforeMoveList.Add(FreezeCheck);
        unit.OnHitList.Add(FireHit);
    }
}

public class StatusToxic : StatusCondition
{
    public override StatusID ID => StatusID.TOX;
    private const float DamageModifier = 1 / 16f;
    private int counter;

    public StatusToxic(BattleUnit unit) : base(unit)
    {
    }

    public override void OnStart()
    {
        Restart();

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is badly poisoned!"));
    }

    public override void OnEnd()
    {
        unit.OnTurnEndList.Add(ToxicDamage);

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} is cured of its poison!"));
    }

    private void ToxicDamage()
    {
        int damage = Mathf.FloorToInt(unit.MaxHP * DamageModifier * counter);
        unit.TakeDamage(damage, $"{unit.Name} is hurt by poison!");
    }

    public virtual void Restart()
    {
        counter = 1;
        unit.OnTurnEndList.Add(ToxicDamage);
    }
}