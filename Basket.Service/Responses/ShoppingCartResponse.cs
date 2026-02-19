namespace Basket.Service.Responses
{
    public record class ShoppingCartResponse
    {
        public string UserName { get; init; }
        public List<ShoppingCartItemsResponse> Items { get; init; } = new List<ShoppingCartItemsResponse>();

        // Parameterless constructor
        public ShoppingCartResponse()
        {
            UserName = string.Empty;
            Items = new List<ShoppingCartItemsResponse>();
        }

        // Constructor with username only
        public ShoppingCartResponse(string userName) : this()
        {
            UserName = userName;
            Items = new List<ShoppingCartItemsResponse>();
        }

        public decimal Totalprice => Items.Sum(i => i.Price * i.Quantity);
    }
}
