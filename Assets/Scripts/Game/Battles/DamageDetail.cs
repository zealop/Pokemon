using UnityEngine;

namespace Game.Battles
{
    public class DamageDetail
    {
        public float Base { get; }
        public float CritMod { get; }
        public float StabMob { get; }
        public float TypeMod { get; }

        public int Value => Mathf.FloorToInt(Base * CritMod * StabMob * TypeMod);
        public DamageDetail(float @base, float critMod = 1, float stabMob = 1, float typeMod = 1)
        {
            Base = @base;
            CritMod = critMod;
            StabMob = stabMob;
            TypeMod = typeMod;
        }
    }
}