namespace RadioEurope.API.Models;
/// <summary>
/// Class <c>LeftRightDiff</c> model of data structure to be stored in sata store (redis).
/// </summary>
public class LeftRightDiff
{
    public string? ID { get; set; }
    public string? Left { get; set; }
    public string? Right { get; set; }
    public string? Diff { get; set; }
}
