using System;
using Battle;
using Data;
using Data.Volatile;
using UnityEngine;

namespace Move
{
    public class MoveSlot
    {
        public MoveBase Base { get; }
        public int Pp { get; set; }
        public int MaxPp { get; }

        public bool IsDisabled
        {
            get
            {
                unit.Volatiles.TryGetValue(VolatileID.Disabled, out var condition);
                var temp = (Disabled) condition;
                return temp?.move == Base;
            }
        }

        public string Name => Base.Name;

        private Unit unit;

        public void Bind(Unit unit)
        {
            this.unit = unit;
        }

        public MoveSlot(MoveBase pBase)
        {
            Base = pBase;
            Pp = Base.Pp;
            MaxPp = Base.Pp;
        }

        public MoveSlot(MoveBase pBase, int pp)
        {
            Base = pBase;
            Pp = pp;
            MaxPp = pp;
        }

        public MoveSlot(MoveSaveData data)
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

        public MoveBuilder Build()
        {
            return Base.Builder().Move(this);
        }
    }

    public class MoveSaveData
    {
        public string name;
        public int pp;
    }
}