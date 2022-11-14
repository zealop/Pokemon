using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Game.Condition;
using Game.Constants;
using Game.Moves;
using Game.Moves.Accuracy;
using Game.Moves.Behavior;
using Game.Moves.Damage;
using Game.Moves.Effect;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using Default = Game.Moves.Damage.Default;

namespace Editor
{
    public class GenerateMove : MonoBehaviour
    {
        private static readonly Type Type = typeof(MoveBase);

        private static readonly IMoveDamage DefaultDamage = new Default();
        private static readonly IMoveAccuracy DefaultAccuracy = new Game.Moves.Accuracy.Default();
        private static readonly IMoveBehavior DefaultBehavior = new Game.Moves.Behavior.Default();

        private const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance;

        private static readonly FieldInfo NameField = Type.GetField("baseName", Flags);
        private static readonly FieldInfo TypeField = Type.GetField("type", Flags);
        private static readonly FieldInfo CategoryField = Type.GetField("category", Flags);
        private static readonly FieldInfo PpField = Type.GetField("pp", Flags);
        private static readonly FieldInfo PowerField = Type.GetField("power", Flags);
        private static readonly FieldInfo AccuracyField = Type.GetField("accuracy", Flags);

        private static readonly FieldInfo CritField = Type.GetField("critStage", Flags);
        private static readonly FieldInfo PriorityField = Type.GetField("priority", Flags);

        private static readonly FieldInfo DamageField = Type.GetField("damage", Flags);
        private static readonly FieldInfo EffectField = Type.GetField("effect", Flags);
        private static readonly FieldInfo SecondaryField = Type.GetField("secondaryEffect", Flags);
        private static readonly FieldInfo AccField = Type.GetField("accuracyCheck", Flags);
        private static readonly FieldInfo BehaviorField = Type.GetField("behavior", Flags);


        private static MoveBase move;

