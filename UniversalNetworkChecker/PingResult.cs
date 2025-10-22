internal class PingResult
{
    internal long RoundTripTime { get; set; }
    internal bool IsPingSuccessful { get; set; }

    internal PingResult(long roundTripTime, bool isSuccessful)
    {
        RoundTripTime = roundTripTime;
        IsPingSuccessful = isSuccessful;
    }
}
