using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopDbLibrary.Contexts;
using ShopDbLibrary.Models;
using ShopDbLibrary.Contexts;
using ShopDbLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopDbLibrary.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ShopContext _context;

        public OrdersController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/Orders/user/{login}
        [Authorize]
        [HttpGet("user/{login}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser(string login) //Получаем список заказов пользователя
        {
            var orders = await _context.Orders  //Загружаем данные из БД
                .Include(o => o.User)
                .Include(o => o.ShoeOrders)
                    .ThenInclude(so => so.Shoe)
                .Where(o => o.User.Login == login)
                .ToListAsync();

            return Ok(orders);  //200 Возварщаем заказы 
        }

        // PUT: api/Orders/{orderId}
        [Authorize(Roles = "admin,manager")]
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] Order updatedOrder)  //Обновляем заказ
        {
            var order = await _context.Orders.FindAsync(orderId);  //Ищем заказ по ID

            if (order is null)  //Проверяем заказ на наличие 
                return NotFound(); //404 Не найден

            if (!string.IsNullOrEmpty(updatedOrder.Status)&& updatedOrder.Status != order.Status)  //Обновляем статус заказ
                order.Status = updatedOrder.Status;

            if (updatedOrder.DeliveryDate != default && updatedOrder.DeliveryDate != order.DeliveryDate)  //Обновляем дату заказа
                order.DeliveryDate = updatedOrder.DeliveryDate;

            _context.Update(order); //Отмечаем как обновленную
            await _context.SaveChangesAsync(); //Сохраняем

            return NoContent(); //204 Успешно ничего не возвращаем
        }
    }
}
