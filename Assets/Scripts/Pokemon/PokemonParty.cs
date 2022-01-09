using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] private List<Pokemon> party;

    public List<Pokemon> Party { get => party; set => party = value; }
    public int Count => party.Count;

    public void SetParty(List<Pokemon> party)
    {
        this.party = party;
    }
    private void Start()
    {
        foreach (var pokemon in party)
        {
            pokemon.Init();
        }
    }

    public Pokemon GetHealthyPokemon()
    {
        return party.Where(x => x.HP > 0).FirstOrDefault();
    }

    public Pokemon GetRandomPokemon(Pokemon current)
    {
        var candidates = party.Where(x => x.HP > 0 && x != current);
        return candidates.ElementAt(Random.Range(0, candidates.Count()));
    }

    public void AddPokemon(Pokemon newPokemon)
    {
        if (party.Count < 6)
        {
            party.Add(newPokemon);
        }
        else
        {
            //TODO: transfer to PC
        }
    }
}
