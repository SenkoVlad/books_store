using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    public class OrderDelivery
    {
        public string UniqueCode { get; }
        public string Description { get;}
        public decimal Price { get; }
        public IReadOnlyDictionary<string, string> Parametrs { get; }

        public OrderDelivery(string uniqueCode, string description, decimal price, IReadOnlyDictionary<string, string> parametrs)
        {
            if (string.IsNullOrWhiteSpace(uniqueCode))
                throw new ArgumentNullException(nameof(uniqueCode));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description));

            if (parametrs == null)
                throw new ArgumentNullException(nameof(uniqueCode));

            UniqueCode = uniqueCode;
            Description = description;
            Price = price;
            Parametrs = parametrs;
        
        }
    }
}
