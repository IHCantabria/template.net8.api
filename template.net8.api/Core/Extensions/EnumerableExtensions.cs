using LanguageExt;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Extensions;

[CoreLibrary]
internal static class EnumerableExtensions
{
    /// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
    /// <exception cref="NotSupportedException">The enumerator does not support being reset.</exception>
    internal static Try<T> MinElement<T, Tr>(this IEnumerable<T> container, Func<T, Tr> valuingFoo)
        where Tr : IComparable
    {
        return () =>
        {
            using var enumerator = container.GetEnumerator();

            // Find the minimum value
            var minValue = FindMinValue(enumerator, valuingFoo);

            // Reset the enumerator and find the element corresponding to the minimum value
            enumerator.Reset(); // Resetting the enumerator
            return FindMinElement(enumerator, valuingFoo, minValue);
        };
    }

    private static Tr FindMinValue<T, Tr>(IEnumerator<T> enumerator, Func<T, Tr> valuingFoo)
        where Tr : IComparable
    {
        if (!enumerator.MoveNext()) throw new ArgumentException("Container is empty!");

        var minVal = valuingFoo(enumerator.Current);

        while (enumerator.MoveNext())
        {
            var currentVal = valuingFoo(enumerator.Current);
            if (currentVal.CompareTo(minVal) < 0) minVal = currentVal;
        }

        return minVal;
    }

    private static T FindMinElement<T, Tr>(IEnumerator<T> enumerator, Func<T, Tr> valuingFoo, Tr minValue)
        where Tr : IComparable
    {
        enumerator.Reset(); // Reset the enumerator to start from the beginning

        while (enumerator.MoveNext())
        {
            var currentVal = valuingFoo(enumerator.Current);
            if (currentVal.CompareTo(minValue) == 0) return enumerator.Current;
        }

        throw new InvalidOperationException("Minimum element not found!");
    }

    /// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
    /// <exception cref="NotSupportedException">The enumerator does not support being reset.</exception>
    internal static Try<T> MaxElement<T, Tr>(this IEnumerable<T> container, Func<T, Tr> valuingFoo)
        where Tr : IComparable
    {
        return () =>
        {
            using var enumerator = container.GetEnumerator();

            // Find the maximum value
            var maxValue = FindMaxValue(enumerator, valuingFoo);

            // Reset the enumerator and find the element corresponding to the maximum value
            enumerator.Reset();
            return FindMaxElement(enumerator, valuingFoo, maxValue);
        };
    }

    private static Tr FindMaxValue<T, Tr>(IEnumerator<T> enumerator, Func<T, Tr> valuingFoo)
        where Tr : IComparable
    {
        if (!enumerator.MoveNext()) throw new ArgumentException("Container is empty!");

        var maxVal = valuingFoo(enumerator.Current);

        while (enumerator.MoveNext())
        {
            var currentVal = valuingFoo(enumerator.Current);
            if (currentVal.CompareTo(maxVal) > 0) maxVal = currentVal;
        }

        return maxVal;
    }

    private static T FindMaxElement<T, Tr>(IEnumerator<T> enumerator, Func<T, Tr> valuingFoo, Tr maxValue)
        where Tr : IComparable
    {
        enumerator.Reset(); // Reset the enumerator to start from the beginning

        while (enumerator.MoveNext())
        {
            var currentVal = valuingFoo(enumerator.Current);
            if (currentVal.CompareTo(maxValue) == 0) return enumerator.Current;
        }

        throw new InvalidOperationException("Maximum element not found!");
    }

    /// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
    /// <exception cref="ArgumentException">Container is empty!</exception>
    internal static Try<IEnumerable<T>> MaxElements<T, Tr>(this IEnumerable<T> container, Func<T, Tr> valuingFoo)
        where Tr : IComparable
    {
        return () =>
        {
            using var enumerator = container.GetEnumerator();

            if (!enumerator.MoveNext()) throw new ArgumentException("Container is empty!");

            var (maxElements, _) = FindMaxElements(enumerator, valuingFoo);

            return maxElements;
        };
    }

    private static (List<T> maxElements, List<Tr> maxValues) FindMaxElements<T, Tr>(IEnumerator<T> enumerator,
        Func<T, Tr> valuingFoo)
        where Tr : IComparable
    {
        // Find the maximum values
        var maxValues = FindMaxValues(enumerator, valuingFoo);

        // Find the elements corresponding to the maximum value
        var maxElements = FindMaxElements(enumerator, valuingFoo, maxValues[0]);

        return (maxElements, maxValues);
    }

    private static List<T> FindMaxElements<T, Tr>(IEnumerator<T> enumerator, Func<T, Tr> valuingFoo, Tr maxValue)
        where Tr : IComparable
    {
        var maxElements = new List<T>();

        enumerator.Reset(); // Resetting the enumerator to start from the beginning

        while (enumerator.MoveNext())
        {
            var currentVal = valuingFoo(enumerator.Current);
            if (currentVal.CompareTo(maxValue) == 0) maxElements.Add(enumerator.Current);
        }

        return maxElements;
    }

    private static List<Tr> FindMaxValues<T, Tr>(IEnumerator<T> enumerator, Func<T, Tr> valuingFoo)
        where Tr : IComparable
    {
        var maxValues = new List<Tr> { valuingFoo(enumerator.Current) };

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
                    break;
                default:
                    maxValues = [currentVal];
                    break;
            }
        }

        return maxValues;
    }
}