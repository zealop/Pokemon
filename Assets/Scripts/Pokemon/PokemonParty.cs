using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> party;

    public List<Pokemon> Party { get => party; }
    public int Count { get => party.Count; }
    private void Start()
    {
        foreach(var pokemon in party)
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
}
