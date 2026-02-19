namespace Basket.Service.Data.DTOs
{
    public record ShoppingCartDto
  (
         string? UserName,
         List<ShoppingCartItemsDto> Items,
         decimal TotalPrice
   );
}
