using Microsoft.AspNetCore.Http;
using Store.Contractors;
using Store.Web.Contractors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.YandexKassa
{
    public class YandexKassaPaymentService : IwebContractorService, IPaymentService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public string Name => "YandexKassa";
        public string Title => "Оплата банковской картой";
        public string GetUri => "/YandexKassa/";
        private HttpRequest Request => httpContextAccessor.HttpContext.Request;

        public YandexKassaPaymentService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                       .AddParametr("orderId", order.Id.ToString());
        }

        public OrderPayment GetPayment(Form form)
        {
            if (form.ServiceName != Name || !form.isFinal)
                throw new InvalidOperationException("Invalid payment form");

            return new OrderPayment(Name, "Оплата картой", form.Parameters);
        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> value)
        {
            if(step != 1)
                throw new InvalidOperationException("Invalid YandexKassa step");

            return Form.CreateLast(Name, step + 1, value);
        }

        public Uri StartSession(IReadOnlyDictionary<string, string> parameters, Uri returnUri)
        {
            var queryString = QueryString.Create(parameters);
            queryString += QueryString.Create("returnUri", returnUri.ToString());

            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = "YandexKassa/",
                Query = queryString.ToString(),
            };

            if (Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;

            return builder.Uri;
        }
    }
}
