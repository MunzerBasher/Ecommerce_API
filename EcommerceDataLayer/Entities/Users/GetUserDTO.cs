public class GetUserDTO
{
    public required string UserName { get; set; }
    public string UserEmail { get; set; }
    public string UserPhone { get; set; }
    public bool UserApprove { get; set; }
    public DateTime UserDate { get; set; }
    public string UserPermission { get; set; }

}
