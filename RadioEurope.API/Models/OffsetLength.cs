namespace RadioEurope.API.Models;
/// <summary>
/// Class <c>OffsetLength</c> a model representing a single diff by offset and length.
/// </summary>
public class OffsetLength: IEquatable<OffsetLength>{
    public int offset {get; set;}
    public int length {get; set;}

    public bool Equals(OffsetLength? other)
    {
        if (other == null)
            return false;

        return offset.Equals(other.offset) &&
        length.Equals(other.length);
    }

}
