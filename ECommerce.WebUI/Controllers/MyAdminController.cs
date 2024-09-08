using ECommerce.Business.Abstract;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    public class MyAdminController : Controller
    {
        private readonly IProductService _productService;

        public MyAdminController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ActionResult> Index(int page = 1, int category = 0)
        {
            int pageSize = 10;
            var items = await _productService.GetAllByCategoryAsync(category);

            var model = new MyAdminListViewModel
            {
                Products = items.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                CurrentCategory = category,
                PageCount = (int)Math.Ceiling(items.Count / (double)pageSize),
                PageSize = pageSize,
                CurrentPage = page
            };
            return View(model);
        }
        public async Task<ActionResult> Remove(int productId)
        {
            await _productService.DeleteAsync(productId);
            TempData.Add("message", "Your product removed successfully from products");
            return RedirectToAction("Index");
        }
    }
}
