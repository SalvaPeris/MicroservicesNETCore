using Basket.API.Entities;

namespace Basket.API.Repository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);

        Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart);

        Task DeleteBasket(string userName);
    }
}
