using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Contractors
{
    public class Form
    {
        public string ServiceName { get; }
        public int Step { get; }
        public bool isFinal { get; }
        private readonly Dictionary<string, string> parametrs;
        public IReadOnlyDictionary<string, string> Parameters => parametrs;
        private readonly List<Field> fields;
        public IReadOnlyList<Field> Fields => fields;

        public Form(string servicename, int step, bool isfinal, IReadOnlyDictionary<string, string> parametrs)
        {
            if (string.IsNullOrWhiteSpace(servicename))
                throw new ArgumentNullException(nameof(servicename));
            if(step < 1)
                throw new ArgumentOutOfRangeException(nameof(step));

            ServiceName = servicename;
            Step = step;
            isFinal = isfinal;

            if (parametrs == null)
                this.parametrs = new Dictionary<string, string>();
            else
                this.parametrs = parametrs.ToDictionary(p => p.Key, 
                                                        p => p.Value);
            fields = new List<Field>();
        }
        public static Form CreateFirst(string serviceName)
        {
            return new Form(serviceName, 1, false, null);
        }
        public static Form CreateNext(string serviceName, int step, IReadOnlyDictionary<string, string> parametrs) 
        {
            if(parametrs == null)
                throw new ArgumentNullException(nameof(parametrs));

            return new Form(serviceName, step, isfinal: false, parametrs);
        }
        public static Form CreateLast(string serviceName, int step, IReadOnlyDictionary<string, string> parametrs)
        {
            if (parametrs == null)
                throw new ArgumentNullException(nameof(parametrs));

            return new Form(serviceName, step, isfinal: true, parametrs);
        }
        public Form AddParametr(string name, string value)
        {
            parametrs.Add(name, value);
            return this;
        }
        public Form AddField(Field field)
        {
            fields.Add(field);
            return this;
        }
    }
}
