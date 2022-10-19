namespace HelloApi.Models.Requests
{
    public class ProductCreationRequest
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
