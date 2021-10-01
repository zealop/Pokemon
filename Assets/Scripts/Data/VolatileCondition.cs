using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum VolatileID
{
    Dig, Dive, Fly, Charging, Recharge, Bide,
    Bound, Flinch, LockedMove,
    Confused, Disabled, Seeded,
    Counter, Rage,
    Minimize,
    Transform,
}

public class VolatileCondition
{
    public virtual VolatileID ID { get; }
    public BattleUnit Unit { get; set; }

    public virtual IEnumerator OnStart()
    {
        yield return null;
    }
    public virtual IEnumerator OnEnd()
    {
        yield return null;
    }
}

public class VolatileCharging : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Charging; }
    public override IEnumerator OnStart()
    {
        Unit.LockedAction = true;
        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.LockedAction = false;
        yield return null;
    }
}

public class VolatileRecharge : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Recharge; }

    int counter;
    public override IEnumerator OnStart()
    {
        Unit.LockedAction = true;
        counter = 2;

        Unit.OnTurnEndList.Add(Recharge);

        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.LockedAction = false;

        Unit.OnTurnEndList.Remove(Recharge);

        yield return null;
    }

    IEnumerator Recharge()
    {
        counter--;
        if (counter == 0)
        {
            yield return Unit.RemoveVolatileCondition(ID);
        }
        else
        {
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} must recharge!");
        }
    }
}
public class VolatileFly : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Fly; }

    static readonly string[] normal = { "Hurricane", "Sky Uppercut", "Smack Down", "Thousand Arrows", "Thunder" };
    static readonly string[] doubled = { "Gust", "Twister" };
    public override IEnumerator OnStart()
    {
        Unit.LockedAction = true;

        Unit.Invulnerability += SemiInvulnerable;
        Unit.DefenderModList.Add(WindyFlight);

        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.LockedAction = false;

        Unit.Invulnerability -= SemiInvulnerable;
        Unit.DefenderModList.Remove(WindyFlight);

        yield return null;
    }

    bool SemiInvulnerable(MoveBase move, BattleUnit source)
    {
        if (normal.Concat(doubled).Contains(move.Name))
            return false;

        return true;
    }

    float WindyFlight(MoveBase move, BattleUnit source)
    {
        if (doubled.Contains(move.Name))
            return 2f;

        return 1f;
    }
}

public class VolatileBide : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Bide; }

    public int Counter;
    public int StoredDamage;
    public override IEnumerator OnStart()
    {
        Counter = 2;
        Unit.LockedAction = true;

        Unit.OnHitList.Add(StoreDamage);

        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.LockedAction = false;

        Unit.OnHitList.Remove(StoreDamage);

        yield return null;
    }

    IEnumerator StoreDamage(MoveBase move, BattleUnit source, int damage)
    {
        StoredDamage += damage;

        yield return null;
    }

}
public class VolatileDig : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Dig; }

    static readonly string[] doubled = { "Magnitude", "Earthquake", "Fissure" };
    public override IEnumerator OnStart()
    {
        Unit.LockedAction = true;

        Unit.Invulnerability += SemiInvulnerable;
        Unit.DefenderModList.Add(Underground);

        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.LockedAction = false;

        Unit.Invulnerability -= SemiInvulnerable;
        Unit.DefenderModList.Remove(Underground);

        yield return null;
    }

    bool SemiInvulnerable(MoveBase move, BattleUnit source)
    {
        if (doubled.Contains(move.Name))
            return false;

        return true;
    }

    float Underground(MoveBase move, BattleUnit source)
    {
        if (doubled.Contains(move.Name))
            return 2f;

        return 1f;
    }
}

//Bind,Clamp, FireSpin,....
public class VolatileBound : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Bound; }

    int counter;
    static float damage = 1 / 8f;

    BattleUnit source;
    MoveBase move;

    public VolatileBound(MoveBase move, BattleUnit source)
    {
        counter = Random.Range(4, 6);

        this.move = move;
        this.source = source;
    }
    public override IEnumerator OnStart()
    {
        Unit.OnTurnEndList.Add(ResidualDamage);
        Unit.CanSwitch += Trapped;
        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnTurnEndList.Remove(ResidualDamage);
        Unit.CanSwitch -= Trapped;
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is freed from {move.Name}!");
    }

    IEnumerator ResidualDamage()
    {
        if (counter > 0)
        {
            counter--;
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is hurt by {move.Name}!");
            yield return Unit.TakeDamage(Mathf.FloorToInt(Unit.MaxHP * damage));
            yield break;
        }

        yield return Unit.RemoveVolatileCondition(ID);
    }

    bool Trapped()
    {
        if (Unit.Types.Contains(PokemonType.Ghost))
            return true;

        return false;
    }
}

public class VolatileFlinch : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Flinch; }
    public override IEnumerator OnStart()
    {
        Unit.OnBeforeMoveList.Add(Flinched);
        Unit.OnTurnEndList.Add(Unflinch);

        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnBeforeMoveList.Remove(Flinched);
        Unit.OnTurnEndList.Remove(Unflinch);

        yield return null;
    }

    IEnumerator Flinched()
    {
        Unit.CanMove = false;
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} flinched!");
    }
    IEnumerator Unflinch()
    {
        yield return Unit.RemoveVolatileCondition(ID);
    }
}

