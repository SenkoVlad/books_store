using System;
using System.Collections.Generic;
using System.Linq;
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

        public string Name => "postamate";
        public string Title => "Доставка с помощью постамата";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                       .AddParametr("orderId", order.Id.ToString())
                       .AddField(new SelectedField("Город", "city", "1", cities));
        }

        public OrderDelivery GetDelivery(Form form)
        {
            if (form.ServiceName != Name || !form.isFinal)
                throw new InvalidOperationException("Invalid form");

            var cityId = form.Parameters["city"];
            var cityName = cities[cityId];
            var postamateId = form.Parameters["postamate"];
            var postamateName = points[cityId][postamateId];

            var parameters = new Dictionary<string, string>
            {
                {nameof(cityId), cityId },
                {nameof(cityName), cityName },
                {nameof(postamateId), postamateId },
                {nameof(postamateName), postamateName }
            };

            var description = $"Город: {cityName}\nПостамат: {postamateName}";

            return new OrderDelivery(Name, description, 150m, parameters);
        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> value)
        {
            if (step == 1)
            {
                if (value["city"] == "1")
                {
                    return Form.CreateNext(Name, step  + 1, value)
                               .AddField(new SelectedField("Постамат", "postamate", "1", points["1"]));
                }
                else if (value["city"] == "2")
                {
                    return Form.CreateNext(Name, step + 1, value)
                               .AddField(new SelectedField("Постамат", "postamate", "2", points["2"]));
                }
                else
                    throw new InvalidOperationException("Invalid Postamate");
            }
            else if (step == 2)
            {
                return Form.CreateLast(Name, step + 1, value);
            }
            else
                throw new InvalidOperationException("Invalid Postamate step");
        }
    }
}
