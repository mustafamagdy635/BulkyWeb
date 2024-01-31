using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public  class ShoppingCartRepository :Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;

        }
        public void Update(ShoppingCart obj)
        {
            db.ShoppingCarts.Update(obj);   
        }


    }
}
