﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Contractors
{
    public class Form
    {
        public string UniqueCode { get; }
        public int OrderId { get; }
        public int Step { get; }
        public bool isFinal { get; }     
        public IReadOnlyList<Field> Fields { get; }

        public Form(string uniqueCode, int orderId, int step, bool isFinal, IEnumerable<Field> fields)
        {
            if (string.IsNullOrWhiteSpace(uniqueCode))
                throw new ArgumentNullException(nameof(uniqueCode));
            if(step < 1)
                throw new ArgumentOutOfRangeException(nameof(step));
            if(fields == null)
                throw new ArgumentNullException(nameof(fields));

            UniqueCode = uniqueCode;
            OrderId = orderId;
            Step = step;
            this.isFinal = isFinal;
            Fields = fields.ToArray();
        }
    }
}
