using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.dataacess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICoverTypeReposirory CoverType { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }

        void Save();
    }
}
