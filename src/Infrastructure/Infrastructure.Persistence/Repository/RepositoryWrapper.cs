using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        //private IFileService _fileRepository;
        //private IAccountRepository _accountRepository;

        private ICategoryRepository _product;
        //private IMailRepository _mail;

        private IWebHostEnvironment _webHostEnvironment;

        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        //private IOptions<EmailSettings> _emailSettings;

        private readonly ISortHelper<AppUser> _appUserSortHelper;
        private readonly ISortHelper<Category> _productSortHelper;
        private readonly ISortHelper<Payment> _paymentSortHelper;
        //private readonly ISortHelper<Workstation> _workstationSortHelper;

        private RepositoryContext _repoContext;
        private UserManager<AppUser> _userManager;
        //private RoleManager<Workstation> _roleManager;

        private string filePath;

        public string Path
        {
            set { filePath = value; }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_product == null)
                {
                    _product = new CategoryRepository(_repoContext, _productSortHelper);
                }
                return _product;
            }
        }




        public RepositoryWrapper(
            UserManager<AppUser> userManager,
            //RoleManager<Workstation> roleManager,
            RepositoryContext repositoryContext,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,

            ISortHelper<AppUser> appUserSortHelper,
            ISortHelper<Category> productSortHelper,
            ISortHelper<Payment> paymentSortHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            _configuration = configuration;
            _repoContext = repositoryContext;


            _appUserSortHelper = appUserSortHelper;
            _productSortHelper = productSortHelper;
            _paymentSortHelper = paymentSortHelper;

            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
