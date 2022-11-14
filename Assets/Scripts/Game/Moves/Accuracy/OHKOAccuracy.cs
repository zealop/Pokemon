using Game.Battles;
using UnityEngine;

namespace Game.Moves.Accuracy
{
    public class OHKOAccuracy : IMoveAccuracy
    {
        public bool Apply(MoveBuilder move, Unit source, Unit target)
        {
            //TODO implement sure shot
            if (target.Modifier.IsSemiInvulnerable(move))
            {
                return false;
            }
            
            var levelDiff = source.Level - target.Level;

            var a = (move.Accuracy + levelDiff);
            var b = a / 100f;
            var c = Random.value;
            return c <= b  && levelDiff >= 0 ;
        }
    }
}