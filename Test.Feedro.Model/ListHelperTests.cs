using Feedro.Model.Helper;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Linq;

namespace Test.Feedro.Model
{
    [TestClass]
    public class ListHelperTests
    {
        protected void InsertAndVerify(int[] left, int[] right)
        {
            int[] expected = left.Concat(right).OrderBy(i => i).ToArray();

            var list = new List<int>(left);
            list.OrderdInsertRange(right, Comparer<int>.Default);
            Assert.IsTrue(expected.SequenceEqual(list));

            list = new List<int>(right);
            list.OrderdInsertRange(left, Comparer<int>.Default);
            Assert.IsTrue(expected.SequenceEqual(list));
        }

        [TestMethod]
        public void OrderedInsertRange_Insertion_OneItem()
        {
            var list = new int[] { 1, 3, 5 };
            int min = list.Min() - 1;
            int max = list.Max() + 1;

            foreach (int i in Enumerable.Range(min, max - min + 1))
                InsertAndVerify(new int[] { i }, list);
        }

        [TestMethod]
        public void OrderedInsertRange_Insertion_TwoDifferentItem()
        {
            var list = new int[] { 1, 3, 5 };
            int min = list.Min() - 1;
            int max = list.Max() + 1;

            foreach (int i in Enumerable.Range(min, max - min + 1))
                foreach (int j in Enumerable.Range(i, max - i + 1))
                    InsertAndVerify(new int[] { i, j }, list);
        }

        [TestMethod]
        public void OrderedInsertRange_Insertion_Empty()
        {
            InsertAndVerify(
                new int[] { },
                new int[] { 1, 2, 3 });
        }
    }
}
