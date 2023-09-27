

using System.Collections.Generic;

namespace EventAggregatorQuickstart
{
    public class ShoppingCart
    {
        private List<ShoppingCartItem> _items = new List<ShoppingCartItem>();

        public void AddItem(ShoppingCartItem item)
        {
            lock (_items)
            {
                _items.Add(item);
            }
        }

        public int Count
        {
            get
            {
                lock (_items)
                {
                    return _items.Count;
                }
            }
        }
    }
}
