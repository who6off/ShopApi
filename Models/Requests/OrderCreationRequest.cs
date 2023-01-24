using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.Requests
{
    public class OrderCreationRequest
    {
        public OrderRequestItem[] Items { get; set; }
    }

    public class OrderRequestItem
    {
        public int ProductId { get; set; }

        [Range(1, uint.MaxValue)]
        public uint Amount { get; set; }
    }
}
