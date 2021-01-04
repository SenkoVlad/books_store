using com.google.i18n.phonenumbers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        protected ISession Session => httpContextAccessor.HttpContext.Session;
        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.getInstance();

        public OrderService(IBookRepository bookRepository, 
                            IOrderRepository orderRepository, 
                            INotificationService notificationService, 
                            IHttpContextAccessor httpContextAccessor)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool TryGetModel(out OrderModel orderModel)
        {
            if(TryGetOrder(out Order order))
            {
                orderModel = Map(order);
                return true;
            }
            orderModel = null; 
            return false;
        }

        private OrderModel Map(Order order)
        {
            var books = GetBooks(order);
            var items = from item in order.Items
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
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description
            };
        }

        internal IEnumerable<Book> GetBooks(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            return bookRepository.GetAllByIds(bookIds);
        }

        internal bool TryGetOrder(out Order order)
        {
            if(Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
                return true;
            }
                
            order = null;
            return false;
        }

        public OrderModel AddBook(int bookId, int count)
        {
            if (count < 1)
                throw new InvalidProgramException("Add few books");

            if (!TryGetOrder(out Order order))
                order = orderRepository.Create();

            AddOrUpdateBook(order, bookId, count);
            UpdateSession(order);

            return Map(order); 
        }
        private void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }
        internal void AddOrUpdateBook(Order order, int bookId, int count)
        {
            var book = bookRepository.GetById(bookId);
            if (order.Items.TryGet(bookId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(bookId, count, book.Price);

            orderRepository.Update(order);
        }
        public OrderModel UpdateBook(int bookId, int count)
        {
            var order = GetOrder();
            order.Items.Get(bookId).Count = count;

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }
        public OrderModel RemoveBook(int bookId)
        {
            var order = GetOrder();
            order.Items.Remove(bookId);

            orderRepository.Update(order);
            UpdateSession(order);
                
            return Map(order);
        }

        public Order GetOrder()
        {
            if(TryGetOrder(out Order order))
                return order;

            throw new InvalidOperationException("Empty session");
        }

        public OrderModel SendConfirmation(string cellPhone)
        {
            var order = GetOrder();
            var model = Map(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111;
                model.CellPhone = cellPhone;
                Session.SetInt32(cellPhone, confirmationCode);
                notificationService.SendConfirmationCode(formattedPhone, confirmationCode);
            }
            else
                model.Errors["cellPhone"] = "Пустой или не соответствует формату +XXXXXXXXXXX";

            return model;
        }

        internal bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.parse(cellPhone, "ru");
                formattedPhone = phoneNumberUtil.format(phoneNumber, PhoneNumberUtil.PhoneNumberFormat.INTERNATIONAL);
                return true;
            }
            catch (Exception)
            {
                formattedPhone = null;
                return false;
            }
        }

        public OrderModel ConfirmCellPhone(string cellPhone, int confirmCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();

            if(storedCode == null)
            {
                model.Errors["conformationCode"] = "Что-то пошло не так. Попробуйсте отправить код повторно";
                return model;
            }
            if(storedCode != confirmCode)
            {
                model.Errors["conformationCode"] = "";
                return model;
            }
            var order = GetOrder();
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            Session.Remove(cellPhone);

            return Map(order);
        }

        public OrderModel SetDelivery(OrderDelivery delivery)
        {
            var order = GetOrder();
            order.Delivery = delivery;
            orderRepository.Update(order);

            return Map(order);
        }

        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment = payment;
            orderRepository.Update(order);

            return Map(order);
        }
    }
}
