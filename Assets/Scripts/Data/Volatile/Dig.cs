using System.Linq;
using Battle;
using Move;

namespace Data.Volatile
{
    public class Dig : TwoTurnMove
    {
        private static readonly string[] Doubled = {"Magnitude", "Earthquake", "Fissure"};

        public override void OnStart()
        {
            base.OnStart();
            
            unit.Modifier.SemiInvulnerable += SemiInvulnerable;
            unit.Modifier.DefenderModList.Add(Underground);
        }

        public override void OnEnd()
        {
            base.OnEnd();

            unit.Modifier.SemiInvulnerable -= SemiInvulnerable;
            unit.Modifier.DefenderModList.Remove(Underground);
        }

        private static bool SemiInvulnerable(MoveBuilder move)
        {
            return !Doubled.Contains(move.name);
        }

        private static float Underground(MoveBuilder move, Unit source)
        {
            return Doubled.Contains(move.name) ? 2f : 1f;
        }
    }
}