using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock
{
    internal static class MethodExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> actionPerform)
        {
            if(source == null) throw new ArgumentNullException("source");

            foreach (var item in source)
            {
                actionPerform(item);
            }
        }

        public static Type GetElementType(this object list)
        {
            Type listType = list?.GetType();

            if (listType == null)
            {
                return null;
            }

            if (listType.IsArray)
            {
                return listType.GetElementType();
            }

            if (listType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                return listType.IsGenericType
                    ? listType.GetGenericArguments()[0]
                    : typeof(object);
            }

            return null;
        }

        public static bool IsEnumerable(this object list)
        {
            if (list == null) return false;

            Type listType = list.GetType();
            return listType.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public static object ElementAt(this IEnumerable source, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            int currentIndex = 0;
            foreach (var item in source)
            {
                if (currentIndex == index)
                {
                    return item;
                }
                currentIndex++;
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

    }
}
