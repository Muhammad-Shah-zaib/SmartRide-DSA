namespace SmartRide.src.Dtos;

public class DriverDto : IComparable<DriverDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public double Rating { get; set; } // out of 5

    public Node CurrentPosition { get; set; } = new Node(); // current node


    int IComparable<DriverDto>.CompareTo(DriverDto? other)
    {
        throw new NotImplementedException();
    }

}



