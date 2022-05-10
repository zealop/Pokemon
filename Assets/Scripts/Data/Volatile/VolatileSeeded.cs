using Battle;
using Data;
using UnityEngine;

public class VolatileSeeded : VolatileCondition
{
    public override VolatileID ID => VolatileID.Seeded;

    private const float ratio = 1 / 8f;

    private Unit source;

    public override void OnStart()
    {
        //Unit.OnTurnEndList.Add(Drain);
        BattleManager.I.DialogBox.TypeDialog($"{unit.Name} was seeded!");
    }

    public override void OnEnd()
    {
        //Unit.OnTurnEndList.Remove(Drain);
    }

    private void Drain()
    {
        int damage = Mathf.FloorToInt(unit.MaxHp * ratio);

        // Unit.TakeDamage(damage);
        // source.TakeDamage(-damage);

        BattleManager.I.DialogBox.TypeDialog($"{unit.Name} was seeded!");
    }
}