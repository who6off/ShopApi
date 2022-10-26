using System.ComponentModel.DataAnnotations;

namespace HelloApi.Models.Requests
{
    public class OrderCreationRequest
    {
        public OrderCreationItem[] Items { get; set; }
    }

    public class OrderCreationItem
    {
        public int ProductId { get; set; }

        [Range(1, uint.MaxValue)]
        public uint Amount { get; set; }
    }
}
