using EcommerceDataLayer.Ropesitry;




namespace EcommerceLogicalLayer.Services
{
    public class OrderItemsLogic
    {


        public static int CreateOrderItem(OrderItemDTO orderItem)
        {
            if (orderItem.Price < 0 || orderItem.Quantity < 1 || orderItem.OrderItemID < 1 || orderItem.OrderID < 1)
            {
                throw new ArgumentException("OrderItemID and Quantity must be greater than 0.");
            }
            return OrderItemDataAccess.CreateOrderItem(orderItem);
        }

        public static bool UpdateOrderItem(OrderItemDTO orderItem)
        {
            if (orderItem.Price < 0 || orderItem.Quantity < 1 || orderItem.OrderItemID < 1 || orderItem.OrderID < 1)
            {
                throw new ArgumentException("OrderItemID and Quantity must be greater than 0.");
            }
            return OrderItemDataAccess.UpdateOrderItem(orderItem);
        }

        public static bool DeleteOrderItem(int orderItemID)
        {
            if (orderItemID < 1)
            {
                throw new ArgumentException("OrderItemID Is Invalid ");
            }
            return OrderItemDataAccess.DeleteOrderItem(orderItemID);
        }

        public static List<OrderItemDTO> GetOrderItemsByOrderID(int orderID)
        {
            return OrderItemDataAccess.GetOrderItemsByOrderID(orderID);
        }

        public static bool UpdateOrderItemQuantity(int orderItemID, int quantity)
        {
            if (orderItemID < 1 || quantity < 1)
            {
                throw new ArgumentException("OrderItemID and Quantity must be greater than 0.");
            }

            int rowsAffected = OrderItemDataAccess.UpdateOrderItemQuantity(orderItemID, quantity);

            return rowsAffected > 0;
        }

    }
}