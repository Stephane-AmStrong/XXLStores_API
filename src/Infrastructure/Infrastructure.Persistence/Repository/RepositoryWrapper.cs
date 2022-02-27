using Application.Interfaces;
using Domain.Entities;
using Domain.Settings;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        private IEmailService _email;
        private IFileService _file;

        private readonly ISortHelper<AppUser> _appUserSortHelper;
        private readonly ISortHelper<Category> _categorySortHelper;
        private readonly ISortHelper<InventoryLevel> _inventoryLevelSortHelper;
        private readonly ISortHelper<Item> _itemSortHelper;
        private readonly ISortHelper<Payment> _paymentSortHelper;
        private readonly ISortHelper<Shop> _shopSortHelper;
        private readonly ISortHelper<ShoppingCart> _shoppingCartSortHelper;
        private readonly ISortHelper<ShoppingCartItem> _shoppingCartItemSortHelper;

        private readonly MailSettings _mailSettings;
        private readonly RepositoryContext _repoContext;
        private readonly UserManager<AppUser> _userManager;
        //private RoleManager<Workstation> _roleManager;

        private IAccountRepository _account;
        private ICategoryRepository _category;
        private IInventoryLevelRepository _inventoryLevel;
        private IItemRepository _item;
        private IPaymentRepository _payment;
        private IShopRepository _shop;
        private IShoppingCartRepository _shoppingCart;
        private IShoppingCartItemRepository _shoppingCartItem;

        private string filePath;

        public string Path
        {
            set { filePath = value; }
        }



        public IAccountRepository Account => throw new System.NotImplementedException();

        public IAppUserRepository AppUser => throw new System.NotImplementedException();


        public IEmailService Email
        {
            get
            {
                if (_email == null)
                {
                    _email = new EmailService(_mailSettings);
                }
                return _email;
            }
        }


        public IFileService File
        {
            get
            {
                if (_file == null)
                {
                    _file = new FileService(_webHostEnvironment, filePath);
                }
                return _file;
            }
        }


        //public IAccountRepository Account
        //{
        //    get
        //    {
        //        if (_account == null)
        //        {
        //            _account = new AccountService(_repoContext, _userManager, _roleManager, _configuration, _httpContextAccessor);
        //        }
        //        return _account;
        //    }
        //}

        
        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_repoContext, _categorySortHelper);
                }
                return _category;
            }
        }


        public IInventoryLevelRepository InventoryLevel
        {
            get
            {
                if (_inventoryLevel == null)
                {
                    _inventoryLevel = new InventoryLevelRepository(_repoContext, _inventoryLevelSortHelper);
                }
                return _inventoryLevel;
            }
        }


        public IItemRepository Item
        {
            get
            {
                if (_item == null)
                {
                    _item = new ItemRepository(_repoContext, _itemSortHelper);
                }
                return _item;
            }
        }


        public IPaymentRepository Payment
        {
            get
            {
                if (_payment == null)
                {
                    _payment = new PaymentRepository(_repoContext, _paymentSortHelper);
                }
                return _payment;
            }
        }


        public IShopRepository Shop
        {
            get
            {
                if (_shop == null)
                {
                    _shop = new ShopRepository(_repoContext, _shopSortHelper);
                }
                return _shop;
            }
        }


        public IShoppingCartRepository ShoppingCart
        {
            get
            {
                if (_shoppingCart == null)
                {
                    _shoppingCart = new ShoppingCartRepository(_repoContext, _shoppingCartSortHelper);
                }
                return _shoppingCart;
            }
        }


        public IShoppingCartItemRepository ShoppingCartItem
        {
            get
            {
                if (_shoppingCartItem == null)
                {
                    _shoppingCartItem = new ShoppingCartItemRepository(_repoContext, _shoppingCartItemSortHelper);
                }
                return _shoppingCartItem;
            }
        }

        /*
            private readonly ISortHelper<ShoppingCartItem> _shoppingCartItemSortHelper;
         */


        public RepositoryWrapper(
            UserManager<AppUser> userManager,
            RepositoryContext repositoryContext,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,

            ISortHelper<AppUser> appUserSortHelper,
            ISortHelper<Category> categorySortHelper,
            ISortHelper<InventoryLevel> inventoryLevelSortHelper,
            ISortHelper<Item> itemSortHelper,
            ISortHelper<Payment> paymentSortHelper,
            ISortHelper<Shop> shopSortHelper,
            ISortHelper<ShoppingCart> shoppingCartSortHelper,
            ISortHelper<ShoppingCartItem> shoppingCartItemSortHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _configuration = configuration;
            _repoContext = repositoryContext;

            _appUserSortHelper = appUserSortHelper;
            _categorySortHelper = categorySortHelper;
            _inventoryLevelSortHelper = inventoryLevelSortHelper;
            _itemSortHelper = itemSortHelper;
            _paymentSortHelper = paymentSortHelper;
            _shopSortHelper = shopSortHelper;
            _shoppingCartSortHelper = shoppingCartSortHelper;
            _shoppingCartItemSortHelper = shoppingCartItemSortHelper;

            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessor;
        }


        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
