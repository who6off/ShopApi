namespace HelloApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Product[]? Products { get; set; }
    }
}
