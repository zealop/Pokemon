using System;
using Battle;
using UnityEngine;

namespace Move
{
    public class Move
    {
        public MoveBase Base { get; }
        public int Pp { get; set; }
        public int MaxPp { get; }
        public bool IsDisabled { get; set; }
        public string Name => Base.Name;

        public Move(MoveBase pBase)
        {
            Base = pBase;
            Pp = Base.Pp;
            MaxPp = Base.Pp;
        }

        public Move(MoveBase pBase, int pp)
        {
            Base = pBase;
            Pp = pp;
            MaxPp = pp;
        }

        public Move(MoveSaveData data)
        {
            Base = Resources.Load<MoveBase>($"Moves/{data.name}");
            Pp = data.pp;
            MaxPp = Base.Pp;
        }

        public MoveSaveData GetSaveData()
        {
            var data = new MoveSaveData
            {
                name = Base.name,
                pp = Pp
            };
            return data;
        }

        public void Execute(Unit source, Unit target)
        {
            // Base.Clone(); //TODO clone the  move to modify
            var moveBuilder = Base.Builder().Move(this);
            
            Base.Execute(source, target, () => consumePp());
        }

        public void consumePp(int value = 1)
        {
            Pp = Math.Min(0, Pp - value);
        }
    }

    public class MoveSaveData
    {
        public string name;
        public int pp;
    }
}