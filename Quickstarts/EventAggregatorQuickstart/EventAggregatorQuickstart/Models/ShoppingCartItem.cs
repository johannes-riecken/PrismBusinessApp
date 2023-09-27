

namespace EventAggregatorQuickstart
{
    public class ShoppingCartItem
    {
        public string Name { get; private set; }
        public decimal Cost { get; private set; }

        public ShoppingCartItem(string name, decimal cost)
        {
            Name = name;
            Cost = cost;
        }
    }
}
