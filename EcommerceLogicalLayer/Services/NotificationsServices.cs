using EcommerceDataLayer.AppDbContex;
using EcommerceDataLayer.Entities.Products;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Helpers;
using EcommerceLogicalLayer.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLogicalLayer.Services
{
    public class NotificationsServices(ApplicationDbContext applicationDbContext,
        IHttpContextAccessor httpContextAccessor, IEmailService emailService, IProductServices productServices ) : INotificationsServices
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailService _emailService = emailService;
        private readonly IProductServices _productServices = productServices;

        public async Task ProductNotifications()
        {
            var Products = await _productServices.GetAll();
            var Member = await (from u in _applicationDbContext.Users
                         join ur in _applicationDbContext.UserRoles on u.Id equals ur.UserId
                         join r in _applicationDbContext.Roles on ur.RoleId equals r.Id
                         where !r.Name!.Contains("admin")
                         select new { u.Email}).ToListAsync();
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            if (Products is null)
                return;
            foreach (var product  in Products.Value!)
            {
                foreach(var member in Member)
                {
                    var placeholders = new Dictionary<string, string>
                {
                    { "{{name}}", product.ProductName },
                    { "{{Description}}",product.Description },
                    { "{{Date}}", DateTime.UtcNow.ToString() },
                    { "{{url}}", $"{origin}/Products/start/{product.ProductID}" }
                };

                    var body = EmailBodyBuilder.GenerateEmailBody("ProductNotification", placeholders);

                    await _emailService.SendEmailAsync(member.Email!, $"📣 Ecommerce: New Product - {product.ProductName} Is Avalible", body);

                }
            }
        }
    }

}