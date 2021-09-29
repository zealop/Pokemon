using System.Collections.Generic;

public class TypeChart
{
    static float[][] chart =
    {
        //                       Nor   Fir   Wat   Ele   Gra   Ice   Fig   Poi   Gro   Fly   Psy   Bug   Roc   Gho   Dra   Dar  Ste    Fai
        /*Normal*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0,    1f,   1f,   0.5f, 1f},
        /*Fire*/    new float[] {1f,   0.5f, 0.5f, 1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 1f,   2f,   1f},
        /*Water*/   new float[] {1f,   2f,   0.5f, 1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f,   1f},
        /*Electric*/new float[] {1f,   1f,   2f,   0.5f, 0.5f, 1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f},
        /*Grass*/   new float[] {1f,   0.5f, 2f,   1f,   0.5f, 1f,   1f,   0.5f, 2f,   0.5f, 1f,   0.5f, 2f,   1f,   0.5f, 1f,   0.5f, 1f},
        /*Ice*/     new float[] {1f,   0.5f, 0.5f, 1f,   2f,   0.5f, 1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f},
        /*Fighting*/new float[] {2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f, 0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   0.5f},
        /*Poison*/  new float[] {1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   0f,   2f},
        /*Ground*/  new float[] {1f,   2f,   1f,   2f,   0.5f, 1f,   1f,   2f,   1f,   0f,   1f,   0.5f, 2f,   1f,   1f,   1f,   2f,   1f},
        /*Flying*/  new float[] {1f,   1f,   1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 1f},
        /*Psychic*/ new float[] {1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   0f,   0.5f, 1f},
        /*Bug*/     new float[] {1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f,   0.5f, 2f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 0.5f},
        /*Rock*/    new float[] {1f,   2f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f},
        /*Ghost*/   new float[] {0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 1f,   1f},
        /*Dragon*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 0f},
        /*Dark*/    new float[] {1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f},
        /*Steel*/   new float[] {1f,   0.5f, 0.5f, 0.5f, 1f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 2f},
        /*Fairy*/   new float[] {1f,   0.5f, 1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   0.5f, 1f},
    };

