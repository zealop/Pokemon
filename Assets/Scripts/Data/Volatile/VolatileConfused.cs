using Battle;
using Data;
using UnityEngine;

public class VolatileConfused : VolatileCondition
{
    public override VolatileID ID => VolatileID.Thrash;

    private const int Power = 40;

    private int counter;

    public override void OnStart()
    {
        counter = Random.Range(2, 6);

        //Unit.OnBeforeMoveList.Add(Confusion);

        BattleManager.I.DialogBox.TypeDialog($"{unit.Name} became confused due to fatigue!");
    }

    public override void OnEnd()
    {
        //Unit.OnBeforeMoveList.Remove(Confusion);
    }

    private void Confusion()
    {
        BattleManager.I.DialogBox.TypeDialog($"{unit.Name} is confused!");
        if (Random.value < 0.33f)
        {
            BattleManager.I.DialogBox.TypeDialog("It hurt it self in its confusion!");
            unit.CanMove = false;
            // Unit.TakeDamage(damage());
        }
    }

    private int damage()
    {
        int damage = 0;

        float randMod = Random.Range(0.85f, 1f);

        int attack = unit.Attack;
        int defense = unit.Defense;

        float a = (2 * unit.Level + 10) / 250f;
        float d = a * Power * ((float) attack / defense) + 2;

        damage = Mathf.FloorToInt(d * randMod);

        return damage;
    }
}