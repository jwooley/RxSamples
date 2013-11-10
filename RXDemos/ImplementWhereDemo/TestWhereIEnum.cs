using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace RxDemo
{
    public class TestWhereIEnum
    {
        private IEnumerable<int> wholeNums;
        private IEnumerable<int> repeats;
        private IEnumerable<int> empty;


        public TestWhereIEnum()
        {
            Random rnd = new Random();
            repeats = Enumerable.Repeat(rnd.Next(int.MaxValue-100), 10);
            wholeNums = Enumerable.Range(0, 100);
            Enumerable.Empty<int>();

        }

        [Fact]
        public void TestRealVsYeild()
        {
            var expected = wholeNums.Where(x => x > wholeNums.First() + 5);
            var actual = RxDemo.CustomWhereCombinator.CustomWhere(wholeNums, x => x > wholeNums.First() + 5);
            Assert.Equal(expected.ToArray(), actual.ToArray());
        }

        [Fact]
        public void TestRealVPedantic()
        {
            var expected = wholeNums.Where(x => x > wholeNums.First() + 5);
            var actual = RxDemo.CustomWhereCombinator.CustomWhereNoYield(wholeNums, x => x > wholeNums.First() + 5);
            Assert.Equal(expected.ToArray(), actual.ToArray());
        }

        [Fact]
        public void TestRealVsYeildRepeatsMatch()
        {
            var expected = repeats.Where(x => x == repeats.First());
            var actual = RxDemo.CustomWhereCombinator.CustomWhere(repeats, x => x == repeats.First());
            Assert.Equal(expected.ToArray(), actual.ToArray());
        }

        [Fact]
        public void TestRealVPedanticRepeatsMatch()
        {
            var expected = repeats.Where(x => x == repeats.First());
            var actual = RxDemo.CustomWhereCombinator.CustomWhereNoYield(repeats, x => x == repeats.First());
            Assert.Equal(expected.ToArray(), actual.ToArray());
        }



    }
}
