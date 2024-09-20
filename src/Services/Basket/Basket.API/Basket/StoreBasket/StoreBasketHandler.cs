using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket
{

    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string userName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(b => b.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(b => b.Cart.UserName).NotEmpty().WithMessage("User name is required");

        }
    }

    public class StoreBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {

            var shoppingCart = new ShoppingCart()
            {
                Items = command.Cart.Items,
                UserName = command.Cart.UserName,
            };

            await basketRepository.StoreBasket(shoppingCart, cancellationToken);


            return new StoreBasketResult(command.Cart.UserName);
        }
    }
}
