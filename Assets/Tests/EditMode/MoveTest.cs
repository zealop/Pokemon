using Game.Battles;
using Game.Pokemons;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.EditMode
{
    public class TestMove
    {
        private PokemonBase test1;
        private PokemonBase test2;
        private Pokemon ally;
        private Pokemon foe;
        private PokemonParty party;
        private IBattle battle;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            test1 = 
                (PokemonBase) AssetDatabase.LoadAssetAtPath("Assets/Tests/Pokemon/Test1.asset", typeof(PokemonBase));
            test2 =
                (PokemonBase) AssetDatabase.LoadAssetAtPath("Assets/Tests/Pokemon/Test2.asset", typeof(PokemonBase));
            ally = new Pokemon(test1, 5);
            foe = new Pokemon(test2, 5);
            party = new PokemonParty(ally);
            battle = new WildBattle(party, foe);
        }

        [Test]
        public void NewTestScriptSimplePasses()
        {
            
        }
    }
}