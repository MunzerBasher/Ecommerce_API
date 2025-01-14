public class OrderDTO
{
    public int OrderID { get; set; }
    public required int UserID { get; set; }
    public DateTime OrderDate { get; set; }
    public required decimal TotalAmount { get; set; }
    public  int Status { get; set; }
}

public class OrderDTOWithUserName
{
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public string UserName { get; set; }
}

