using System.Collections.Generic;
using Game.Moves;
using Game.Pokemons;

namespace Game.Battles
{
    public class Unit
    {
        private IBattle Battle { get; }
        private Pokemon Pokemon { get; }
        public string Name => Pokemon.Name;
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAttack { get; set; }
        public int SpDefense { get; set; }
        public int Speed { get; set; }
        public int Hp { get; set; }
        public float MaxHp { get; set; }
        public List<MoveSlot> Moves { get; set; } 

        public Unit(IBattle battle, Pokemon pokemon)
        {
            Battle = battle;
            Pokemon = pokemon;
            Moves = pokemon.Moves;
        }
    }
}