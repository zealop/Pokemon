using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Battle;
using Data;
using Data.Volatile;
using JetBrains.Annotations;
using Move;
using Move.Behaviour;
using Move.Component;
using Move.Effect;
using UnityEditor;
using UnityEngine;
using Flinch = Move.Effect.Flinch;

namespace Editor
{
    public class GenerateMove : MonoBehaviour
    {
        private static readonly Type Type = typeof(MoveBase);

        private static readonly MoveDamage Damage = new Move.Damage.Default();
        private static readonly MoveAccuracy Accuracy = new Move.Accuracy.Default();
        private static readonly MoveBehavior Behavior = new Move.Behaviour.Default();

        private const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance;

        private static readonly FieldInfo CritField = Type.GetField("critStage", Flags);
        private static readonly FieldInfo PriorityField = Type.GetField("priority", Flags);

        private static readonly FieldInfo DamageField = Type.GetField("damage", Flags);
        private static readonly FieldInfo EffectField = Type.GetField("effect", Flags);
        private static readonly FieldInfo SecondaryField = Type.GetField("secondaryEffect", Flags);
        private static readonly FieldInfo AccField = Type.GetField("accuracyCheck", Flags);
        private static readonly FieldInfo BehaviorField = Type.GetField("behavior", Flags);


        // [MenuItem("Assets/Generate Moves")]
        private static void ModifyMoves()
        {
            Debug.Log(DamageField);

            var moves = Resources.LoadAll<MoveBase>("Moves");
            foreach (var move in moves)
            {
                DamageField?.SetValue(move, Damage);
                AccField?.SetValue(move, Accuracy);
                BehaviorField?.SetValue(move, Behavior);

                EditorUtility.SetDirty(move);
            }
        }

        private static MoveBase move;

        [MenuItem("Assets/Generate Moves")]
        private static void ModifyMoves2()
        {
            Generate1_10();
            Generate11_20();
            Generate21_30();
            Generate31_40();
            Generate41_50();
        }

        private static void Generate1_10()
        {
            move = Resources.Load<MoveBase>("Moves/2.Karate Chop");
            CritField.SetValue(move, 1);

            move = Resources.Load<MoveBase>("Moves/3.Double Slap");
            BehaviorField.SetValue(move, new Fury());

            move = Resources.Load<MoveBase>("Moves/4.Comet Punch");
            BehaviorField.SetValue(move, new Fury());

            move = Resources.Load<MoveBase>("Moves/6.Pay Day");
            EffectField.SetValue(move, new Payday());

            move = Resources.Load<MoveBase>("Moves/7.Fire Punch");
            SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Status(StatusID.BRN)));

            move = Resources.Load<MoveBase>("Moves/8.Ice Punch");
            SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Status(StatusID.FRZ)));

            move = Resources.Load<MoveBase>("Moves/9.Thunder Punch");
            SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Status(StatusID.PRZ)));
        }

        private static void Generate11_20()
        {
            move = Resources.Load<MoveBase>("Moves/12.Guillotine");
            DamageField.SetValue(move, new Move.Damage.OHKO());
            AccField.SetValue(move, new Move.Accuracy.OHKO());

            move = Resources.Load<MoveBase>("Moves/13.Razor Wind");
            CritField.SetValue(move, 1);
            BehaviorField.SetValue(move, new TwoTurn("{0} whipped up a whirlwind!", new TwoTurnMove()));

            move = Resources.Load<MoveBase>("Moves/14.Swords Dance");
            EffectField.SetValue(move, new Boost(BoostableStat.Attack, 2));

            move = Resources.Load<MoveBase>("Moves/18.Whirlwind");
            PriorityField.SetValue(move, -6);
            EffectField.SetValue(move, new Roar("{1} was blown away!"));

            move = Resources.Load<MoveBase>("Moves/19.Fly");
            BehaviorField.SetValue(move, new TwoTurn("{0} flew up high!", new Fly()));

            move = Resources.Load<MoveBase>("Moves/20.Bind");
            EffectField.SetValue(move, new Bind("{1} was squeezed by {0}!"));
        }

        private static void Generate21_30()
        {
            move = Resources.Load<MoveBase>("Moves/23.Stomp");
            SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Flinch()));

            move = Resources.Load<MoveBase>("Moves/24.Double Kick");
            BehaviorField.SetValue(move, new Fury());

            move = Resources.Load<MoveBase>("Moves/26.Jump Kick");
            BehaviorField.SetValue(move, new JumpKick());

            move = Resources.Load<MoveBase>("Moves/27.Rolling Kick");
            SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Flinch()));

            move = Resources.Load<MoveBase>("Moves/28.Sand Attack");
            EffectField.SetValue(move, new Boost(BoostableStat.Accuracy, -1));

            move = Resources.Load<MoveBase>("Moves/29.Headbutt");
            SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Flinch()));
        }

        private static void Generate31_40()
        {
            move = Resources.Load<MoveBase>("Moves/31.Fury Attack");
            BehaviorField.SetValue(move, new Fury());

            move = Resources.Load<MoveBase>("Moves/32.Horn Drill");
            DamageField.SetValue(move, new Move.Damage.OHKO());
            AccField.SetValue(move, new Move.Accuracy.OHKO());

            move = Resources.Load<MoveBase>("Moves/34.Body Slam");
            SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Status(StatusID.PRZ)));

            move = Resources.Load<MoveBase>("Moves/35.Wrap");
            EffectField.SetValue(move, new Bind("{1} was wrapped by {0}!"));
            
            move = Resources.Load<MoveBase>("Moves/36.Take Down");
            BehaviorField.SetValue(move, new Recoil(0.25f));
            
            move = Resources.Load<MoveBase>("Moves/38.Double-Edge");
            BehaviorField.SetValue(move, new Recoil(0.25f));
            
            move = Resources.Load<MoveBase>("Moves/39.Tail Whip");
            EffectField.SetValue(move, new Boost(BoostableStat.Defense, -1));

            move = Resources.Load<MoveBase>("Moves/40.Poison Sting");
            SecondaryField.SetValue(move, new SecondaryEffect(0.2f, new Status(StatusID.PSN)));
        }

        private static void Generate41_50()
        {
        }

        // [MenuItem("Assets/Generate Moves")]
        [UsedImplicitly]
        private static void GenerateMoves()
        {
            var moveTexts = ReadCsv();

            foreach (string[] moveText in moveTexts)
            {
                GenerateMoveFromText(moveText);
            }
        }

        private static void GenerateMoveFromText(IReadOnlyList<string> fields)
        {
            var example = ScriptableObject.CreateInstance<MoveBase>();
            string path = $"Assets/Game/Resources/Moves/{fields[0]}.{fields[1]}.asset";

            example._name = fields[1];
            Enum.TryParse(fields[2], out example.type);
            Enum.TryParse(fields[3], out example.category);

            int.TryParse(fields[5], out example.pp);
            int.TryParse(fields[6], out example.power);
            int.TryParse(fields[7], out example.accuracy);

            // example.MoveDamage = new DefaultDamage();
            // example.MoveAccuracy = new DefaultAccuracy();

            AssetDatabase.CreateAsset(example, path);
        }

        private static List<string[]> ReadCsv()
        {
            const string path = "Assets/Game/Resources/Moves/gen1.csv";
            var reader = new StreamReader(path);

            var list = new List<string[]>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    string[] fields = line.Split(',');
                    list.Add(fields);
                }
            }

            return list;
        }
    }
}