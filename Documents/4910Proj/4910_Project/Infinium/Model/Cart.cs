using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinium.Model
{
    public class Cart
    {
        //not set yet
        private int _id;
        private int _userID;
        private Sponsor sponsor;

        private List<Product> _prods_in_cart;

        public Cart(int id)
        {
            _userID = id;
            _id = id;
            _prods_in_cart = new List<Product>();

        }

        public int getID()
        {
            return _id;
        }

        public void setID(int id)
        {
            _id = id;
        }

        public List<CartItem> getCartItems()
        {
            List<CartItem> cartItems = new List<CartItem>();

            foreach (Product product in _prods_in_cart)
            {
                CartItem foundItem = null;

                foreach (CartItem ci in cartItems)
                {
                    if (ci.GetProduct().GetId() == product.GetId())
                    {
                        foundItem = ci;
                        continue;
                    }
                }

                if (foundItem != null)
                {
                    foundItem.IncQuantity();
                } else
                {
                    cartItems.Add(new CartItem(product));
                }
            }

            return cartItems;
        }

        public List<Product> getProducts()
        {
            return _prods_in_cart;
        }

        public double getProdCost()
        {
            double cost = 0;

            foreach (Product product in _prods_in_cart)
            {
                cost += product.GetPrice();
            }

            return cost;
        }

        public double getTotalCost()
        {
            return Math.Round((getProdCost() + getShipCost() + getTaxCost()), 2);
        }

        public int getPointsCost(Sponsor sponsor)
        {
            if (getTotalCost() == 0 || sponsor == null)
                return 0;

            if (getTotalCost() % sponsor.GetUsdToPoints() == 0)
            {
                return (int) (getTotalCost() / sponsor.GetUsdToPoints());
            }

            return ((int)(getTotalCost() / sponsor.GetUsdToPoints())) + 1;
        }

        public double getShipCost()
        {
            return 5.0; //TODO: change when when find out calculation
        }

        public double getTaxCost()
        {
            return Math.Round((0.05 * getProdCost()), 2);
        }

        public int getQuantity()
        {
            return _prods_in_cart.Count();
        }

        public void addToCart(Product newprod)
        {
            _prods_in_cart.Add(newprod);
        }
    }
}