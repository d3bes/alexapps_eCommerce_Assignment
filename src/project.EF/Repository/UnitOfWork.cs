using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Core.Interfaces;
using project.Core.Models;

namespace project.EF.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            users = new BaseRepository<User>(_applicationDbContext);
            merchants = new BaseRepository<Merchant>(_applicationDbContext);
            products = new BaseRepository<Product>(_applicationDbContext);
            carts = new BaseRepository<Cart>(_applicationDbContext);
            cartItems = new BaseRepository<CartItem>(_applicationDbContext);

        }
        public IBaseRepository<User> users { get; private set; }

        public IBaseRepository<Merchant> merchants { get; private set; }

        public IBaseRepository<Product> products { get; private set; }

        public IBaseRepository<Cart> carts { get; private set; }
        public IBaseRepository<CartItem> cartItems { get; private set; }

    

    public int Complete()
    {
        return _applicationDbContext.SaveChanges();
    }

    public void Dispose()
    {
        _applicationDbContext.Dispose();
    }

    }
}
