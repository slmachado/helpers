namespace Helpers;

public static class TimeseriesHelper
{
    /// <summary>
    /// Return period under threshold
    /// </summary>
    /// <param name="timeseriesValues"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static IEnumerable<Tuple<DateTimeOffset, DateTimeOffset>> GetPeriodeSpansUnderThreshold(this IEnumerable<KeyValuePair<DateTimeOffset, double>> timeseriesValues, double threshold)
    {
        var spans = new List<Tuple<DateTimeOffset, DateTimeOffset>>();
        DateTimeOffset? start = null;
        DateTimeOffset? previous = null;

        foreach (var timeserieValue in timeseriesValues)
        {
            if (timeserieValue.Value < threshold)
            {
                if (start.HasValue == false)
                {
                    start = timeserieValue.Key;
                }

                previous = timeserieValue.Key;
            }
            else
            {
                if (!start.HasValue || !previous.HasValue) continue;
                spans.Add(new Tuple<DateTimeOffset, DateTimeOffset>(start.Value, previous.Value));
                start = null;
                previous = null;
            }
        }


        return spans;
    }

    /// <summary>
    /// Exclude from timeseries values the date periode passed in parameter
    /// </summary>
    /// <param name="timeseriesValues"></param>
    /// <param name="spansToExclude"></param>
    public static IEnumerable<KeyValuePair<DateTimeOffset, double>> ExcludeSpan(this IEnumerable<KeyValuePair<DateTimeOffset, double>> timeseriesValues, IEnumerable<Tuple<DateTimeOffset, DateTimeOffset>>? spansToExclude)
    {
        var lstCounterData = new List<KeyValuePair<DateTimeOffset, double>>();
        var enumerator = timeseriesValues.GetEnumerator();
        using IDisposable enumerator1 = enumerator;
        var hasValue = false;
        if (spansToExclude != null)
        {
            var toExclude = spansToExclude as Tuple<DateTimeOffset, DateTimeOffset>[] ?? spansToExclude.ToArray();
            if (toExclude.Any())
            {
                foreach (var span in toExclude)
                {
                    if (hasValue)
                    {
                        if (enumerator.Current.Key < span.Item1)
                        {
                            lstCounterData.Add(enumerator.Current);
                        }
                    }

                    hasValue = enumerator.MoveNext();
                    while (hasValue)
                    {
                        if (enumerator.Current.Key < span.Item1)
                        {
                            lstCounterData.Add(enumerator.Current);
                        }

                        if (enumerator.Current.Key > span.Item2)
                        {
                            break;
                        }

                        hasValue = enumerator.MoveNext();
                    }
                }
            }
        }

        if (hasValue)
        {
            lstCounterData.Add(enumerator.Current);
        }

        while (enumerator.MoveNext())
        {
            lstCounterData.Add(enumerator.Current);
        }

        return lstCounterData;
    }
}