using EventBus.Messages.Events;
using Newtonsoft.Json;
using Orders.Service.Commands;
using Orders.Service.DTOs;
using Orders.Service.Entities;
using Orders.Service.Helpers;

namespace Orders.Service.Mapper
{
    public static class OrderMapper
    {
        public static OrderDTO ToDto(this Order order) =>
            new OrderDTO(
                order.Id,
                order.UserName,
                order.TotalPrice ?? 0,
                order.FirstName,
                order.LastName,
                order.EmailAddress,
                order.AddressLine,
                order.Country,
                order.State,
                order.ZipCode,
                order.CardName,
                order.CardNumber,
                order.CVV,
                order.ExpirationDate,
                order.PaymentMethod
            );


        public static Order ToEntity(this CheckoutOrderCommand command) =>
            new Order
            {
                UserName = command.UserName,
                TotalPrice = command.TotalPrice,
                FirstName = command.FirstName,
                LastName = command.LastName,
                EmailAddress = command.EmailAddress,
                AddressLine = command.AddressLine,
                Country = command.Country,
                State = command.State,
                ZipCode = command.ZipCode,
                CardName = command.CardName,
                CardNumber = command.CardNumber,
                CVV = command.CVV,
                PaymentMethod = command.PaymentMethod
            };

        public static void MapUpdate(this Order orderToUpdate, UpdateOrderCommand command)
        {
            orderToUpdate.UserName = command.UserName;
            orderToUpdate.TotalPrice = command.TotalPrice;
            orderToUpdate.FirstName = command.FirstName;
            orderToUpdate.LastName = command.LastName;
            orderToUpdate.EmailAddress = command.EmailAddress;
            orderToUpdate.AddressLine = command.AddressLine;
            orderToUpdate.Country = command.Country;
            orderToUpdate.State = command.State;
            orderToUpdate.ZipCode = command.ZipCode;
            orderToUpdate.CardName = command.CardName;
            orderToUpdate.CardNumber = command.CardNumber;
            orderToUpdate.CVV = command.CVV;
            orderToUpdate.ExpirationDate = command.ExpirationDate;
            orderToUpdate.PaymentMethod = command.PaymentMethod;
        }


        public static CheckoutOrderCommand ToCommand(this CheckoutOrderDTO order)
        {
            return new CheckoutOrderCommand
            {
                UserName = order.UserName,
                TotalPrice = order.TotalPrice,
                FirstName = order.FirstName,
                LastName = order.LastName,
                EmailAddress = order.EmailAddress,
                AddressLine = order.AddressLine,
                Country = order.Country,
                State = order.State,
                ZipCode = order.ZipCode,
                CardName = order.CardName,
                CardNumber = order.CardNumber,
                CVV = order.CVV,
                PaymentMethod = order.PaymentMethod,
            };
        }

        public static UpdateOrderCommand ToCommand(this OrderDTO order)
        {
            return new UpdateOrderCommand
            {
                Id = order.Id,
                UserName = order.UserName,
                TotalPrice = order.TotalPrice,
                FirstName = order.FirstName,
                LastName = order.LastName,
                EmailAddress = order.EmailAddress,
                AddressLine = order.AddressLine,
                Country = order.Country,
                State = order.State,
                ZipCode = order.ZipCode,
                CardName = order.CardName,
                CardNumber = order.CardNumber,
                CVV = order.CVV,
                ExpirationDate = order.ExpirationDate,
                PaymentMethod = order.PaymentMethod,
            };
        }

        public static CheckoutOrderCommand ToCheckoutOrderCommand(this BasketCheckoutEvent message)
        {
            return new CheckoutOrderCommand
            {
                UserName = message.UserName,
                TotalPrice = message.TotalPrice,
                FirstName = message.FirstName,
                LastName = message.LastName,
                EmailAddress = message.EmailAddress,
                AddressLine = message.AddressLine,
                Country = message.Country,
                State = message.State,
                ZipCode = message.ZipCode,
                CardName = message.CardName,
                CardNumber = message.CardNumber,
                CVV = message.CVV,
                ExpirationDate = message.Expiration,
                PaymentMethod = message.PaymentMethod.GetValueOrDefault()
            };
        }

        public static OutBoxMessage ToOutBoxMessage(Order order, Guid CorrelationId)
        {
            return new OutBoxMessage
            {
                CorrelationId = CorrelationId.ToString(),
                Type = OutboxMessageTypes.OrderCreated,
                OccurredOn = DateTime.UtcNow,
                Content = JsonConvert.SerializeObject(new
                {
                    order.Id,
                    order.UserName,
                    order.TotalPrice,
                    order.FirstName,
                    order.LastName,
                    order.EmailAddress,
                    order.AddressLine,
                    order.Country,
                    order.State,
                    order.ZipCode,
                    order.ExpirationDate,
                    //PCI Senstive Data should be encrypted or tokenized in real world applications
                    order.CardName,
                    order.CardNumber,
                    order.CVV,
                    order.PaymentMethod
                })
            };
        }

        public static OutBoxMessage ToOutBoxMessageForUpdate(Order orderToUpdate, Guid CorrelationId)
        {
            return new OutBoxMessage
            {
                CorrelationId = CorrelationId.ToString(),
                Type = OutboxMessageTypes.OrderCreated,
                OccurredOn = DateTime.UtcNow,
                Content = JsonConvert.SerializeObject(new
                {
                    orderToUpdate.Id,
                    orderToUpdate.UserName,
                    orderToUpdate.TotalPrice,
                    orderToUpdate.FirstName,
                    orderToUpdate.LastName,
                    orderToUpdate.EmailAddress,
                    orderToUpdate.AddressLine,
                    orderToUpdate.Country,
                    orderToUpdate.State,
                    orderToUpdate.ZipCode,
                    orderToUpdate.ExpirationDate,
                    //PCI Senstive Data should be encrypted or tokenized in real world applications
                    orderToUpdate.CardName,
                    orderToUpdate.CardNumber,
                    orderToUpdate.CVV,
                    orderToUpdate.PaymentMethod
                })
            };
        }
    }
}



