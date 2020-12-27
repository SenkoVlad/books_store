﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Contractors
{
    public class CashPaymentService : IPaymentService
    {
        public string UniqueCode => "cash";
        public string Title => "Оплата наличными";

        public OrderPayment GetPayment(Form form)
        {
            if (UniqueCode != form.UniqueCode || !form.isFinal)
                throw new InvalidOperationException("Invalid cash form.");

            return new OrderPayment(UniqueCode, "Оплата наличными", new Dictionary<string, string>());
        }

        public Form CreateForm(Order order)
        {
            return new Form(UniqueCode, order.Id, 1, true, new Field[0]);
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> value)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid cash form.");

            return new Form(UniqueCode, orderId, 2, true, new Field[0]);
        }

    }
}
