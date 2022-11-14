using Game.Battles;
using Game.Pokemons;
using UnityEngine;

namespace Tests.EditMode
{
    [CreateAssetMenu(menuName = "Test data")]
    public class TestData : ScriptableObject
    {
        [SerializeField] private BattleContext battleContext;
        [SerializeField] private Pokemon ally;
        [SerializeField] private Pokemon foe;

        public BattleContext BattleContext => battleContext;
        public Pokemon Ally => ally;
        public Pokemon Foe => foe;
    }
}