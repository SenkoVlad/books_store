using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Contractors;
using Store.Web.App;
using Store.Web.Contractors;
using Store.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private OrderService orderService;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly IEnumerable<IwebContractorService> webContractorServices;

        public OrderController(OrderService orderService,
                              IEnumerable<IDeliveryService> deliveryServices,
                              IEnumerable<IPaymentService> paymentServices,
                              IEnumerable<IwebContractorService> webContractorServices)
        {
            this.orderService = orderService;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.webContractorServices = webContractorServices;
        }
        [HttpPost]
        public IActionResult UpdateItem(int bookId, int count)
        {
            orderService.UpdateBook(bookId, count);

            return RedirectToAction("Index", "Order", new { id = bookId });
        }
        [HttpPost]
        public IActionResult AddItem(int bookId, int count = 1)
        {
            orderService.AddBook(bookId, count);
            
            return RedirectToAction("Index", "Book", new { id = bookId });
        }
        [HttpPost]
        public IActionResult RemoveItem(int bookId)
        {
            orderService.RemoveBook(bookId);

            return RedirectToAction("Index", "Order", new { id = bookId });
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (orderService.TryGetModel(out OrderModel model))
                return View(model);

            return View("Empty"); 
        }
        [HttpPost]
        public IActionResult SendConfirmation(string cellPhone)
        {
            var model = orderService.SendConfirmation(cellPhone);
          
            if(model.Errors.Count > 0)
                return View("Index", model);

            return View("Confirmation", model);
        }
        public IActionResult Confirmate(string cellPhone, int code)
        {
            var model = orderService.ConfirmCellPhone(cellPhone, code);

            if (model.Errors.Count > 0)
                return View("Confirmation", model);

            var deliveryMethod = deliveryServices.ToDictionary(service => service.Name,
                                                               service => service.Title);
            return View("DeliveryMethod", deliveryMethod);
        }
        [HttpPost]
        public IActionResult StartDelivery(int id, string serviceName)
        {
            var deliveryService = deliveryServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = deliveryService.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);
            if (webContractorService == null)
                return View("DeliveryStep", form);

            var returnUri = GetReturnUri(nameof(DeliveryStep));
            var redirectUri = webContractorService.StartSession(form.Parameters, new Uri(returnUri.ToString()));

            return Redirect(redirectUri.ToString());
        }

        private Uri GetReturnUri(string action)
        {
            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = Url.Action(action),
                Query = null
            };

            if(Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;

            return builder.Uri;
        }

        [HttpPost]
        public IActionResult DeliveryStep(string serviceName, int step, Dictionary<string, string> values)
        {
            var deliveryService = deliveryServices.Single(service => service.Name == serviceName);
            var form = deliveryService.NextForm(step, values);

            if (!form.isFinal)
                return View("DeliveryStep", form);

            var delivery = deliveryService.GetDelivery(form);
            orderService.SetDelivery(delivery);

            var paymentMethods = paymentServices.ToDictionary(service => service.Name,
                                                             service => service.Title);

            return View("PaymentMethod", paymentMethods);
        }

        [HttpPost]
        public IActionResult StartPayment(string serviceName)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = paymentService.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);
            
            if (webContractorService == null)
                return View("PaymentStep", form);

            var returnUri = GetReturnUri(nameof(PaymentStep));
            var redirectUri = webContractorService.StartSession(form.Parameters, returnUri);

            return Redirect(redirectUri.ToString());
        }

        [HttpPost]
        public IActionResult PaymentStep(string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);
            var form = paymentService.NextForm(step, values);

            if (!form.isFinal)
            {
                return View("PaymentStep", form);
            }

            var payment = paymentService.GetPayment(form);
            var model = orderService.SetPayment(payment);

            return View("Finish", model);
        }
    }
}
