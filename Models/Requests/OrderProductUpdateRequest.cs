using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.Requests
{
    public class OrderProductUpdateRequest
    {
        public int Id { get; set; }

        [Range(1, uint.MaxValue)]
        public uint Amount { get; set; }
    }
}
