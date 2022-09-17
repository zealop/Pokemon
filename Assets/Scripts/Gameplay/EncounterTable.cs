using System.Collections.Generic;
using Pokemons;
using UnityEngine;

public class EncounterTable : MonoBehaviour
{
    [SerializeField] private List<Pokemon> wildPokemons;

    public Pokemon GetRandomPokemon()
    {
        var wildPokemon = wildPokemons[Random.Range(0, wildPokemons.Count)];

        return new Pokemon(wildPokemon.Base, wildPokemon.Level);
    }
}
