using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PittigRestoMVC.Data;
using PittigRestoMVC.Models;

namespace PittigRestoMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CouponsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Coupons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coupon.ToListAsync());
        }

        // GET: Admin/Coupons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // GET: Admin/Coupons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Coupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CouponType,Discount,MinimumAmount,Picture,IsActive")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coupon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        // GET: Admin/Coupons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        // POST: Admin/Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CouponType,Discount,MinimumAmount,Picture,IsActive")] Coupon coupon)
        {
            if (id != coupon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coupon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CouponExists(coupon.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        // GET: Admin/Coupons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // POST: Admin/Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupon = await _context.Coupon.FindAsync(id);
            _context.Coupon.Remove(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CouponExists(int id)
        {
            return _context.Coupon.Any(e => e.Id == id);
        }
    }
}
