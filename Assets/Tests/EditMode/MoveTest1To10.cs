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
    public class MoveTest1To10 : MoveTestBase
    {
        [Test]
        public void TestPound()
        {
            LoadAndUseMove(1);

            Assert.Greater(Foe.MaxHp, Foe.Hp);
        }

        [Test]
        public void TestKarateChop()
        {
            LoadAndUseMove(2);

            Assert.Greater(Foe.MaxHp, Foe.Hp);
        }

        [Test]
        public void TestDoubleSlap()
        {
            LoadAndUseMove(3);

            Assert.Greater(Foe.MaxHp, Foe.Hp);
            Assert.That(PlayerUnit.AttacksThisTurn, Is.InRange(2, 5));
        }

        [Test]
        public void TestCometPunch()
        {
            LoadAndUseMove(4);

            Assert.Greater(Foe.MaxHp, Foe.Hp);
            Assert.That(PlayerUnit.AttacksThisTurn, Is.InRange(2, 5));
        }

        [Test]
        public void TestMegaPunch()
        {
            LoadAndUseMove(5);

            Assert.Greater(Foe.MaxHp, Foe.Hp);
        }

        [Test]
        public void TestPayday()
        {
            LoadAndUseMove(6);

            Assert.Greater(Foe.MaxHp, Foe.Hp);
            Assert.AreEqual((PlayerUnit.Side.Conditions[SideConditionID.Payday] as PaydayCondition)?.Count
                , Ally.Level * 5);

            LoadAndUseMove(6);
            Assert.AreEqual((PlayerUnit.Side.Conditions[SideConditionID.Payday] as PaydayCondition)?.Count
                , Ally.Level * 10);
        }

        [Test]
        public void TestFirePunch()
        {
            var move = Load(7);
            move.SecondaryEffect = new SecondaryEffect(1, move.SecondaryEffect.Effect);
            UseMove(move);
            
            Assert.Greater(Foe.MaxHp, Foe.Hp);
            Assert.IsInstanceOf<BurnCondition>(Foe.StatusCondition);
        }
        
        [Test]
        public void TestIcePunch()
        {
            var move = Load(8);
            move.SecondaryEffect = new SecondaryEffect(1, move.SecondaryEffect.Effect);
            UseMove(move);
            
            Assert.Greater(Foe.MaxHp, Foe.Hp);
            Assert.IsInstanceOf<FreezeCondition>(Foe.StatusCondition);
        }
        
        [Test]
        public void TestThunderPunch()
        {
            var move = Load(9);
            move.SecondaryEffect = new SecondaryEffect(1, move.SecondaryEffect.Effect);
            UseMove(move);
            
            Assert.Greater(Foe.MaxHp, Foe.Hp);
            Assert.IsInstanceOf<ParalyzeCondition>(Foe.StatusCondition);
        }
        
        [Test]
        public void TestScratch()
        {
            LoadAndUseMove(10);
            
            Assert.Greater(Foe.MaxHp, Foe.Hp);
            Debug.Log($"Max: {Foe.MaxHp}, {Foe.Hp}");
        }
    }
}