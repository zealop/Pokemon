using System;
using Random = UnityEngine.Random;

namespace Game.Utils
{
    /// <summary>
    /// A PRNG intended to emulate the on-cartridge PRNG for Gen 5 with a 64-bit initial seed.
    /// This simulates the on-cartridge PRNG used in the real games.
    /// In addition to potentially allowing us to read replays from in-game,
    /// this also makes it possible to record an "input log"
    /// (a seed + initial teams + move/switch decisions) and "replay" a simulation to get the same result.
    /// </summary>
    public class PRNG
    {
        /** 64-bit big-endian [high -> low] int */
        private int[] initialSeed;

        private int[] seed;

        public PRNG(int[] seed = null)
        {
            seed ??= GenerateSeed();

            this.initialSeed = seed.Clone() as int[];
            this.seed = seed.Clone() as int[];
        }

        /// <summary>
        /// Getter to the initial seed.
        /// This should be considered a hack and is only here for backwards compatibility.
        /// </summary>
        public int[] StartingSeed => this.initialSeed;

        /// <summary>
        /// Creates a clone of the current PRNG.
        /// The new PRNG will have its initial seed set to the seed of the current instance.
        /// </summary>
        public PRNG Clone()
        {
            return new PRNG(this.seed);
        }

        public double Next(int? from = null, int? to = null)
        {
            this.seed = NextFrame(this.seed); //Advance the RNG
            double result = (int)((uint)(this.seed[0] << 16) >> 0) + this.seed[1]; // Use the upper 32 bits

            if (from is null)
            {
                result /= 0x100000000;
            }
            else if (to is null)
            {
                result = Math.Floor(result * from.Value / 0x100000000);
            }
            else
            {
                result = Math.Floor(result * (to.Value - from.Value) / 0x100000000) + from.Value;
            }

            return result;
        }

        /// <summary>
        /// Flip a coin (two-sided die), returning true or false.
        /// This function returns true with probability `P`, where `P = numerator
        /// / denominator`. This function returns false with probability `1 - P`.
        /// The numerator must be a non-negative integer (`>= 0`).
        /// The denominator must be a positive integer (`> 0`).
        /// </summary>
        public bool RandomChance(int numerator, int denominator)
        {
            return this.Next(denominator) < numerator;
        }

        /// <summary>
        /// Return a random item from the given array.
        /// This function chooses items in the array with equal probability.
        /// If there are duplicate items in the array, each duplicate is
        /// considered separately. For example, sample(['x', 'x', 'y']) returns
        /// 'x' 67% of the time and 'y' 33% of the time.
        /// The array must contain at least one item.
        /// The array must not be sparse.
        /// </summary>
        public T Sample<T>(T[] items)
        {
            if (items.Length == 0)
            {
                throw new SystemException("Cannot sample an empty array");
            }

            var index = (int)this.Next(items.Length);
            var item = items[index];
            if (item is null)
            {
                throw new SystemException("Cannot sample sparse array");
            }

            return item;
        }


        /// <summary>
        /// A Fisher-Yates shuffle. This is how the game resolves speed ties.
        ///
        /// At least according to V4 in
        /// https://github.com/smogon/pokemon-showdown/issues/1157#issuecomment-214454873
        /// </summary>
        private void Shuffle<T>(T[] items, int start = 0, int? end = null)
        {
            end ??= items.Length;
            while (start < end - 1)
            {
                var nextIndex = (int)this.Next(start, end);
                if (start != nextIndex)
                {
                    var temp = items[start];
                    items[start] = items[nextIndex];
                    items[nextIndex] = items[start];
                }

                start++;
            }
        }

        private static int[] MultiplyAdd(int[] a, int[] b, int[] c)
        {
            var ret = new int[4];
            var carry = 0;

            for (var outIndex = 3; outIndex >= 0; outIndex--)
            {
                for (var bIndex = outIndex; bIndex < 4; bIndex++)
                {
                    var aIndex = 3 - (bIndex - outIndex);

                    carry += a[aIndex] * b[bIndex];
                }

                carry += c[outIndex];
                ret[outIndex] = carry & 0xFFFF;
                carry = (int)((uint)carry >> 16);
            }

            return ret;
        }

        /// <summary>
        /// The RNG is a Linear Congruential Generator (LCG) in the form: `x_{n + 1} = (a x_n + c) % m`
        /// Where: `x_0` is the seed, `x_n` is the random number after n iterations,
        ///
        /// ````
        /// a = 0x5D588B656C078965
        /// c = 0x00269EC3
        /// m = 2^64
        ///````
        /// </summary>
        private static int[] NextFrame(int[] seed, int framesToAdvance = 1)
        {
            var a = new int[] { 0x5D58, 0x8B65, 0x6C07, 0x8965 };
            var c = new int[] { 0, 0, 0x26, 0x9EC3 };

            for (var i = 0; i < framesToAdvance; i++)
            {
                seed = MultiplyAdd(seed, a, c);
            }

            return seed;
        }

        private static int[] GenerateSeed()
        {
            return new[]
            {
                (int)Math.Floor(Random.value * 0x10000),
                (int)Math.Floor(Random.value * 0x10000),
                (int)Math.Floor(Random.value * 0x10000),
                (int)Math.Floor(Random.value * 0x10000),
            };
        }
    }
}