using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMusicStoreApplication.Models
{
    public class ShoppingCart
    {
        public string ShoppingCartId;

        private MVCMusicStoreDB db = new MVCMusicStoreDB();

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            ShoppingCart cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        private string GetCartId(HttpContextBase context)
        {
            const string CartSessionId = "CartId";
            string cartId;

            if(context.Session[CartSessionId] == null)
            {
                //create a new cart id
                cartId = Guid.NewGuid().ToString();
                //save to the session state
                context.Session[CartSessionId] = cartId;
            }
            else
            {
                //return the existing cart id
                cartId = context.Session[CartSessionId].ToString();
            }

            return cartId;
        }

        public List<Cart> GetCartItems()
        {
           return db.Carts.Where(c => c.CartId == this.ShoppingCartId).ToList(); 
        }

        public decimal GetCartTotal()
        {
            decimal? total = (from cartItem in db.Carts
                         where cartItem.CartId == this.ShoppingCartId
                         select cartItem.AlbumSelected.Price * (int?)cartItem.Count).Sum() ;

            return total ?? decimal.Zero;
        }

        public void AddToCart(int albumId)
        {
            //TODO:Verify that the Album ID exists in the database
            Cart cartItem = db.Carts.SingleOrDefault(c => c.CartId == this.ShoppingCartId && c.AlbumId == albumId);

            if(cartItem == null)
            {
                //Item is not cart, add new cart item
                cartItem = new Cart()
                {
                    CartId = this.ShoppingCartId,
                    AlbumId = albumId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };db.Carts.Add(cartItem);
            }
            else
            {
                //Item is already; inccrease item count (quantity)
                cartItem.Count++;
            }

            db.SaveChanges();
        }

        public int RemoveFromCart(int recordId)
        {
            Cart cartItem = db.Carts.SingleOrDefault(c => c.CartId == this.ShoppingCartId && c.RecordId == recordId);

            if (cartItem == null)
            {
                throw new NotImplementedException();
            }

            int newCount;

            if(cartItem.Count > 1)
            {
                //The count >1; decrement count
                cartItem.Count--;
                newCount = cartItem.Count;
            }
            else
            {
                //The count <= 0; remove cart item
                db.Carts.Remove(cartItem);
                newCount = 0;
            }

            db.SaveChanges();

            return newCount;
        }
    }
}

   