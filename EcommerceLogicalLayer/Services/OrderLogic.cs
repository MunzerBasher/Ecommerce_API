namespace EcommerceLogicalLayer.Services
{
    public class OrderLogic
    {

        public static int CreateOrder(OrderDTO order)
        {
            return OrderDataAccess.CreateOrder(order);
        }

        public static int UpdateOrder(OrderDTO order)
        {
            return OrderDataAccess.UpdateOrder(order);

        }

        public static int DeleteOrder(int orderId)
        {
            return OrderDataAccess.DeleteOrder(orderId);
        }

        public static List<OrderDTOWithUserName> GetAllOrders()
        {
            return OrderDataAccess.GetAllOrders();
        }

        public static List<OrderDTOWithUserName> RecentlyOrders()
        {
            return OrderDataAccess.RecentlyOrders();
        }


        public static int CountOrders()
        {
            return OrderDataAccess.CountOrders();
        }


        public static int CountOrdersByStatus(int ordersStatus)
        {
            return OrderDataAccess.CountOrdersByStatus(ordersStatus);
        }

        public static int GetOrderTotalPrice(int OrderID)
        {
            return OrderDataAccess.GetOrderTotalPrice(OrderID);
        }
    }
}
