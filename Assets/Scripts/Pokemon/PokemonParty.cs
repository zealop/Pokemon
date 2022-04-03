using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    private const int MaxPartySize = 6;
    [SerializeField] private List<Pokemon> party;

    public List<Pokemon> Party => party;

    private void Start()
    {
        foreach (var pokemon in party)
        {
            pokemon.Init();
        }
    }

    public Pokemon GetHealthyPokemon()
    {
        return party.FirstOrDefault(x => x.HP > 0);
    }
    
    public void AddPokemon(Pokemon newPokemon)
    {
        if (party.Count < MaxPartySize)
        {
            party.Add(newPokemon);
        }
        else
        {
            //TODO: transfer to PC
        }
    }

    public void RestorePartyState(List<Pokemon> pokemons)
    {
        party = pokemons;
    }
}
