using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Contractors
{
    public class PostamateDelivaryService : IDeliveryService
    {
        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            {"1", "Брест" },
            {"2", "Минск" }
        };
        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> points = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            {
                "1",
                new Dictionary<string, string>
                {
                    {"1", "Вокзал-Брест 1" },
                    {"2", "Вокзал-Брест 2" },
                    {"3", "Вокзал-Брест 3" },
                }
            },
            {
                "2",
                new Dictionary<string, string>
                {
                    {"1", "Вокзал-Минск 1" },
                    {"2", "Вокзал-Минск 2" },
                    {"3", "Вокзал-Минск 3" },
                }
            }
        };

        public string UniqueCode => "postamate";
        public string Title => "Доставка с помощью постамата";
        public Form CreateForm(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return new Form(UniqueCode, order.Id, 1, false, new[]
            {
                new SelectedField("Город", "city", "1", cities)
            });
        }

        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> value)
        {
            if(step == 1)
            {
                if (value["city"] == "1")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("Город", "city", "1"),
                        new SelectedField("Постамат", "postamate", "1", points["1"])
                    });
                }
                else if (value["city"] == "2")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("Город", "city", "2"),
                        new SelectedField("Постамат", "postamate", "2", points["2"])
                    });
                }
                else
                    throw new InvalidOperationException("Invalid Postamate");
            }
            else if(step == 2)
            {
                return new Form(UniqueCode, orderId, 3, true, new Field[]
                {
                    new HiddenField("Город", "city", value["city"]),
                    new HiddenField("Город", "city", value["postamate"])
                });
            }
            else 
                throw new InvalidOperationException("Invalid Postamate step");
        }
    }
}
