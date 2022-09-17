using System.Linq;
using Battle;
using Data;

public class VolatileTransform : VolatileCondition
{
    public override VolatileID ID => VolatileID.Transform;

    private Unit target;

    public VolatileTransform(Unit unit)
    {
        target = unit;
    }

    public override void OnStart()
    {
        unit.Transform(target.Pokemon);
        unit.StatStage = target.StatStage;

        unit.Moves.Clear();
        foreach (var newMove in target.Moves.Select(move => new Move.MoveSlot(move.Base, 5)))
        {
            unit.Moves.Add(newMove);
        }

        BattleManager.i.MoveSelector.SetMoves(unit.Moves);

        BattleManager.i.DialogBox.TypeDialog($"{unit.Name} transformed into {target.Name}");
    }

    public override void OnEnd()
    {
         
    }
}