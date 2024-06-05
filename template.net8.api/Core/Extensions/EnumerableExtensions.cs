using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Extensions;

[CoreLibrary]
internal static class EnumerableExtensions
{
    /// <exception cref="ArgumentException">Container is empty!</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
    internal static T MinElement<T, TR>(this IEnumerable<T> container, Func<T, TR> valuingFoo) where TR : IComparable
    {
        using var enumerator = container.GetEnumerator();
        if (!enumerator.MoveNext()) throw new ArgumentException("Container is empty!");
        var minElem = enumerator.Current;
        var minVal = valuingFoo(minElem);
        while (enumerator.MoveNext())
        {
            var currentVal = valuingFoo(enumerator.Current);
            if (currentVal.CompareTo(minVal) >= 0) continue;
            minVal = currentVal;
            minElem = enumerator.Current;
        }

        return minElem;
    }

    /// <exception cref="ArgumentException">Container is empty!</exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
    internal static T MaxElement<T, TR>(this IEnumerable<T> container, Func<T, TR> valuingFoo) where TR : IComparable
    {
        using var enumerator = container.GetEnumerator();
        if (!enumerator.MoveNext()) throw new ArgumentException("Container is empty!");
        var maxElem = enumerator.Current;
        var maxVal = valuingFoo(maxElem);
        while (enumerator.MoveNext())
        {
            var currentVal = valuingFoo(enumerator.Current);
            if (currentVal.CompareTo(maxVal) <= 0) continue;
            maxVal = currentVal;
            maxElem = enumerator.Current;
        }

        return maxElem;
    }

    /// <exception cref="ArgumentException">Container is empty!</exception>
    /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
    internal static IEnumerable<T> MaxElements<T, TR>(this IEnumerable<T> container, Func<T, TR> valuingFoo)
        where TR : IComparable
    {
        using var enumerator = container.GetEnumerator();

        if (!enumerator.MoveNext()) throw new ArgumentException("Container is empty!");

        var (maxElements, _) = FindMaxElements(enumerator, valuingFoo);

        return maxElements;
    }

    private static (List<T> maxElements, List<TR> maxValues) FindMaxElements<T, TR>(IEnumerator<T> enumerator,
        Func<T, TR> valuingFoo)
        where TR : IComparable
    {
        var maxElements = new List<T> { enumerator.Current };
        var maxValues = new List<TR> { valuingFoo(maxElements[0]) };

        while (enumerator.MoveNext())
        {
            var currentVal = valuingFoo(enumerator.Current);
            var comparisonResult = currentVal.CompareTo(maxValues[0]);

            switch (comparisonResult)
            {
                case < 0:
                    continue;
                case 0:
                    maxValues.Add(currentVal);
                    maxElements.Add(enumerator.Current);
                    break;
                default:
                    maxValues = [currentVal];
                    maxElements = [enumerator.Current];
                    break;
            }
        }

        return (maxElements, maxValues);
    }
}