using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriEnergyConnect.Controllers
{
    [Authorize]
    public class ProductController(IProductRepository productRepo, IFarmerRepository farmerRepo) : Controller
    {

        // Employee-only features
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Filter()
        {
            IEnumerable<Farmer> farmers = await farmerRepo.GetAllAsync();
            ProductFilterViewModel viewModel = new()
            {
                Farmers = farmers,
                Products = null
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> FilterResults(ProductFilterViewModel filter)
        {
            if (!ModelState.IsValid)
            {
                filter.Farmers = await farmerRepo.GetAllAsync();
                filter.Products = null;
                return View("Filter", filter);
            }

            IEnumerable<Farmer> farmers = await farmerRepo.GetAllAsync();
            IEnumerable<Product> products = await productRepo.GetFilteredProductsAsync(
                filter.FarmerId,
                filter.Category,
                filter.StartDate,
                filter.EndDate);

            filter.Farmers = farmers;
            filter.Products = products;
            return View("Filter", filter);
        }
        // Ensure ModelState.IsValid is checked in all POST actions (already done above)
        // Farmer dashboard - shows only current farmer's products
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Dashboard()
        {
            Farmer? farmer = await GetCurrentFarmerAsync();
            if (farmer == null)
            {
                return Forbid();
            }

            IEnumerable<Product> products = await productRepo.GetFilteredProductsAsync(
                farmerId: farmer.FarmerID,
                category: null,
                startDate: null,
                endDate: null);

            ViewBag.FarmerName = farmer.Name;
            return View(products);
        }

        // Shows all products (for employees)
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await productRepo.GetAllAsync();
            return View(products);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create()
        {
            Farmer? farmer = await GetCurrentFarmerAsync();
            if (farmer == null)
            {
                return Forbid();
            }

            ViewBag.FarmerId = farmer.FarmerID;
            ViewBag.FarmerName = farmer.Name;
            return View();
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Category,ProductionDate,Description,FarmerID")] Product product)
        {
            // Ownership validation
            Farmer? farmer = await GetCurrentFarmerAsync();
            if (farmer == null || product.FarmerID != farmer.FarmerID)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                await productRepo.AddAsync(product);
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.FarmerId = farmer.FarmerID;
            ViewBag.FarmerName = farmer.Name;
            return View(product);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product? product = await productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Ownership validation
            Farmer? farmer = await GetCurrentFarmerAsync();
            if (farmer == null || product.FarmerID != farmer.FarmerID)
            {
                return Forbid();
            }

            ViewBag.FarmerName = farmer.Name;
            return View(product);
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Category,ProductionDate,Description,FarmerID")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            // Ownership validation
            Farmer? farmer = await GetCurrentFarmerAsync();
            if (farmer == null || product.FarmerID != farmer.FarmerID)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                await productRepo.UpdateAsync(product);
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.FarmerName = farmer.Name;
            return View(product);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product? product = await productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Ownership validation
            Farmer? farmer = await GetCurrentFarmerAsync();
            return farmer == null || product.FarmerID != farmer.FarmerID ? Forbid() : View(product);
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product? product = await productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Ownership validation
            Farmer? farmer = await GetCurrentFarmerAsync();
            if (farmer == null || product.FarmerID != farmer.FarmerID)
            {
                return Forbid();
            }

            await productRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Dashboard));
        }

        private async Task<Farmer?> GetCurrentFarmerAsync()
        {
            string? userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return null;
            }

            IEnumerable<Farmer> farmers = await farmerRepo.FindAsync(f => f.UserEmail == userEmail);
            return farmers.FirstOrDefault();
        }
    }
}