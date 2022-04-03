using Move;

[System.Serializable]
public class MoveModifier
{
    protected MoveBase _base;

    public void LoadEffect(MoveBase move)
    {
        _base = move;
        ModifyMove();
    }

    public virtual void ModifyMove()
    {
    }
}