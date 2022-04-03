using Data;

public class VolatileRecharge : VolatileCondition
{
    public override VolatileID ID => VolatileID.Recharge;

    private int counter;

    public override void OnStart()
    {
        unit.LockedAction = true;
        counter = 2;
        unit.Modifier.OnTurnEndList.Add(Recharge);
    }

    public override void OnEnd()
    {
        unit.LockedAction = false;
        unit.Modifier.OnTurnEndList.Remove(Recharge);
    }

    private void Recharge()
    {
        counter--;
        if (counter == 0)
        {
            unit.RemoveVolatileCondition(ID);
        }
        else
        {
            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name} must recharge!"));
        }
    }
}