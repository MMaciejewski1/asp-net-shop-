using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testowo.DAL;
using testowo.Models;

namespace testowo.Infrastructure
{
    public class CartManager
    {
        private CourseContext db;
        private ISessionManager session;

        public CartManager(ISessionManager session, CourseContext db) {

            this.session = session;
            this.db = db;

        }

        public List<CartPosition> GetCart() {

            List<CartPosition> cart;
            if (session.Get<List<CartPosition>>(Consts.CartSessionKey) == null)
            {
                cart = new List<CartPosition>();
            }
            else
            {
                cart = session.Get<List<CartPosition>>(Consts.CartSessionKey) as List<CartPosition>; 
            }

            return cart;

        }

        public void AddToCart(int idcourse) {

            var cart = GetCart();
            var positionCart = cart.Find(k=>k.course.CourseId == idcourse);

            if (positionCart != null)
            {
                positionCart.count++;
            }
            else
            {
                var CourseToAdd = db.Course.Where(k => k.CourseId == idcourse).SingleOrDefault();

                if (CourseToAdd != null)
                    {
                    var newPositionCart = new CartPosition { course= CourseToAdd,count=1,value= Decimal.ToDouble(CourseToAdd.PriceCourse) };
                    cart.Add(newPositionCart);

                }
            }
            session.Set(Consts.CartSessionKey,cart);
        }
        public int DeleteFromCart(int idcourse) {
            var cart = GetCart();
            var positionCart = cart.Find(k => k.course.CourseId == idcourse);
            if (positionCart != null)
            {
                if (positionCart.count > 1)
                {   positionCart.count--;
                return positionCart.count;
                }
                else cart.Remove(positionCart);
               
            }
            return 0;
        }
        public decimal GetValueCart() {
            var cart = GetCart();

            return cart.Sum(k=>(k.count*k.course.PriceCourse));
        }
        public int GetCountPositionCart()
        {
            var cart = GetCart();

            return cart.Sum(k => (k.count));
        }
        public Order CreateOrder(Order newOrder,string userId) {

            var cart = GetCart();
            newOrder.AddDate = DateTime.Now;
            newOrder.UserId = userId;
           
            if (newOrder.OrderItem == null) 
                newOrder.OrderItem = new List<OrderItem>();

                decimal cartValue = 0;

                foreach (var orderItem in cart)
                {
                    var newPositionOrder = new OrderItem() {
                        CourseId = orderItem.course.CourseId,
                        Amount = orderItem.count,
                        PurchasePrice = orderItem.course.PriceCourse

                    };

                    cartValue += orderItem.count * orderItem.course.PriceCourse;
                    newOrder.OrderItem.Add(newPositionOrder);
                }
                newOrder.OrderValue = cartValue;
         db.Order.Add(newOrder);
            db.SaveChanges();
               
            
            return newOrder;
        }

        public void EmptyCart() {
            session.Set<List<OrderItem>>(Consts.CartSessionKey,null);
        }

    }
}