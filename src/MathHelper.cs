// ReSharper disable InconsistentNaming
namespace Helpers;

/// <summary>
/// Provides mathematical helper methods for trend analysis, statistics calculations, and other operations on data series.
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Computes the trend of a series using the Least Squares method.
    /// </summary>
    /// <param name="serie">List of data points with X and Y coordinates.</param>
    /// <returns>The calculated trend value.</returns>
    public static double GetLeastSquaresTrend(IList<Tuple<float, float>> serie)
    {
        if (!serie.Any()) return 0;

        double sumX = 0, sumY = 0, sumXY = 0, sumXSquare = 0;
        float lastX = 0;

        foreach (var point in serie)
        {
            sumX += point.Item1;
            sumY += point.Item2;
            sumXY += point.Item1 * point.Item2;
            sumXSquare += Math.Pow(point.Item1, 2);
            lastX = point.Item1;
        }

        int n = serie.Count;
        double avgY = sumY / n;
        double avgX = sumX / n;
        double a = (n * sumXY - sumX * sumY) / (n * sumXSquare - Math.Pow(sumX, 2));
        double b = avgY - (a * avgX);

        const int scale = 100;
        double trend = b.NearlyZero() ? (a * lastX) * scale : (a * lastX) / Math.Abs(b) * scale;

        return Math.Round(trend, 2);
    }

    /// <summary>
    /// Computes the minimum, maximum, and average of a series of values.
    /// </summary>
    /// <param name="serie">List of values.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="avg">The average value.</param>
    public static void GetMinMaxAvg(IList<double> serie, out double min, out double max, out double avg)
    {
        min = double.MaxValue;
        max = double.MinValue;
        avg = 0;

        if (!serie.Any()) return;

        double sum = 0;
        foreach (var value in serie)
        {
            sum += value;
            min = Math.Min(min, value);
            max = Math.Max(max, value);
        }

        avg = sum / serie.Count;
    }

    /// <summary>
    /// Returns the discrepancy between the maximum and minimum values in the series.
    /// </summary>
    /// <param name="values">Series of values.</param>
    /// <returns>The discrepancy value.</returns>
    public static double GetDiscrepancy(IEnumerable<double> values)
    {
        var enumerable = values as double[] ?? values.ToArray();
        return enumerable.Any() ? enumerable.Max() - enumerable.Min() : 0;
    }

    /// <summary>
    /// Computes the standard deviation of a series.
    /// </summary>
    /// <param name="values">Series of values.</param>
    /// <param name="average">The average of the series.</param>
    /// <param name="stdDeviation">The standard deviation of the series.</param>
    public static void CalculateStandardDeviation(IEnumerable<double> values, out double? average, out double? stdDeviation)
    {
        var enumerable = values as double[] ?? values.ToArray();
        if (enumerable.Any())
        {
            var avg = enumerable.Average(); // Calcula a média e armazena em uma variável local
            average = avg;
            stdDeviation = Math.Sqrt(enumerable.Average(v => Math.Pow(v - avg, 2))); // Usa a média armazenada para calcular o desvio padrão
        }
        else
        {
            average = null;
            stdDeviation = null;
        }
    }


    /// <summary>
    /// Computes the weighted average of a series of values with associated timestamps.
    /// </summary>
    /// <param name="values">Series of date-value pairs.</param>
    /// <returns>The weighted average value.</returns>
    public static double? GetAverage(IList<KeyValuePair<DateTimeOffset, double>>? values)
    {
        if (values == null || !values.Any()) return null;

        var first = values.First();
        var last = values.Last();
        if (first.Key == last.Key) return first.Value;

        double totalWeightedValue = 0;
        for (int i = 1; i < values.Count; i++)
        {
            totalWeightedValue += ComputeAvgValue(values[i - 1].Value, values[i - 1].Key, values[i].Value, values[i].Key);
        }

        return totalWeightedValue / (last.Key - first.Key).TotalMinutes;
    }

    /// <summary>
    /// Computes the average squared value of a series of values with associated timestamps.
    /// </summary>
    /// <param name="values">Series of date-value pairs.</param>
    /// <returns>The average squared value.</returns>
    public static double? GetAverageSquare(IList<KeyValuePair<DateTimeOffset, double>>? values)
    {
        if (values == null || !values.Any()) return null;

        var first = values.First();
        var last = values.Last();
        if (first.Key == last.Key) return first.Value;

        double totalWeightedSquareValue = 0;
        for (int i = 1; i < values.Count; i++)
        {
            totalWeightedSquareValue += ComputeAvgSquareValue(values[i - 1].Value, values[i - 1].Key, values[i].Value, values[i].Key);
        }

        return totalWeightedSquareValue / (last.Key - first.Key).TotalMinutes / 3;
    }

    /// <summary>
    /// Computes the standard deviation of a series using its average and squared average.
    /// </summary>
    /// <param name="values">Series of date-value pairs.</param>
    /// <param name="average">The average value.</param>
    /// <returns>The standard deviation.</returns>
    public static double? GetStandardDeviation(IList<KeyValuePair<DateTimeOffset, double>>? values, double average)
    {
        double? avgSquare = GetAverageSquare(values);
        if (avgSquare == null) return null;

        return Math.Sqrt(avgSquare.Value - Math.Pow(average, 2));
    }

    /// <summary>
    /// Computes the weighted average value based on two data points and their timestamps.
    /// </summary>
    /// <param name="previousValue">The previous value in the series.</param>
    /// <param name="previousDate">The timestamp of the previous value.</param>
    /// <param name="value">The current value in the series.</param>
    /// <param name="currentDate">The timestamp of the current value.</param>
    /// <returns>The computed weighted average value.</returns>
    private static double ComputeAvgValue(double previousValue, DateTimeOffset previousDate, double value, DateTimeOffset currentDate)
    {
        return ((previousValue + value) / 2) * (currentDate - previousDate).TotalMinutes;
    }

    /// <summary>
    /// Computes the weighted average of the squared values based on two data points and their timestamps.
    /// </summary>
    /// <param name="previousValue">The previous value in the series.</param>
    /// <param name="previousDate">The timestamp of the previous value.</param>
    /// <param name="value">The current value in the series.</param>
    /// <param name="currentDate">The timestamp of the current value.</param>
    /// <returns>The computed weighted average squared value.</returns>
    private static double ComputeAvgSquareValue(double previousValue, DateTimeOffset previousDate, double value, DateTimeOffset currentDate)
    {
        return (currentDate - previousDate).TotalMinutes * (Math.Pow(previousValue, 2) + Math.Pow(value, 2) + (previousValue * value));
    }
}
