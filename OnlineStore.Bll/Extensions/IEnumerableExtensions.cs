using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Bll.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> predicate) =>
            condition ? enumerable.Where(predicate) : enumerable;
    }
}
