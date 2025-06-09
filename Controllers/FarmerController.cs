using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmerController(IFarmerRepository farmerRepo) : Controller
    {
        public async Task<IActionResult> Index()
        {
            IEnumerable<Farmer> farmersWithProducts = await farmerRepo.GetFarmersWithProductsAsync();
            return View(farmersWithProducts);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            Farmer? farmer = await farmerRepo.GetByIdAsync(id);
            return farmer == null ? NotFound() : View(farmer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Location")] Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                await farmerRepo.AddAsync(farmer);
                return RedirectToAction(nameof(Index));
            }
            return View(farmer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Location")] Farmer farmer)
        {
            if (id != farmer.FarmerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await farmerRepo.UpdateAsync(farmer);
                return RedirectToAction(nameof(Index));
            }
            return View(farmer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Farmer? farmer = await farmerRepo.GetByIdAsync(id);
            return farmer == null ? NotFound() : View(farmer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await farmerRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}