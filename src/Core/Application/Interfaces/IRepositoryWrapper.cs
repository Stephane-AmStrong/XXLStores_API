using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepositoryWrapper
    {
        IFileService FileService { get; }

        IAccountService AccountService { get; }
        IEmailService EmailService { get; }

        ICategoryRepository Category { get; }
        IInventoryLevelRepository InventoryLevel { get; }
        IItemRepository Item { get; }
        IPaymentRepository Payment { get; }
        IShopRepository Shop { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IShoppingCartItemRepository ShoppingCartItem { get; }

        string Path { set; }

        Task SaveAsync();
    }
}
