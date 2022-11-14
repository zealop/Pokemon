using System;
using Game.Battles;
using Random = UnityEngine.Random;

namespace Game.Moves.Accuracy
{
    public class Default : IMoveAccuracy
    {
        /// <summary>
        /// Step 1: Check for Sure Shot status (No-Guard, Toxic,...)
        /// Step 2: Check for semi-invulnerability on defender (Fly, Dig,...)
        /// Step 3: Check bypass accuracy check (accuracy 0 moves, minimized,..)
        /// Step 4: Normal accuracy % check
        /// </summary>
        /// <param name="move"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>true if the move hits</returns>
        public bool Apply(MoveBuilder move, Unit source, Unit target)
        {
            bool result;
            //TODO implement sure shot
            if (target.Modifier.IsSemiInvulnerable(move))
            {
                result = false;
            }
            else
            {
                result = IsBypassAccuracyCheck(move, target) || IsPassHitChanceCheck(move, source, target);
            }

            return result;
        }

        private bool IsBypassAccuracyCheck(MoveBuilder move, Unit target)
        {
            return move.Accuracy == 0 || target.Modifier.IsVulnerable(move);
        }

        private bool IsPassHitChanceCheck(MoveBuilder move, Unit source, Unit target)
        {
            return Random.value <= move.Accuracy * source.StatStage.AccuracyModifier(target.StatStage.Evasion);
        }
        
    }
}