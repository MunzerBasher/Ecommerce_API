public class AddressRequest
{

    public string UserId { get; set; } = string.Empty;
    public required string AddressLine { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required decimal Longitude { get; set; }
    public required decimal Latitude { get; set; }


}