        [MenuItem("Moves/Generate/Extra info")]
        private static void ModifyMoves2()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                Generate1_10();
                Generate11_20();
                // Generate21_30();
                // Generate31_40();
                // Generate41_50();
                // Generate51_60();
                // Generate61_70();
            }
            finally
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.StopAssetEditing();
            }
        }

        private static MoveBase Load(int id)
        {
            var instance = AssetDatabase.LoadAssetAtPath<MoveBase>($"Assets/Game/Addressables/Moves/{id}.asset");
            EditorUtility.SetDirty(instance);
            return instance;
        }

        private static void Generate1_10()
        {
            move = Load(1);
            CritField.SetValue(move, 1);

            move = Load(2);

            move = Load(3);
            BehaviorField.SetValue(move, new MultiHit());

            move = Load(4);
            BehaviorField.SetValue(move, new MultiHit());

            move = Load(6);
            EffectField.SetValue(move, new Payday());

            move = Load(7);
            SecondaryField.SetValue(move,
                new SecondaryEffect(0.1f, new Status(StatusConditionID.Burn)));

            move = Load(8);
            SecondaryField.SetValue(move,
                new SecondaryEffect(0.1f, new Status(StatusConditionID.Freeze)));

            move = Load(9);
            SecondaryField.SetValue(move,
                new SecondaryEffect(0.1f, new Status(StatusConditionID.Paralyze)));

            move = Load(10);
        }

        private static void Generate11_20()
        {
            move = Load(11);

            move = Load(12);
            DamageField.SetValue(move, new OHKODamage());
            AccField.SetValue(move, new OHKOAccuracy());

            // move = Resources.Load<MoveBase>("Moves/13.Razor Wind");
            // CritField.SetValue(move, 1);
            // BehaviorField.SetValue(move, new TwoTurn("{0} whipped up a whirlwind!", new TwoTurnMove()));
            //
            // move = Resources.Load<MoveBase>("Moves/14.Swords Dance");
            // EffectField.SetValue(move, new Boost(BoostableStat.Attack, 2));
            //
            // move = Resources.Load<MoveBase>("Moves/15.Cut");
            //
            // move = Resources.Load<MoveBase>("Moves/16.Gust");
            //
            // move = Resources.Load<MoveBase>("Moves/17.Wing Attack");
            //
            //
            // move = Resources.Load<MoveBase>("Moves/18.Whirlwind");
            // PriorityField.SetValue(move, -6);
            // EffectField.SetValue(move, new Roar("{1} was blown away!"));
            //
            // move = Resources.Load<MoveBase>("Moves/19.Fly");
            // BehaviorField.SetValue(move, new TwoTurn("{0} flew up high!", new Fly()));
            //
            // move = Resources.Load<MoveBase>("Moves/20.Bind");
            // EffectField.SetValue(move, new Bind("{1} was squeezed by {0}!"));
        }
        //
        // private static void Generate21_30()
        // {
        //     move = Resources.Load<MoveBase>("Moves/21.Slam");
        //
        //     move = Resources.Load<MoveBase>("Moves/22.Vine Whip");
        //
        //     move = Resources.Load<MoveBase>("Moves/23.Stomp");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Flinch()));
        //
        //     move = Resources.Load<MoveBase>("Moves/24.Double Kick");
        //     BehaviorField.SetValue(move, new FixedHit(2));
        //
        //     move = Resources.Load<MoveBase>("Moves/25.Mega Kick");
        //
        //     move = Resources.Load<MoveBase>("Moves/26.Jump Kick");
        //     BehaviorField.SetValue(move, new JumpKick());
        //
        //     move = Resources.Load<MoveBase>("Moves/27.Rolling Kick");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Flinch()));
        //
        //     move = Resources.Load<MoveBase>("Moves/28.Sand Attack");
        //     EffectField.SetValue(move, new Boost(BoostableStat.Accuracy, -1));
        //
        //     move = Resources.Load<MoveBase>("Moves/29.Headbutt");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Flinch()));
        //
        //     move = Resources.Load<MoveBase>("Moves/30.Horn Attack");
        // }
        //
        // private static void Generate31_40()
        // {
        //     move = Resources.Load<MoveBase>("Moves/31.Fury Attack");
        //     BehaviorField.SetValue(move, new MultiHit());
        //
        //     move = Resources.Load<MoveBase>("Moves/32.Horn Drill");
        //     DamageField.SetValue(move, new Move.Damage.OHKO());
        //     AccField.SetValue(move, new Move.Accuracy.OHKO());
        //
        //     move = Resources.Load<MoveBase>("Moves/33.Tackle");
        //
        //     move = Resources.Load<MoveBase>("Moves/34.Body Slam");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Status(StatusID.PRZ)));
        //
        //     move = Resources.Load<MoveBase>("Moves/35.Wrap");
        //     EffectField.SetValue(move, new Bind("{1} was wrapped by {0}!"));
        //
        //     move = Resources.Load<MoveBase>("Moves/36.Take Down");
        //     BehaviorField.SetValue(move, new Recoil(0.25f));
        //
        //     move = Resources.Load<MoveBase>("Moves/37.Thrash");
        //     EffectField.SetValue(move, new Frenzy());
        //
        //     move = Resources.Load<MoveBase>("Moves/38.Double-Edge");
        //     BehaviorField.SetValue(move, new Recoil(0.25f));
        //
        //     move = Resources.Load<MoveBase>("Moves/39.Tail Whip");
        //     EffectField.SetValue(move, new Boost(BoostableStat.Defense, -1));
        //
        //     move = Resources.Load<MoveBase>("Moves/40.Poison Sting");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.2f, new Status(StatusID.PSN)));
        // }
        //
        // private static void Generate41_50()
        // {
        //     move = Resources.Load<MoveBase>("Moves/41.Twineedle");
        //     BehaviorField.SetValue(move, new FixedHit(2));
        //
        //     move = Resources.Load<MoveBase>("Moves/42.Pin Missile");
        //     BehaviorField.SetValue(move, new MultiHit());
        //
        //     move = Resources.Load<MoveBase>("Moves/43.Leer");
        //     EffectField.SetValue(move, new Boost(BoostableStat.Defense, -1));
        //
        //     move = Resources.Load<MoveBase>("Moves/44.Bite");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.3f, new Flinch()));
        //
        //     move = Resources.Load<MoveBase>("Moves/45.Growl");
        //     EffectField.SetValue(move, new Boost(BoostableStat.Attack, -1));
        //
        //     move = Resources.Load<MoveBase>("Moves/46.Roar");
        //     PriorityField.SetValue(move, -6);
        //     EffectField.SetValue(move, new Roar("{1} ran away scared!"));
        //
        //     move = Resources.Load<MoveBase>("Moves/47.Sing");
        //     EffectField.SetValue(move, new Status(StatusID.SLP));
        //
        //     move = Resources.Load<MoveBase>("Moves/48.Supersonic");
        //     EffectField.SetValue(move, new Confuse());
        //
        //     move = Resources.Load<MoveBase>("Moves/49.Sonic Boom");
        //     DamageField.SetValue(move, new Flat(20));
        //
        //     move = Resources.Load<MoveBase>("Moves/50.Disable");
        //     EffectField.SetValue(move, new Disable());
        // }
        //
        // private static void Generate51_60()
        // {
        //     move = Resources.Load<MoveBase>("Moves/51.Acid");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Boost(BoostableStat.SpDefense, -1)));
        //     
        //     move = Resources.Load<MoveBase>("Moves/52.Ember");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Status(StatusID.BRN)));
        //     
        //     move = Resources.Load<MoveBase>("Moves/53.Flamethrower");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Status(StatusID.BRN)));
        //     
        //     move = Resources.Load<MoveBase>("Moves/54.Mist");
        //     EffectField.SetValue(move, new Misted());
        //     
        //     move = Resources.Load<MoveBase>("Moves/55.Water Gun");
        //     move = Resources.Load<MoveBase>("Moves/56.Hydro Pump");
        //     move = Resources.Load<MoveBase>("Moves/57.Surf");
        //     
        //     move = Resources.Load<MoveBase>("Moves/58.Ice Beam");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Status(StatusID.FRZ)));
        //     
        //     move = Resources.Load<MoveBase>("Moves/59.Blizzard");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Status(StatusID.BRN)));
        //     AccField.SetValue(move, new Blizzard());
        //     
        //     move = Resources.Load<MoveBase>("Moves/60.Psybeam");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Confuse()));
        // }
        //
        // private static void Generate61_70()
        // {
        //     move = Resources.Load<MoveBase>("Moves/61.Bubble Beam");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Boost(BoostableStat.Speed, -1)));
        //     
        //     move = Resources.Load<MoveBase>("Moves/62.Aurora Beam");
        //     SecondaryField.SetValue(move, new SecondaryEffect(0.1f, new Boost(BoostableStat.Attack, -1)));
        //     
        //     move = Resources.Load<MoveBase>("Moves/63.Hyper Beam");
        //     EffectField.SetValue(move, new Recharge());
        //     
        //     move = Resources.Load<MoveBase>("Moves/64.Peck");
        //     move = Resources.Load<MoveBase>("Moves/65.Drill Peck");
        //     
        //     move = Resources.Load<MoveBase>("Moves/66.Submission");
        //     BehaviorField.SetValue(move, new Recoil(0.25f));
        //     
        //     move = Resources.Load<MoveBase>("Moves/67.Low Kick");
        //     BehaviorField.SetValue(move, new LowKick());
        //     
        //     move = Resources.Load<MoveBase>("Moves/68.Counter");
        // }
        //
        // private static void Generate71_80()
        // {
        // }
        //

        private static void AddAssetToAddressables(string path)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var assetGuid = AssetDatabase.AssetPathToGUID(path);
            Debug.Log(assetGuid);
            settings.CreateAssetReference(assetGuid);
        }


        [MenuItem("Moves/Generate/from csv")]
        public static void GenerateMoves()
        {
            var moveTexts = ReadCsv();

            AssetDatabase.StartAssetEditing();

            try
            {
                moveTexts.ForEach(GenerateMoveFromText);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }

        private static void GenerateMoveFromText(IReadOnlyList<string> fields)
        {
            // var instance = ScriptableObject.CreateInstance<MoveBase>();
            var path = $"Assets/Game/Addressables/Moves/{fields[0]}.asset";

            var instance = AssetDatabase.LoadAssetAtPath<MoveBase>(path);

            NameField.SetValue(instance, fields[1]);

            Enum.TryParse(fields[2], out PokemonType type);
            TypeField.SetValue(instance, type);

            Enum.TryParse(fields[3], out MoveCategory category);
            CategoryField.SetValue(instance, category);

            int.TryParse(fields[5], out var pp);
            PpField.SetValue(instance, pp);

            int.TryParse(fields[6], out var power);
            PowerField.SetValue(instance, power);

            int.TryParse(fields[7], out var accuracy);
            AccuracyField.SetValue(instance, accuracy);

            DamageField.SetValue(instance, DefaultDamage);
            AccField.SetValue(instance, DefaultAccuracy);
            BehaviorField.SetValue(instance, DefaultBehavior);

            // AssetDatabase.CreateAsset(instance, path);
            // AddAssetToAddressables(path);
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
        }

        private static List<string[]> ReadCsv()
        {
            const string path = "Assets/Game/Addressables/Moves/gen1.csv";
            var reader = new StreamReader(path);

            var list = new List<string[]>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line == null) continue;
                string[] fields = line.Split(',');
                list.Add(fields);
            }

            return list;
        }
    }
}