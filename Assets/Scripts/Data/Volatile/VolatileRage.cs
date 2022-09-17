using Battle;
using Data;
using Move;

public class VolatileRage : VolatileCondition
{
    public override VolatileID ID => VolatileID.Rage;

    public int StoredDamage;

    //static readonly Dictionary<BoostableStat, int> boost = new Dictionary<BoostableStat, int>()
    //{
    //    {BoostableStat.Attack, 1 }
    //};

    public override void OnStart()
    {
        //Unit.OnHitList.Add(Rage);
        //Unit.OnTurnEndList.Add(Clear);

        BattleManager.i.DialogBox.TypeDialog($"{unit.Name} started building its rage!");
    }

    public override void OnEnd()
    {
        //Unit.OnHitList.Remove(Rage);
        //Unit.OnTurnEndList.Remove(Clear);

        ;
    }

    private void Rage(MoveBase move, Unit source, int damage)
    {
        // Unit.ApplyStatBoost(boost);
        ;
    }

    private void Clear()
    {
        // if (unit.LastUsedMove.Base.Name != "Rage")
            // unit.RemoveVolatileCondition(ID);
    }
}