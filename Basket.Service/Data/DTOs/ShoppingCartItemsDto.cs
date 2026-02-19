namespace Basket.Service.Data.DTOs
{
    public record ShoppingCartItemsDto
    (
        string ProductId,
        string ProductName,
        string ImageFile,
        decimal Price,
        int Quantity
    );
}