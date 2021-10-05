using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongGrass : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] List<Pokemon> wildPokemons;

    
    public void OnPlayerTriggered(PlayerController player)
    {
        if (Random.Range(1, 101) <= 10)
        {
            //GameController.Instance.StartWildBattle(GetRandomPokemon());
        }
    }

    private Pokemon GetRandomPokemon()
    {
        var wildPokemon = wildPokemons[Random.Range(0, wildPokemons.Count)];

        return new Pokemon(wildPokemon.Base, wildPokemon.Level);
    }
}
