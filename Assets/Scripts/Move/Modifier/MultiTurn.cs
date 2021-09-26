using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTurn : MoveModifier
{
    [SerializeField] public string message;
    public override void ModifyMove()
    {
        _base.DamageBehaviour = Charge;
    }
    IEnumerator Charge(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        if (!source.Volatiles.ContainsKey(VolatileID.Charging))
        {
            yield return dialogBox.TypeDialog($"{source.Name} {message}");


            yield return source.AddVolatileCondition(new VolatileCharging());
        }
        else
        {
            yield return source.RemoveVolatileCondition(VolatileID.Charging);

            yield return _base.DefaultDamageBehaviour(source, target);
        }

    }
}

public class MultiTurnFly : MultiTurn
{
    public override void ModifyMove()
    {
        _base.DamageBehaviour = Charge;
    }

    IEnumerator Charge(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        if (!source.Volatiles.ContainsKey(VolatileID.Fly))
        {
            yield return dialogBox.TypeDialog($"{source.Name} {message}");


            yield return source.AddVolatileCondition(new VolatileFly());
        }
        else
        {
            yield return source.RemoveVolatileCondition(VolatileID.Fly);

            yield return _base.DefaultDamageBehaviour(source, target);
        }

    }
}

public class MultiTurnDig : MultiTurn
{
    public override void ModifyMove()
    {
        _base.DamageBehaviour = Charge;
    }

    IEnumerator Charge(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        if (!source.Volatiles.ContainsKey(VolatileID.Dig))
        {
            yield return dialogBox.TypeDialog($"{source.Name} {message}");


            yield return source.AddVolatileCondition(new VolatileDig());
        }
        else
        {
            yield return source.RemoveVolatileCondition(VolatileID.Dig);

            yield return _base.DefaultDamageBehaviour(source, target);
        }

    }
}

public class MultiTurnBash : MultiTurn
{
    [SerializeField] MoveEffect effect;
    public override void ModifyMove()
    {
        _base.DamageBehaviour = Charge;
    }

    IEnumerator Charge(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        if (!source.Volatiles.ContainsKey(VolatileID.Charging))
        {
            yield return dialogBox.TypeDialog($"{source.Name} {message}");

            yield return effect.Run(source, target);
            yield return source.AddVolatileCondition(new VolatileCharging());
        }
        else
        {
            yield return source.RemoveVolatileCondition(VolatileID.Charging);

            yield return _base.DefaultDamageBehaviour(source, target);
        }

    }
}

public class MultiTurnBide : MultiTurn
{
    [SerializeField] int multiplier = 2;
    public override void ModifyMove()
    {
        _base.DamageBehaviour = Endure;
        _base.Damage = Unleash;
    }

    IEnumerator Endure(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        

        if (!source.Volatiles.ContainsKey(VolatileID.Bide))
        {
            yield return dialogBox.TypeDialog($"{source.Name} {message}!");

            yield return source.AddVolatileCondition(new VolatileBide());
        }
        else
        {
            var bide = (VolatileBide)source.Volatiles[VolatileID.Bide];

            if (bide.Counter > 1)
            {
                bide.Counter--;
                yield return dialogBox.TypeDialog($"{source.Name} {message}!");
            }
            else
            {
                yield return dialogBox.TypeDialog($"{source.Name} uleashed its energy!");

                yield return _base.DefaultDamageBehaviour(source, target);

                yield return source.RemoveVolatileCondition(VolatileID.Bide);
               
            }

            
        }

    }

    int Unleash(BattleUnit source, BattleUnit target)
    {
        var bide = (VolatileBide)source.Volatiles[VolatileID.Bide];
        return bide.StoredDamage * multiplier;
    }
}