namespace HelloApi.Models.Requests
{
    public class OrderProductRequest
    {
        public int ProductId { get; set; }

        public uint Amount { get; set; }

        public int? OrderId { get; set; }
    }
}
