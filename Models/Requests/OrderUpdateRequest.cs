namespace ShopApi.Models.Requests
{
    public class OrderUpdateRequest
    {
        public int OrderId { get; set; }

        public OrderRequestItem[] Items { get; set; }
    }
}
