using System;
using UnityEngine;

namespace Game.Moves
{
    [Serializable]
    public class MoveSlot
    {
        [SerializeField] private MoveBase @base;
        [SerializeField] private int pp;
        [SerializeField] private int maxPp;
        public MoveBase Base => @base;
        private int Pp => pp;
        private int MaxPp => maxPp;
        public string Name => Base.Name;

        public MoveSlot(MoveBase moveBase)
        {
            @base = moveBase;
            pp = Base.Pp;
            maxPp = Base.Pp;
        }
    }
}