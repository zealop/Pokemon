using System.Collections;
using UnityEngine;

public class BoundEffect : MoveEffect
{
    [SerializeField] string message;

    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if (source.EndTurn) yield break;

        VolatileCondition bound = new VolatileBound(_base, source);

        if (!target.Volatiles.ContainsKey(VolatileID.Bound))
        {
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{message.Replace("<source>", source.Name).Replace("<target>", target.Name)}");
            yield return target.AddVolatileCondition(bound);
        }

    }
}

public class FlinchEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if (!target.Volatiles.ContainsKey(VolatileID.Flinch))
        {
            yield return target.AddVolatileCondition(new VolatileFlinch());
        }

    }
}

public class ConfusionEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if (!target.Volatiles.ContainsKey(VolatileID.Confused))
        {
            yield return target.AddVolatileCondition(new VolatileConfused());
        }

    }
}

public class DisableEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if (target.LastUsedMove is null)
        {
            yield return BattleSystem.Instance.DialogBox.TypeDialog("But it failed!");
        }


        if (target.Volatiles.ContainsKey(VolatileID.Disabled))
        {
            yield return target.RemoveVolatileCondition(VolatileID.Disabled);
        }

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{target.Name}'s {target.LastUsedMove.Base.Name} is disabled!");
        yield return target.AddVolatileCondition(new VolatileDisabled());

    }
}

public class RechargeEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {

        yield return target.AddVolatileCondition(new VolatileRecharge());


    }
}

public class SeededEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if (!target.Volatiles.ContainsKey(VolatileID.Seeded))
            yield return target.AddVolatileCondition(new VolatileSeeded());
        else
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{target.Name} is already seeded!");

    }
}

public class RageEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {

        yield return target.AddVolatileCondition(new VolatileRage());


    }
}

public class TeleportEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{source.Name} fled from battle!");
        BattleSystem.Instance.BattleOver(true);
    }
}

public class MinimizeEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if (!source.Volatiles.ContainsKey(VolatileID.Minimize))
            yield return target.AddVolatileCondition(new VolatileMinimize());

    }
}

