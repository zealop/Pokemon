using Game.Battles;
using Game.Condition;
using Game.Condition.Side;
using Game.Condition.Status;
using Game.Moves;
using Game.Pokemons;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.EditMode
{
    public class MoveTestBase
    {
        protected TestData TestData;
        protected BattleContext Context;
        protected Pokemon Ally => TestData.Ally;
        protected Pokemon Foe => TestData.Foe;
        protected Battle Battle;
        protected Unit PlayerUnit => Battle.PlayerSide.Units[0];
        protected Unit EnemyUnit => Battle.EnemySide.Units[0];

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestData = AssetDatabase.LoadAssetAtPath<TestData>("Assets/Tests/Data/Test.asset");
            Context = TestData.BattleContext;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Resources.UnloadAsset(TestData);
        }

        [SetUp]
        public void Setup()
        {
            Ally.Init();
            Foe.Init();

            Battle = new WildBattle(Context
                , new PokemonParty(Ally)
                , Foe);
        }

        protected MoveBuilder Load(int id)
        {
            var moveBase = AssetDatabase.LoadAssetAtPath<MoveBase>($"Assets/Game/Addressables/Moves/{id}.asset");
            return new MoveBuilder(moveBase, null);
        }

        protected void UseMove(MoveBuilder move)
        {
            Battle.Enqueue(move, PlayerUnit, EnemyUnit);
            Battle.RunTurn();
        }

        protected void LoadAndUseMove(int id)
        {
            UseMove(Load(id));
        }
    }
}