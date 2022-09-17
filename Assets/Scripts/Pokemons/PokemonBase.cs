using System;
using System.Collections.Generic;
using Move;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Pokemons
{
    // [CreateAssetMenu(fileName = "Pokemon", menuName = "New Pokemon ")]
    public class PokemonBase : ScriptableObject
    {
        [FormerlySerializedAs("_name")] [SerializeField] private string specieName;

        [SerializeField] private PokemonSprite sprite;

        [SerializeField] private int catchRate;

        [BoxGroup("Types")]
        [SerializeField]
        private PokemonType type1;
        [BoxGroup("Types")]
        [SerializeField]
        private PokemonType type2;

        [BoxGroup("Size")]
        [SerializeField]
        private int height;
        [BoxGroup("Size")]
        [SerializeField]
        private int weight;

        [BoxGroup("EXP")]
        [SerializeField]
        private int expYield;
        [BoxGroup("EXP")]
        [SerializeField]
        private GrowthType growthRate;


        [BoxGroup("Base Stat")]
        [SerializeField]
        private int hp;
        [BoxGroup("Base Stat")]
        [SerializeField]
        private int attack;
        [BoxGroup("Base Stat")]
        [SerializeField]
        private int defense;
        [BoxGroup("Base Stat")]
        [SerializeField]
        private int spAttack;
        [BoxGroup("Base Stat")]
        [SerializeField]
        private int spDefense;
        [BoxGroup("Base Stat")]
        [SerializeField]
        private int speed;

        [SerializeField] private List<LearnableMove> learnableMoves;

        public string SpecieName => specieName;
        public PokemonSprite Sprite => sprite;

        public int CatchRate => catchRate;
        public PokemonType Type1 => type1;
        public PokemonType Type2 => type2;
        public int Height => height;
        public int Weight => weight;
        public int ExpYield => expYield;
        public GrowthType GrowthRate => growthRate;
        public int Hp => hp;
        public int Attack => attack;
        public int Defense => defense;
        public int SpAttack => spAttack;
        public int SpDefense => spDefense;
        public int Speed => speed;

        public List<LearnableMove> LearnableMoves => learnableMoves;
    }

    [Serializable]
    public class LearnableMove
    {
        [SerializeField] private MoveBase moveBase;
        [SerializeField] private int level;

        public MoveBase Base => moveBase;
        public int Level => level;
    }

    [Serializable]
    public class PokemonSprite
    {
        [PreviewField(100)]
        [SerializeField]
        private Sprite front;
        [PreviewField(100)]
        [SerializeField]
        private Sprite back;
        [PreviewField(100)]
        [SerializeField]
        private Sprite box;

        public Sprite Front => front;
        public Sprite Back => back;
        public Sprite Box => box;
    }

    [CustomEditor(typeof(PokemonBase))]
    public class ExampleEditor : OdinEditor
    {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            PokemonBase example = (PokemonBase)target;

            if (example == null || example.Sprite.Front == null)
                return null;

            // example.PreviewIcon must be a supported format: ARGB32, RGBA32, RGB24,
            // Alpha8 or one of float formats
            Texture2D tex = new Texture2D(width, height);
            EditorUtility.CopySerialized(TextureFromSprite(example.Sprite.Front), tex);

            return tex;
        }

        private static Texture2D TextureFromSprite(Sprite sprite)
        {
            if (Math.Abs(sprite.rect.width - sprite.texture.width) < Tolerance) return sprite.texture;
            var newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            var newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;

        }

        private const float Tolerance = 0.000001f;
    }
}