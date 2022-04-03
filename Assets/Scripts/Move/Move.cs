using Battle;
using UnityEngine;

namespace Move
{
    public class Move
    {
        public MoveBase Base { get; }
        public int Pp { get; private set; }
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

        public void Execute(BattleUnit source, BattleUnit target)
        {
            // Base.Clone(); //TODO clone the  move to modify
            Base.Execute(source, target, () => Pp--);
        }
    }

    public class MoveSaveData
    {
        public string name;
        public int pp;
    }
}