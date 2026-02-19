using Basket.Service.Commands;
using Basket.Service.Data.DTOs;
using Basket.Service.Entities;
using Basket.Service.Responses;
using EventBus.Messages.Events;

namespace Basket.Service.Mappers
{
    public static class BasketMapper
    {
        public static ShoppingCartResponse ToResponse(this ShoppingCart shoppingCart)
        {
            return new ShoppingCartResponse
            {
                UserName = shoppingCart.UserName!,
                Items = shoppingCart.CartItems.Select(i => new ShoppingCartItemsResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    ImageFile = i.ImageFile
                }).ToList()
            };
        }

        public static ShoppingCart ToEntity(this CreateShoppingCartCommand cartCommand)
        {
            return new ShoppingCart
            {
                UserName = cartCommand.userName,
                CartItems = cartCommand.Items.Select(i => new ShoppingCartItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    ImageFile = i.ImageFile
                }).ToList()
            };
        }

        public static ShoppingCart ToEntity(this ShoppingCartResponse response)
        {
            return new ShoppingCart(response.UserName)
            {
                CartItems = response.Items.Select(item => new ShoppingCartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    ImageFile = item.ImageFile

                }).ToList()
            };
        }

        public static BasketCheckoutEvent ToCheckoutBasketEvent(this CheckoutBasketDto basketDto, ShoppingCart cart)
        {
            return new BasketCheckoutEvent
            {
                UserName = basketDto.UserName,
                TotalPrice = cart.CartItems.Sum(i => i.Price * i.Quantity),
                FirstName = basketDto.FirstName,
                LastName = basketDto.LastName,
                EmailAddress = basketDto.EmailAddress,
                AddressLine = basketDto.AddressLine,
                Country = basketDto.Country,
                State = basketDto.State,
                ZipCode = basketDto.ZipCode,
                CardName = basketDto.CardName,
                CardNumber = basketDto.CardNumber,
                Expiration = basketDto.Expiration,
                CVV = basketDto.CVV,
                PaymentMethod = basketDto.PaymentMethod
            };
        }
    }
}
