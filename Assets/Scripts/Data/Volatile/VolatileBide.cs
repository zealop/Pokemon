using Battle;
using Data;
using Move;

public class VolatileBide : VolatileCondition
{
    public override VolatileID ID => VolatileID.Bide;

    public int Counter;
    public int StoredDamage;

    public override void OnStart()
    {
        Counter = 2;
        unit.LockedAction = true;

        //Unit.OnHitList.Add(StoreDamage);
    }

    public override void OnEnd()
    {
        unit.LockedAction = false;

        //Unit.OnHitList.Remove(StoreDamage);
    }

    private void StoreDamage(MoveBase move, BattleUnit source, int damage)
    {
        StoredDamage += damage;
    }
}