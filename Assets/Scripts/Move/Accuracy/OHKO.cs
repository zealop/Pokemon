using Battle;
using UnityEngine;

namespace Move.Accuracy
{
    public class OHKO : MoveAccuracy
    {
        public override bool Apply(BattleUnit source, BattleUnit target)
        {
            int diff = source.Level - target.Level;
            return Random.value <= diff / 100f && diff >= 0 ;
        }
    }
}