    public static float GetEffectiveness(PokemonType attackType, List<PokemonType> defenseTypes)
    {
        float result = 1f;
        foreach (var defenseType in defenseTypes)
        {
            if (defenseType == PokemonType.None)
                continue;

            int row = (int)attackType - 1;
            int col = (int)defenseType - 1;
            result *= chart[row][col];
        }
        return result;
    }
}
public class EXPChart
{
    static int[][] chart =
    {
        new int[] {0,0,0,0,0,0,},
        new int[] {15,6,8,9,10,4,},
        new int[] {52,21,27,57,33,13,},
        new int[] {122,51,64,96,80,32,},
        new int[] {237,100,125,135,156,65,},
        new int[] {406,172,216,179,270,112,},
        new int[] {637,274,343,236,428,178,},
        new int[] {942,409,512,314,640,276,},
        new int[] {1326,583,729,419,911,393,},
        new int[] {1800,800,1000,560,1250,540,},
        new int[] {2369,1064,1331,742,1663,745,},
        new int[] {3041,1382,1728,973,2160,967,},
        new int[] {3822,1757,2197,1261,2746,1230,},
        new int[] {4719,2195,2744,1612,3430,1591,},
        new int[] {5737,2700,3375,2035,4218,1957,},
        new int[] {6881,3276,4096,2535,5120,2457,},
        new int[] {8155,3930,4913,3120,6141,3046,},
        new int[] {9564,4665,5832,3798,7290,3732,},
        new int[] {11111,5487,6859,4575,8573,4526,},
        new int[] {12800,6400,8000,5460,10000,5440,},
        new int[] {14632,7408,9261,6458,11576,6482,},
        new int[] {16610,8518,10648,7577,13310,7666,},
        new int[] {18737,9733,12167,8825,15208,9003,},
        new int[] {21012,11059,13824,10208,17280,10506,},
        new int[] {23437,12500,15625,11735,19531,12187,},
        new int[] {26012,14060,17576,13411,21970,14060,},
        new int[] {28737,15746,19683,15244,24603,16140,},
        new int[] {31610,17561,21952,17242,27440,18439,},
        new int[] {34632,19511,24389,19411,30486,20974,},
        new int[] {37800,21600,27000,21760,33750,23760,},
        new int[] {41111,23832,29791,24294,37238,26811,},
        new int[] {44564,26214,32768,27021,40960,30146,},
        new int[] {48155,28749,35937,29949,44921,33780,},
        new int[] {51881,31443,39304,33084,49130,37731,},
        new int[] {55737,34300,42875,36435,53593,42017,},
        new int[] {59719,37324,46656,40007,58320,46656,},
        new int[] {63822,40522,50653,43808,63316,50653,},
        new int[] {68041,43897,54872,47846,68590,55969,},
        new int[] {72369,47455,59319,52127,74148,60505,},
        new int[] {76800,51200,64000,56660,80000,66560,},
        new int[] {81326,55136,68921,61450,86151,71677,},
        new int[] {85942,59270,74088,66505,92610,78533,},
        new int[] {90637,63605,79507,71833,99383,84277,},
        new int[] {95406,68147,85184,77440,106480,91998,},
        new int[] {100237,72900,91125,83335,113906,98415,},
        new int[] {105122,77868,97336,89523,121670,107069,},
        new int[] {110052,83058,103823,96012,129778,114205,},
        new int[] {115015,88473,110592,102810,138240,123863,},
        new int[] {120001,94119,117649,109923,147061,131766,},
        new int[] {125000,100000,125000,117360,156250,142500,},
        new int[] {131324,106120,132651,125126,165813,151222,},
        new int[] {137795,112486,140608,133229,175760,163105,},
        new int[] {144410,119101,148877,141677,186096,172697,},
        new int[] {151165,125971,157464,150476,196830,185807,},
        new int[] {158056,133100,166375,159635,207968,196322,},
        new int[] {165079,140492,175616,169159,219520,210739,},
        new int[] {172229,148154,185193,179056,231491,222231,},
        new int[] {179503,156089,195112,189334,243890,238036,},
        new int[] {186894,164303,205379,199999,256723,250562,},
        new int[] {194400,172800,216000,211060,270000,267840,},
        new int[] {202013,181584,226981,222522,283726,281456,},
        new int[] {209728,190662,238328,234393,297910,300293,},
        new int[] {217540,200037,250047,246681,312558,315059,},
        new int[] {225443,209715,262144,259392,327680,335544,},
        new int[] {233431,219700,274625,272535,343281,351520,},
        new int[] {241496,229996,287496,286115,359370,373744,},
        new int[] {249633,240610,300763,300140,375953,390991,},
        new int[] {257834,251545,314432,314618,393040,415050,},
        new int[] {267406,262807,328509,329555,410636,433631,},
        new int[] {276458,274400,343000,344960,428750,459620,},
        new int[] {286328,286328,357911,360838,447388,479600,},
        new int[] {296358,298598,373248,377197,466560,507617,},
        new int[] {305767,311213,389017,394045,486271,529063,},
        new int[] {316074,324179,405224,411388,506530,559209,},
        new int[] {326531,337500,421875,429235,527343,582187,},
        new int[] {336255,351180,438976,447591,548720,614566,},
        new int[] {346965,365226,456533,466464,570666,639146,},
        new int[] {357812,379641,474552,485862,593190,673863,},
        new int[] {367807,394431,493039,505791,616298,700115,},
        new int[] {378880,409600,512000,526260,640000,737280,},
        new int[] {390077,425152,531441,547274,664301,765275,},
        new int[] {400293,441094,551368,568841,689210,804997,},
        new int[] {411686,457429,571787,590969,714733,834809,},
        new int[] {423190,474163,592704,613664,740880,877201,},
        new int[] {433572,491300,614125,636935,767656,908905,},
        new int[] {445239,508844,636056,660787,795070,954084,},
        new int[] {457001,526802,658503,685228,823128,987754,},
        new int[] {467489,545177,681472,710266,851840,1035837,},
        new int[] {479378,563975,704969,735907,881211,1071552,},
        new int[] {491346,583200,729000,762160,911250,1122660,},
        new int[] {501878,602856,753571,789030,941963,1160499,},
        new int[] {513934,622950,778688,816525,973360,1214753,},
        new int[] {526049,643485,804357,844653,1005446,1254796,},
        new int[] {536557,664467,830584,873420,1038230,1312322,},
        new int[] {548720,685900,857375,902835,1071718,1354652,},
        new int[] {560922,707788,884736,932903,1105920,1415577,},
        new int[] {571333,730138,912673,963632,1140841,1460276,},
        new int[] {583539,752953,941192,995030,1176490,1524731,},
        new int[] {591882,776239,970299,1027103,1212873,1571884,},
        new int[] {600000,800000,1000000,1059860,1250000,1640000,},
    };

    public static int GetEXPAtLevel(GrowthType type, int level)
    {
        int row = level - 1;
        int col = (int)type;

        return chart[row][col];
    }
}
