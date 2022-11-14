using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Constants;
using UnityEditor;

namespace Editor
{
    public static class CreateTypeChart
    {
        private static readonly float[][] Array =
        {
            new[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1, 1f, 1f, 1f, 1f },
            new[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 0, 1f, 1f, 0.5f, 1f },
            new[] { 1f, 1f, 0.5f, 0.5f, 1f, 2f, 2f, 1f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 0.5f, 1f, 2f, 1f },
            new[] { 1f, 1f, 2f, 0.5f, 1f, 0.5f, 1f, 1f, 1f, 2f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f, 1f, 1f },
            new[] { 1f, 1f, 1f, 2f, 0.5f, 0.5f, 1f, 1f, 1f, 0f, 2f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f },
            new[] { 1f, 1f, 0.5f, 2f, 1f, 0.5f, 1f, 1f, 0.5f, 2f, 0.5f, 1f, 0.5f, 2f, 1f, 0.5f, 1f, 0.5f, 1f },
            new[] { 1f, 1f, 0.5f, 0.5f, 1f, 2f, 0.5f, 1f, 1f, 2f, 2f, 1f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f },
            new[] { 1f, 2f, 1f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f, 0.5f, 0.5f, 0.5f, 2f, 0f, 1f, 2f, 2f, 0.5f },
            new[] { 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, 0f, 2f },
            new[] { 1f, 1f, 2f, 1f, 2f, 0.5f, 1f, 1f, 2f, 1f, 0f, 1f, 0.5f, 2f, 1f, 1f, 1f, 2f, 1f },
            new[] { 1f, 1f, 1f, 1f, 0.5f, 2f, 1f, 2f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f, 0.5f, 1f },
            new[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 2f, 1f, 1f, 0.5f, 1f, 1f, 1f, 1f, 0f, 0.5f, 1f },
            new[] { 1f, 1f, 0.5f, 1f, 1f, 2f, 1f, 0.5f, 0.5f, 1f, 0.5f, 2f, 1f, 1f, 0.5f, 1f, 2f, 0.5f, 0.5f },
            new[] { 1f, 1f, 2f, 1f, 1f, 1f, 2f, 0.5f, 1f, 0.5f, 2f, 1f, 2f, 1f, 1f, 1f, 1f, 0.5f, 1f },
            new[] { 1f, 0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 2f, 1f, 0.5f, 1f, 1f },
            new[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 0.5f, 0f },
            new[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f, 2f, 1f, 1f, 2f, 1f, 0.5f, 1f, 0.5f },
            new[] { 1f, 1f, 0.5f, 0.5f, 0.5f, 1f, 2f, 1f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f, 0.5f, 2f },
            new[] { 1f, 1f, 0.5f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 2f, 0.5f, 1f },
        };

        private static Dictionary<PokemonType, Dictionary<PokemonType, float>> Dict =>
            Array.Select((s, i) => new { s, i })
                .ToDictionary(g => (PokemonType)g.i, g => g.s.Select((s, i) => new { s, i })
                    .ToDictionary(h => (PokemonType)h.i, h => h.s));

        [MenuItem("TypeChart/Save")]
        public static void MakeChart()
        {
            var typeChart = AssetDatabase.LoadAssetAtPath<TypeChart>("Assets/Game/Resources/Other/TypeChart.asset");

            var chart = typeof(TypeChart).GetField("_chart", BindingFlags.NonPublic | 
                                                             BindingFlags.Instance);
            chart?.SetValue(typeChart, Dict);

            EditorUtility.SetDirty(typeChart);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}