public class VolatileLockedMove : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.LockedMove; }

    int counter;

    public override IEnumerator OnStart()
    {
        counter = Random.Range(1, 3);
        Unit.LastUsedMove = new Move(Unit.LastUsedMove.Base);
        Unit.LockedAction = true;


        Unit.OnTurnEndList.Add(Rampage);
        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.LockedAction = false;

        Unit.OnTurnEndList.Remove(Rampage);
        yield return null;
    }

    IEnumerator Rampage()
    {
        if (!Unit.CanMove)
        {
            yield return Unit.RemoveVolatileCondition(ID);
        }
        else if (counter == 0)
        {
            yield return Unit.RemoveVolatileCondition(ID);
            yield return Unit.AddVolatileCondition(new VolatileConfused());
        }
        else
        {
            counter--;
        }
    }
}

public class VolatileConfused : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.LockedMove; }

    readonly static int power = 40;

    int counter;

    public override IEnumerator OnStart()
    {
        counter = Random.Range(2, 6);

        Unit.OnBeforeMoveList.Add(Confusion);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} became confused due to fatigue!");
    }
    public override IEnumerator OnEnd()
    {

        Unit.OnBeforeMoveList.Remove(Confusion);

        yield return null;
    }

    IEnumerator Confusion()
    {
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} is confused!");
        if (Random.value < 0.33f)
        {
            yield return BattleSystem.Instance.DialogBox.TypeDialog("It hurt it self in its confusion!");
            Unit.CanMove = false;
            yield return Unit.TakeDamage(damage());
        }
    }

    int damage()
    {
        int damage = 0;

        float randMod = Random.Range(0.85f, 1f);

        int attack = Unit.Attack;
        int defense = Unit.Defense;

        float a = (2 * Unit.Level + 10) / 250f;
        float d = a * power * ((float)attack / defense) + 2;

        damage = Mathf.FloorToInt(d * randMod);

        return damage;
    }
}

public class VolatileDisabled : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Disabled; }

    Move disabled;
    public override IEnumerator OnStart()
    {
        Unit.LastUsedMove.Disabled = true;
        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.LastUsedMove.Disabled = false; ;
        yield return null;
    }
}

public class VolatileCounter : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Counter; }

    public int StoredDamage;
    public override IEnumerator OnStart()
    {
        Unit.OnHitList.Add(StoreDamage);
        Unit.OnTurnEndList.Add(Clear);

        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnHitList.Remove(StoreDamage);
        Unit.OnTurnEndList.Remove(Clear);

        yield return null;
    }

    IEnumerator StoreDamage(MoveBase move, BattleUnit source, int damage)
    {
        if (move.Category == MoveCategory.Physical)
            StoredDamage += damage;

        yield return null;
    }

    IEnumerator Clear()
    {
        yield return Unit.RemoveVolatileCondition(ID);
    }
}

public class VolatileSeeded : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Seeded; }

    readonly static float ratio = 1 / 8f;

    BattleUnit source;
    public override IEnumerator OnStart()
    {
        Unit.OnTurnEndList.Add(Drain);
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} was seeded!");
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnTurnEndList.Remove(Drain);
        yield return null;
    }

    IEnumerator Drain()
    {
        int damage = Mathf.FloorToInt(Unit.MaxHP * ratio);

        yield return Unit.TakeDamage(damage);
        yield return source.TakeDamage(-damage);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} was seeded!");
    }
}

public class VolatileRage : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Rage; }

    public int StoredDamage;

    static readonly Dictionary<BoostableStat, int> boost = new Dictionary<BoostableStat, int>()
    {
        {BoostableStat.Attack, 1 }
    };

    public override IEnumerator OnStart()
    {
        Unit.OnHitList.Add(Rage);
        Unit.OnTurnEndList.Add(Clear);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} started building its rage!");
    }
    public override IEnumerator OnEnd()
    {
        Unit.OnHitList.Remove(Rage);
        Unit.OnTurnEndList.Remove(Clear);

        yield return null;
    }

    IEnumerator Rage(MoveBase move, BattleUnit source, int damage)
    {
        yield return Unit.ApplyStatBoost(boost);
    }

    IEnumerator Clear()
    {
        if (Unit.LastUsedMove.Base.Name != "Rage")
            yield return Unit.RemoveVolatileCondition(ID);
    }


}

public class VolatileMinimize : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Minimize; }

    static readonly string[] big = { "Body Slam", "Stomp", "Dragon Rush", "Steamroller", "Heat Crash", "Heavy Slam", "Flying Press", "Malicious Moonsault" };
    public override IEnumerator OnStart()
    {
        Unit.DefenderModList.Add(Small);
        Unit.ByPassAccuracyCheckList.Add(Tiny);

        yield return null;
    }
    public override IEnumerator OnEnd()
    {
        Unit.DefenderModList.Remove(Small);
        Unit.ByPassAccuracyCheckList.Remove(Tiny);

        yield return null;
    }

    float Small(MoveBase move, BattleUnit source)
    {
        if (big.Contains(move.Name))
            return 2;

        return 1;
    }

    bool Tiny(MoveBase move, BattleUnit source)
    {
        if (big.Contains(move.Name))
            return true;

        return false;
    }
}

public class VolatileTransform : VolatileCondition
{
    public override VolatileID ID { get => VolatileID.Transform; }

    BattleUnit target;
    public VolatileTransform(BattleUnit unit)
    {
        target = unit;
    }

    public override IEnumerator OnStart()
    {
        Unit.Transform(target.Pokemon);
        Unit.StatStage = target.StatStage;

        Unit.Moves.Clear();
        foreach (var move in target.Moves)
        {
            var newMove = new Move(move.Base);
            newMove.PP = 5;
            Unit.Moves.Add(newMove);
        }
        BattleSystem.Instance.MoveSelector.SetMoves(Unit.Moves);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Unit.Name} transformed into {target.Name}");
    }
    public override IEnumerator OnEnd()
    {


        yield return null;
    }

}