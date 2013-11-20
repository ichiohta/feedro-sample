using System.Collections.Generic;
using System.Linq;

namespace Feedro.Model.Helper
{
    public static class ListHelper
    {
        public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> toRemove)
        {
            foreach (T item in toRemove.ToArray())
                list.Remove(item);
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> toAdd)
        {
            foreach (T item in toAdd.ToArray())
                list.Add(item);
        }

        public static void OrderdInsertRange<T>(this IList<T> orderedList, IEnumerable<T> orderedItems, IComparer<T> comparer)
        {
            int indexToInsert = 0;

            foreach (T item in orderedItems)
            {
                while (indexToInsert < orderedList.Count && comparer.Compare(orderedList[indexToInsert], item) < 0)
                    indexToInsert++;
                orderedList.Insert(indexToInsert, item);
            }
        }
    }
}
