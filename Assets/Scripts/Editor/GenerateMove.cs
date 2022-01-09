using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GenerateMove : MonoBehaviour
{
    private static void ModifyMoves()
    {
        var moves = Resources.LoadAll<MoveBase>("Moves");
        foreach(var move in moves)
        {
            move.MoveDamage = new DefaultDamage();
            EditorUtility.SetDirty(move);
        }
        
    }

    // [MenuItem("Assets/Generate Moves")]
    private static void GenerateMoves()
    {
        var moveTexts = readCSV();

        foreach (string[] moveText in moveTexts)
        {
            GenerateMoveFromText(moveText);
        }
    }

    private static void GenerateMoveFromText(IReadOnlyList<string> fields)
    {
        var example = ScriptableObject.CreateInstance<MoveBase>();
        string path = $"Assets/Game/Resources/Moves/{fields[0]}.{fields[1]}.asset";

        example._name = fields[1];
        Enum.TryParse(fields[2], out example.type);
        Enum.TryParse(fields[3], out example.category);

        int.TryParse(fields[5], out example.pp);
        int.TryParse(fields[6], out example.power);
        int.TryParse(fields[7], out example.accuracy);

        example.MoveDamage = new DefaultDamage();
        example.MoveAccuracy = new DefaultAccuracy();

        AssetDatabase.CreateAsset(example, path);
    }

    private static List<string[]> readCSV()
    {
        const string path = "Assets/Game/Resources/Moves/gen1.csv";
        var reader = new StreamReader(path);

        var list = new List<string[]>();
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] fields = line.Split(',');
            list.Add(fields);
        }

        return list;
    }
}
