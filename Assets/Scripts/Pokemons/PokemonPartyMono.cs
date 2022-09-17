using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pokemons
{
    public class PokemonPartyMono : MonoBehaviour
    {
        private const int MaxPartySize = 6;
        [FormerlySerializedAs("party")] [SerializeField] private List<Pokemon> pokemons;
        public List<Pokemon> Pokemons => pokemons;
        
        public PokemonPartyMono(List<Pokemon> pokemons)
        {
            this.pokemons = pokemons;
        }
        private void Start()
        {
            foreach (var pokemon in pokemons)
            {
                pokemon.Init();
            }
        }

        public Pokemon GetHealthyPokemon()
        {
            return pokemons.FirstOrDefault(x => x.Hp > 0);
        }
    
        public void AddPokemon(Pokemon newPokemon)
        {
            if (pokemons.Count < MaxPartySize)
            {
                pokemons.Add(newPokemon);
            }
            else
            {
                //TODO: transfer to PC
            }
        }

        public void RestorePartyState(List<Pokemon> pokemons)
        {
            this.pokemons = pokemons;
        }
    }
}
