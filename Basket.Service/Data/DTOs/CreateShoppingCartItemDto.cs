namespace Basket.Service.Data.DTOs
{
    public record CreateShoppingCartItemDto
    {
        public string? ProductId { get; init; }
        public string? ProductName { get; init; }
        public decimal Price { get; set; }
        public int Quantity { get; init; }
        public string? ImageFile { get; init; }
    }
}