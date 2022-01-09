using UnityEngine;

public class Move
{
    public MoveBase Base { get; }
    public int PP { get; set; }
    public int MaxPP { get; private set; }
    public bool IsDisabled { get; set; }
    public string Name => Base.Name;

    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = Base.PP;
        MaxPP = Base.PP;
    }
    public Move(MoveSaveData data)
    {
        Base = Resources.Load<MoveBase>($"Moves/{data.name}");
        PP = data.pp;
        MaxPP = Base.PP;
    }
    public MoveSaveData GetSaveData()
    {
        var data = new MoveSaveData
        {
            name = Base.name,
            pp = PP
        };
        return data;
    }
}

public class MoveSaveData
{
    public string name;
    public int pp;
}
