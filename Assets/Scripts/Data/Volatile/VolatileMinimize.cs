using System.Linq;
using Battle;
using Data;
using Move;

public class VolatileMinimize : VolatileCondition
{
    public override VolatileID ID => VolatileID.Minimize;

    private static readonly string[] big =
    {
        "Body Slam", "Stomp", "Dragon Rush", "Steamroller", "Heat Crash", "Heavy Slam", "Flying Press",
        "Malicious Moonsault"
    };

    public override void OnStart()
    {
        unit.Modifier.DefenderModList.Add(Small);
        unit.Modifier.AccuracyMod.Add(Tiny);
    }

    public override void OnEnd()
    {
        unit.Modifier.DefenderModList.Remove(Small);
        unit.Modifier.AccuracyMod.Remove(Tiny);
    }

    private static float Small(MoveBase move, Unit source)
    {
        return big.Contains(move.Name) ? 2 : 1;
    }

    private static bool Tiny(MoveBase move, Unit source)
    {
        return big.Contains(move.Name);
    }
}