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
    
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members.ToListAsync();

            foreach (var member in members)
            {
                // Fetch the MembershipPlan using MembershipPlanId
                var membershipPlan = await _context.MembershipPlans
                                                   .FirstOrDefaultAsync(mp => mp.Id == member.MembershipPlanId);

                // Store the MembershipPlan name in ViewData with a key for each member
                ViewData[$"MembershipPlanName_{member.Id}"] = membershipPlan?.PlanName ?? "N/A";
            }

            return View(members);
        }

        //print report
        public async Task<IActionResult> Report()
        {
            var members = await _context.Members.ToListAsync();

            // Retrieve and store the MembershipPlanName for each member in ViewData
            foreach (var member in members)
            {
                var membershipPlan = await _context.MembershipPlans
                    .FirstOrDefaultAsync(mp => mp.Id == member.MembershipPlanId);

                // Use the member's Id as the key to store the plan name in ViewData
                ViewData[$"MembershipPlanName_{member.Id}"] = membershipPlan?.PlanName;
            }

            return View(members);
        }


        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            // Get the MembershipPlan by MembershipPlanId
            var membershipPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(mp => mp.Id == member.MembershipPlanId);

            // Pass the PlanName to the view
            ViewData["MembershipPlanName"] = membershipPlan?.PlanName;

            return View(member);
        }


        // GET: Member/Create
        public IActionResult Create()
        {
            ViewData["MembershipPlanId"] = new SelectList(_context.MembershipPlans, "Id", "PlanName");
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index)); // or another appropriate action
            }
            // Re-populate the SelectList for the view in case of validation failure
            ViewData["MembershipPlanId"] = new SelectList(_context.MembershipPlans, "Id", "PlanName", member.MembershipPlanId);
            return View(member);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            // Populate MembershipPlanId dropdown
            ViewData["MembershipPlanId"] = new SelectList(_context.MembershipPlans, "Id", "PlanName", member.MembershipPlanId);

            return View(member);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,Age,MembershipPlanId,Address,StartDate,EndDate,Status")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the member details in the database
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); // or another appropriate action
            }

            // Repopulate MembershipPlanId dropdown in case of validation failure
            ViewData["MembershipPlanId"] = new SelectList(_context.MembershipPlans, "Id", "PlanName", member.MembershipPlanId);

            // You may want to handle the Status in the ViewData or another helper if you want to customize it more
            // For example, you can manually populate the status options in ViewData if needed
            return View(member);
        }

        // Renders the Delete confirmation view
        public IActionResult Delete(int id)
        {
            var member = _context.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }
            return PartialView(member);
        }

        // Handles the actual delete action
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var member = _context.Members.Find(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //API Postman Method only 

        // GET: api/Members
        [HttpGet("api/members")]
        [AllowAnonymous] // Optional: Allows unauthenticated access to this endpoint
        public async Task<IActionResult> GetAllMembers()
        {
            var members = await _context.Members
                .Select(member => new
                {
                    member.Id,
                    member.Name,
                    member.Email,
                    member.Phone,
                    member.Age,
                    MembershipPlanName = _context.MembershipPlans
                        .Where(mp => mp.Id == member.MembershipPlanId)
                        .Select(mp => mp.PlanName)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(members);
        }

        // POST: api/members
        [HttpPost("api/members")]
        public async Task<IActionResult> CreateMember([FromBody] Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMemberById), new { id = member.Id }, member);
        }

        // PUT: api/members/{id}
        [HttpPut("api/members/{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Member member)
        {
            if (id != member.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/members/{id}
        [HttpDelete("api/members/{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/members/{id}
        [HttpGet("api/members/{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }


        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
