using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinium.Model
{
    public class CartItem
    {

        private Product product;
        private int quantity = 1;

        public CartItem(Product product)
        {
            this.product = product;
        }

        public Product GetProduct()
        {
            return product;
        }

        public int GetQuantity()
        {
            return quantity;
        }

        public void IncQuantity()
        {
            quantity++;
        }
        public void DecQuantity()
        {
            quantity--;
        }

    }
}
