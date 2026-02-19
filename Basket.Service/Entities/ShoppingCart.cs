namespace Basket.Service.Entities
{
    public class ShoppingCart
    {

        public string? UserName { get; set; }
        public List<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart() { }
        public ShoppingCart(string? userName)
        {
            userName = UserName;
        }


    }
}
