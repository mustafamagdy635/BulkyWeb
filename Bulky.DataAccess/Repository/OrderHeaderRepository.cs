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
    public  class OrderHeaderRepository :Repository<OrderHeader>,IOrderHeaderRepository
    {
        private readonly ApplicationDbContext db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;

        }

        public void Update(OrderHeader obj)
        {
            db.orderHeaders.Update(obj);
        }

        public void UpdateStatus(int ID, string orderStatus, string? paymentStatus = null)
        {
           var orderFromDb =db.orderHeaders.FirstOrDefault(u=>u.Id == ID);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }

        }

        public void UpdateStripePaymentID(int ID, string sessionId, string PaymentID)
        {
            var orderFromDb = db.orderHeaders.FirstOrDefault(u => u.Id == ID);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(PaymentID))
            {
                orderFromDb.PaymentIntentId = PaymentID;
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }
    }
    
}
