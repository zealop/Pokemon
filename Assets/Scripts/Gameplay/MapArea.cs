using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemons;

    public Pokemon GetRandomPokemon()
    {
        var wildPokemon = wildPokemons[Random.Range(0, wildPokemons.Count)];

        return new Pokemon(wildPokemon.Base, wildPokemon.Level);
    }
}
