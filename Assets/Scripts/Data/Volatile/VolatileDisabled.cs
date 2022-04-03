using Data;

public class VolatileDisabled : VolatileCondition
{
    public override VolatileID ID => VolatileID.Disabled;

    private Move.Move disabled;

    public override void OnStart()
    {
        // unit.LastUsedMove.IsDisabled = true;
    }

    public override void OnEnd()
    {
        // unit.LastUsedMove.IsDisabled = false;
    }
}