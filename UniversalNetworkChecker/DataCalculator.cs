internal class DataCalculator
{
    internal static double GetPercentageValueOf(int failure, int all)
    {
        return (double)failure / (double)all * 100;
    }

    internal static double GetMedianOfList(List<long> values)
    {
        values.Sort();

        int size = values.Count;
        int mid = size / 2;

        double median = (size % 2 != 0) ? (double)values[mid] : ((double)values[mid] + (double)values[mid - 1]) / 2;

        return median;
    }
}