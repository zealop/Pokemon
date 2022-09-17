using System.Collections.Generic;
using Pokemons;
using UnityEngine;

public class LongGrass : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] private List<Pokemon> wildPokemons;


    public void OnPlayerTriggered(PlayerController player)
    {
        if (Random.Range(1, 101) <= 10)
        {
            GameController.I.StartWildBattle(GetRandomPokemon());
        }
    }

    private Pokemon GetRandomPokemon()
    {
        var wildPokemon = wildPokemons[Random.Range(0, wildPokemons.Count)];

        return new Pokemon(wildPokemon.Base, wildPokemon.Level);
    }
}
