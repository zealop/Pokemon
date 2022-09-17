using System.Linq;
using Battle;
using Move;

namespace Data.Volatile
{
    public class Fly : TwoTurnMove
    {
        private static readonly string[] Normal = {"Hurricane", "Sky Uppercut", "Smack Down", "Thousand Arrows", "Thunder"};
        private static readonly string[] Doubled = {"Gust", "Twister"};

        public override void OnStart()
        {
            base.OnStart();

            unit.Modifier.SemiInvulnerable += SemiInvulnerable;
            unit.Modifier.DefenderModList.Add(WindyFlight);
        }

        public override void OnEnd()
        {
            base.OnStart();

            unit.Modifier.SemiInvulnerable -= SemiInvulnerable;
            unit.Modifier.DefenderModList.Remove(WindyFlight);
        }

        private static bool SemiInvulnerable(MoveBuilder move)
        {
            return !Normal.Concat(Doubled).Contains(move.name);
        }

        private static float WindyFlight(MoveBuilder move, Unit source)
        {
            return Doubled.Contains(move.name) ? 2f : 1f;
        }
    }
}