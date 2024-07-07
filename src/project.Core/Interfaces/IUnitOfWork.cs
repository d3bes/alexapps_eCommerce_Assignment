using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Core.Models;

namespace project.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<User> users {get;}
        IBaseRepository<Merchant> merchants {get;}
        IBaseRepository<Product> products {get;}
        IBaseRepository<Cart> carts {get;}
        IBaseRepository<CartItem> cartItems {get;}

        int Complete();

    }
}