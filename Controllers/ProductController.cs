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
            var farmers = await farmerRepo.GetAllAsync();
            var viewModel = new ProductFilterViewModel
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
            var farmers = await farmerRepo.GetAllAsync();
            var products = await productRepo.GetFilteredProductsAsync(
                filter.FarmerId,
                filter.Category,
                filter.StartDate,
                filter.EndDate);

            filter.Farmers = farmers;
            filter.Products = products;
            return View("Filter", filter);
        }

        // Farmer dashboard - shows only current farmer's products
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Dashboard()
        {
            var farmer = await GetCurrentFarmerAsync();
            if (farmer == null) return Forbid();

            var products = await productRepo.GetFilteredProductsAsync(
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
            var products = await productRepo.GetAllAsync();
            return View(products);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create()
        {
            var farmer = await GetCurrentFarmerAsync();
            if (farmer == null) return Forbid();

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
            var farmer = await GetCurrentFarmerAsync();
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
            var product = await productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            // Ownership validation
            var farmer = await GetCurrentFarmerAsync();
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
            if (id != product.ProductID) return NotFound();

            // Ownership validation
            var farmer = await GetCurrentFarmerAsync();
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
            var product = await productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            // Ownership validation
            var farmer = await GetCurrentFarmerAsync();
            if (farmer == null || product.FarmerID != farmer.FarmerID)
            {
                return Forbid();
            }

            return View(product);
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            // Ownership validation
            var farmer = await GetCurrentFarmerAsync();
            if (farmer == null || product.FarmerID != farmer.FarmerID)
            {
                return Forbid();
            }

            await productRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Dashboard));
        }

        private async Task<Farmer?> GetCurrentFarmerAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail)) return null;

            var farmers = await farmerRepo.FindAsync(f => f.UserEmail == userEmail);
            return farmers.FirstOrDefault();
        }
    }
}