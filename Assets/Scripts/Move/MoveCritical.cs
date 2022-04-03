using Battle;
using UnityEngine;
public class MoveCritical
{
    private static readonly float[] CritTable = { 1 / 24f, 1 / 8f, 1 / 2f, 1f };
    public static float CritChance(int stage, BattleUnit source, BattleUnit target)
    {
        stage = Mathf.Clamp(stage + source.CritStage, 0, 3);

        return CritTable[stage];
    }
}