using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Contractors
{
    public class CashPaymentService : IPaymentService
    {
        public string Name => "cash";
        public string Title => "Оплата наличными";

        public OrderPayment GetPayment(Form form)
        {
            if (Name != form.ServiceName || !form.isFinal)
                throw new InvalidOperationException("Invalid cash form.");

            return new OrderPayment(Name, "Оплата наличными", new Dictionary<string, string>());
        }

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                       .AddParametr("orderId", order.Id.ToString());
        }

        public Form NextForm( int step, IReadOnlyDictionary<string, string> value)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid cash form.");

            return Form.CreateLast(Name,  step + 1,value);
        }
    }
}
