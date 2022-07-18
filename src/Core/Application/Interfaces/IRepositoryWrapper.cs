using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepositoryWrapper
    {
        IAccountRepository Account { get; }
        IAppUserRepository AppUser { get; }
        ICategoryRepository Category { get; }
        IInventoryLevelRepository InventoryLevel { get; }
        IItemRepository Item { get; }
        IPaymentRepository Payment { get; }
        IShopRepository Shop { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IShoppingCartItemRepository ShoppingCartItem { get; }
        ITokenService Token { get; }

        IFileService File { get; }
        IEmailService Email { get; }

        string Path { set; }

        Task SaveAsync();
    }
}
