using Battle;
using Data;
using Move;

public class VolatileCounter : VolatileCondition
{
    public override VolatileID ID => VolatileID.Counter;

    public int StoredDamage;

    public override void OnStart()
    {
        //Unit.OnHitList.Add(StoreDamage);
        //Unit.OnTurnEndList.Add(Clear);

        ;
    }

    public override void OnEnd()
    {
        //Unit.OnHitList.Remove(StoreDamage);
        //Unit.OnTurnEndList.Remove(Clear);
    }

    private void StoreDamage(MoveBase move, Unit source, int damage)
    {
        if (move.Category == MoveCategory.Physical)
            StoredDamage += damage;
    }

    private void Clear()
    {
        unit.RemoveVolatileCondition(ID);
    }
}