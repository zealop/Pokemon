using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using Utils;

namespace Tests.EditMode
{
    public class WeightedRngTest

    {
        private static readonly Dictionary<int, int> Table = new()
        {
            { 2, 7 }, { 3, 7 }, { 4, 3 }, { 5, 3 }
        };

        private const int Iteration = 1_000_000;

        // [Test]
        public void TestPound()
        {
            int c3, c4, c5;
            var c2 = c3 = c4 = c5 = 0;
            for (var i = 0; i < Iteration; i++)
            {
                switch (WeightedRng.Roll(Table))
                {
                    case 2:
                        c2++;
                        break;
                    case 3:
                        c3++;
                        break;
                    case 4:
                        c4++;
                        break;
                    case 5:
                        c5++;
                        break;
                }
            }

            Assert.That(c2, Is.EqualTo(350000).Within(1000));
            Assert.That(c3, Is.EqualTo(350000).Within(1000));
            Assert.That(c4, Is.EqualTo(150000).Within(1000));
            Assert.That(c5, Is.EqualTo(150000).Within(1000));
        }
    }
}