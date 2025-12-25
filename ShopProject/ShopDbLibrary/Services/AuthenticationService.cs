using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopDbLibrary.Contexts;
using ShopDbLibrary.Models;
using ShopDbLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopDbLibrary.Services
{
    public class AuthenticationService(ShopContext context)
    {
        private readonly ShopContext _context = context;
        public static readonly string secretKey = "12345678123456781234567812345678";

        public string GenerateToken(User user) //Генерация JWT токена
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Создаём ключ безопасности
            var authority = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // Создаём учётные данные для подписи токена

            var claims = new Claim[] // Формируем набор утверждений
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),  //ID 
                new(ClaimTypes.Name, user.Login),  //Логин
                new(ClaimTypes.Role, user.Role.Name)  //Пароль
            };

            var token = new JwtSecurityToken(  //Создаем JWT токен
                signingCredentials: authority,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15) //15 минут действует токен
                );

            return new JwtSecurityTokenHandler().WriteToken(token);  //преобразуем токен в строку для передачи
        }

        public string LoginUser(string login, string password) //Аутентификацмя пользователя
        {
            var user = _context.Users  //Поиск пользователя в БД по логину и паролю
                .Include(u => u.Role)  //Загружаем роль
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null) //Проверяем найден ли пользователь
                return null;

            return GenerateToken(user);  //Генерируем JWT токен для данного пользователя
        }
    }
}
