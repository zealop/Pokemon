using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Pokemons
{
    public class PokemonParty
    {
        private const int MaxPartySize = 6;
        private List<Pokemon> Pokemons { get; }

        public PokemonParty(params Pokemon[] pokemons)
        {
            Pokemons = pokemons.ToList();
        }
        
        public IEnumerable<Pokemon> GetHealthyPokemon(int amount)
        {
            var a = Pokemons.Where(p => !p.IsFainted);
            return Pokemons.Where(p => !p.IsFainted);
        }

        public void AddPokemon(Pokemon newPokemon)
        {
            if (Pokemons.Count < MaxPartySize)
            {
                Pokemons.Add(newPokemon);
            }
            else
            {
                TransferToPc(newPokemon);
            }
        }

        private void TransferToPc(Pokemon newPokemon)
        {
            //TODO: transfer to PC
            throw new NotImplementedException();
        }
    }
}