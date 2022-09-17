using System;
using UnityEngine;

namespace Game.Moves
{
    [Serializable]
    public class LearnableMove
    {
        [SerializeField] private MoveBase moveBase;
        [SerializeField] private int level;

        public MoveBase Base => moveBase;
        public int Level => level;
    }
}