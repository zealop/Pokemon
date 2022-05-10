using Battle;
using UnityEngine;

namespace Move.Accuracy
{
    public class OHKO : MoveAccuracy
    {
        public override bool Apply(Unit source, Unit target)
        {
            int diff = source.Level - target.Level;
            return Random.value <= diff / 100f && diff >= 0 ;
        }
    }
}