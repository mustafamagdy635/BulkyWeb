using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationDbContext db;

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }

		public IApplicationUserRepository ApplicationUser { get; private set; }

		public IOrderDetailRepository OrderDetail { get; private set; }

		public IOrderHeaderRepository OrderHeader { get; private set; }

		public UnitOfWork( ApplicationDbContext db)
        {
            this.db = db;
            Category = new CategoryRepository(db);
            Product  = new ProductRepository(db);
            Company = new CompanyRepository(db);
			ApplicationUser = new ApplicationUserRepository(db); 
            ShoppingCart = new ShoppingCartRepository(db);
			OrderDetail = new OrderDetialRepository(db);
			OrderHeader = new OrderHeaderRepository(db); 
        }


        public void Save()
        {
            db.SaveChanges();
        }
    }
}
