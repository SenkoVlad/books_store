using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;

        public OrderController(IBookRepository bookRepository,
                              IOrderRepository orderRepository,
                              INotificationService notificationService)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
        }
        [HttpPost]
        public IActionResult UpdateItem(int bookId, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            
            order.GetItem(bookId).Count = count;
            SaveOrderAndCart(order, cart);

            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Order", new { id = bookId });
        }
        [HttpPost]
        public IActionResult AddItem(int bookId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            var book = bookRepository.GetById(bookId);

            order.AddOrUpdateItem(book, count);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Book", new { id = bookId });
        }
        [HttpPost]
        public IActionResult RemoveItem(int bookId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            order.RemoveItem(bookId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order", new { id = bookId });
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;

            HttpContext.Session.Set(cart);
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                OrderModel model = Map(order);

                return View(model);
            }
            return View("Empty"); 
        }

        private OrderModel Map(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            var books = bookRepository.GetAllByIds(bookIds);
            var itemModels = from item in order.Items
                             join book in books on item.BookId equals book.Id
                             select new OrderItemModel
                             {
                                 BookId = book.Id,
                                 Title = book.Title,
                                 Author = book.Author,
                                 Price = item.Price,
                                 Count = item.Count
                             };

            return new OrderModel
            {
                Id = order.Id,
                Items = itemModels.ToArray(),
                State = order.State,
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice
            };
        }

        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }

            return (order, cart);
        }

        [HttpPost]
        public IActionResult SendConfirmation(int id, string cellPhone)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            if (!IsValidCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Пустой или не соответствует формату +XXXXXXXXXXX";
                return View("Index", model);
            }

            var code = 1111;
            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);
            model.CellPhone = cellPhone;

            return View("Confirmation", new ConfirmationModel { 
                orderId = id,
                CellPhone = cellPhone
            });
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            cellPhone = cellPhone?.Replace(" ", "")
                                 ?.Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }
        public IActionResult StartDelivery(int id, string cellPhone, int code)
        {
            int? storedCode = HttpContext.Session.GetInt32(cellPhone);
            if(storedCode == null)
            {
                return View("Confirmation",
                    new ConfirmationModel
                    {
                        orderId = id,
                        CellPhone = cellPhone,
                        Errors = new Dictionary<string, string>
                        {
                            {"code", "Код не может быть пустым." }
                        }
                    });
            }

            if(storedCode != code )
            {
                return View("Confirmation",
                    new ConfirmationModel
                    {
                        orderId = id,
                        CellPhone = cellPhone,
                        Errors = new Dictionary<string, string>
                        {
                            {"code", "Отличается от отправленого" }
                        }
                    });
            }

            return View("Empty");
        }
    }
}
