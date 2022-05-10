using Battle;
using UnityEngine;

namespace Move.Accuracy
{
    public class Default : MoveAccuracy
    {
        /// <summary>
        /// Step 1: Check for Sure Shot status (No-Guard, Toxic,...)
        /// Step 2: Check for semi-invulnerability on defender (Fly, Dig,...)
        /// Step 3: Check bypass accuracy check (accuracy 0 moves, minimized,..)
        /// Step 4: Normal accuracy % check
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>true if the move hits</returns>
        public override bool Apply(Unit source, Unit target)
        {
            bool result;
            //TODO implement sure shot
            if (IsDefenderSemiInvulnerable(target))
            {
                result = false;
            }
            else
            {
                result = IsBypassAccuracyCheck(target) || HitChanceCheck(source, target);
            }

            return result;
        }

        private bool IsBypassAccuracyCheck(Unit target)
        {
            return move.Accuracy == 0 || target.Modifier.Vulnerable(move);
        }

        private bool IsDefenderSemiInvulnerable(Unit target)
        {
            return target.Modifier.SemiInvulnerable(move);
        }

        private bool HitChanceCheck(Unit source, Unit target)
        {
            return Random.value <= move.Accuracy * source.Accuracy * target.Evasion / 100f;
        }
    }
}