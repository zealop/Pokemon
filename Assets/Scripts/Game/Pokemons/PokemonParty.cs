using System;
using System.Collections.Generic;
using System.Linq;

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
        
        public Pokemon GetHealthyPokemon()
        {
            return Pokemons.FirstOrDefault(x => x.Hp > 0);
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