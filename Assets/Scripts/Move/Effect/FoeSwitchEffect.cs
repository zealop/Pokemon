using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoeSwitchEffect : MoveEffect
{
    [SerializeField] string message;
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if (target.IsPlayerUnit)
        {
            if (BattleSystem.Instance.PlayerParty.Count < 2)
            {
                yield return BattleSystem.Instance.DialogBox.TypeDialog("But it failed!");
                yield break;
            }

            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{target.Name} {message}");
            var newPokemon = BattleSystem.Instance.PlayerParty.GetRandomPokemon(source.Pokemon);

            target.Setup(newPokemon);
            BattleSystem.Instance.DialogBox.SetMoveName(newPokemon.Moves);

            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{target.Name} is dragged out!");
            yield break;
        }
        else
        {
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{target.Name} {message}");
            BattleSystem.Instance.BattleOver(true);
        }
        

        //if(target.IsPlayerUnit)

    }
}

public class MimicEffect : MoveEffect
{
    static readonly string[] disallows = { "Chatter", "Mimic", "Sketch", "Struggle", "Transform" };
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        var move = target.LastUsedMove;

        if (move is null || disallows.Contains(move.Base.Name) || source.Pokemon.KnowMove(move.Base))
        {
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"But it failed!");
            yield break;
        }

        int mimicIndex = source.Pokemon.IndexofMove(_base);
        if (mimicIndex < 0) yield break;

        source.Moves[mimicIndex] = new Move(move.Base);
        if (source.IsPlayerUnit)
            BattleSystem.Instance.DialogBox.SetMoveName(source.Moves);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{source.Name} learned {move.Base.Name}");

    }
}

public class HealEffect : MoveEffect
{
    [SerializeField] float ratio;
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return source.TakeDamage(-Mathf.FloorToInt(source.MaxHP * ratio));

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{source.Name} regained health!");

    }
}


public class HazeEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        source.StatStage = new StatStage();
        target.StatStage = new StatStage();

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"All stat changes are eliminated!");
    }
}

public class PumpEffect : MoveEffect
{
    [SerializeField] int crit = 2;
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    { 
        source.StatStage.CritStage = crit;

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{source.Name} is getting pumped!");
    }
}

public class SuicideEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return source.TakeDamage(source.MaxHP);
    }
}

public class TransformEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return source.AddVolatileCondition(new VolatileTransform(target));
    }
}

public class SplashEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return BattleSystem.Instance.DialogBox.TypeDialog("But nothing happened!");
    }
}

public class RestEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if(source.Status.ID == StatusID.SLP || source.HP == source.MaxHP)
        {
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"But it failed!");
            yield break;
        }
        source.Status = new StatusSleep(2);

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{source.Name} slept and became healthy!");
    }
}
public class ConversionEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        var dialogBox = BattleSystem.Instance.DialogBox;

        var moveType = source.Moves[0].Base.Type;

        if (source.Types.Contains(moveType))
        {
            yield return dialogBox.TypeDialog($"But it failed!");
            yield break;
        }

        source.Types = new List<PokemonType>() { moveType};

        yield return dialogBox.TypeDialog($"{source.Name} transformed into the {moveType} type!");
    }
}