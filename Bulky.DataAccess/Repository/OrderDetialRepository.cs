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
    public  class OrderDetialRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
        private readonly ApplicationDbContext db;

        public OrderDetialRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;

        }

        public void Update(OrderDetail obj)
        {
            db.orderDetails.Update(obj);
        }

    }
}
