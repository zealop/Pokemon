using System.Collections;

[System.Serializable]
public class MoveEffect
{
    protected MoveBase _base;

    public void LoadEffect(MoveBase move)
    {
        _base = move;
    }
    public virtual IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return null;
    }

}
