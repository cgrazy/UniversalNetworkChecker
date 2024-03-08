// See https://aka.ms/new-console-template for more information

using System.Diagnostics.CodeAnalysis;

public class Host
{
    [NotNull]
    public required string Hostname { get; set; }

    [NotNull]
    public required string IP { get; set; }
}
