using UnityEngine;

public class Dice
{
    /// <summary>
    /// Roll a dice
    /// </summary>
    /// <param name="faces">Number of faces the dice has</param>
    /// <returns>Result of the dice the roll</returns>
    public static int Roll(int faces)
    {
        return Random.Range(1, faces + 1);
    }
}
