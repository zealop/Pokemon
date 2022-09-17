using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Pokemons
{
    [Serializable]
    public class PokemonSprite
    {
        [PreviewField(100)] [SerializeField] private Sprite front;
        [PreviewField(100)] [SerializeField] private Sprite back;
        [PreviewField(100)] [SerializeField] private Sprite box;

        public Sprite Front => front;
        public Sprite Back => back;
        public Sprite Box => box;
    }
}