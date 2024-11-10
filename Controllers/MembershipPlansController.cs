using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymMemberships.Data;
using GymMemberships.Models;
using Microsoft.AspNetCore.Authorization;

namespace GymMemberships.Controllers
{
    public class MembershipPlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MembershipPlans
        public async Task<IActionResult> Index()
        {
            return View(await _context.MembershipPlans.ToListAsync());
        }

        // GET: MembershipPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (membershipPlan == null)
            {
                return NotFound();
            }

            return View(membershipPlan);
        }

        // GET: MembershipPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MembershipPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanName,Price,Description")] MembershipPlan membershipPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membershipPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membershipPlan);
        }

        // GET: MembershipPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipPlan = await _context.MembershipPlans.FindAsync(id);
            if (membershipPlan == null)
            {
                return NotFound();
            }
            return View(membershipPlan);
        }

        // POST: MembershipPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlanName,Price,Description")] MembershipPlan membershipPlan)
        {
            if (id != membershipPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membershipPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipPlanExists(membershipPlan.Id))
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
            return View(membershipPlan);
        }

        // GET: MembershipPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (membershipPlan == null)
            {
                return NotFound();
            }

            return View(membershipPlan);
        }

        // POST: MembershipPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membershipPlan = await _context.MembershipPlans.FindAsync(id);
            if (membershipPlan != null)
            {
                _context.MembershipPlans.Remove(membershipPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipPlanExists(int id)
        {
            return _context.MembershipPlans.Any(e => e.Id == id);
        }
    }
}
