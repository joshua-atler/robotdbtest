using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Text;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize(Roles = "Team")]
    public class InventoryController : Controller
    {
        private readonly MyDatabaseContext _context;

        public InventoryController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: Inventory
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventory.ToListAsync());
        }

        // GET: Inventory/Details/5
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventory/Create
        [Authorize(Roles = "Business")]
        public IActionResult Create()
        {
            Console.WriteLine("Inventory create");

            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Business")]
        public async Task<IActionResult> Create([Bind("ID,PartName,PartType,SKU,UnitCost,Quantity,Location,Status")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                if(HttpContext.User.IsInRole("Admin"))
                {
                    inventory.Status = Inventory.InventoryStatus.Approved;
                } else
                {
                    inventory.Status = Inventory.InventoryStatus.Submitted;
                }
                
                Console.WriteLine("status");
                Console.WriteLine(inventory.Status);


                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        public IActionResult SelectEdit(string partName)
        {
            Console.WriteLine("Select edit");

            Console.WriteLine(partName);

            int partID = 0;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = "robotdbtestserver.database.windows.net";
                builder.UserID = "jatler";
                builder.Password = "G1raffe$";
                builder.InitialCatalog = "coredb";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    String sql = String.Format("SELECT id FROM [dbo].[Inventory] WHERE PartName = '{0}'", partName);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("reading");
                                partID = reader.GetInt32(0);
                                Console.WriteLine(partID);
                            }
                        }
                    }
                }
            } catch (SqlException e)
            {
                Console.WriteLine("SQL Exception");
                Console.WriteLine(e.ToString()); 
            }

            Console.WriteLine("done");

            var redirect = Url.Action("Edit", "Inventory", new { id = partID });

            return Json(new
            {
                redirectUrl = redirect
            });

            /*return RedirectToAction("Edit", "Inventory", new { id = partID });*/
        }

        // GET: Inventory/Edit/5
        [Authorize(Roles = "Business")]
        public async Task<IActionResult> Edit(int? id)
        {

            Console.WriteLine("edit!");

            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.FindAsync(id);
            ViewData["status"] = inventory.Status;

            if (!HttpContext.User.IsInRole("Admin") && inventory.Status == Inventory.InventoryStatus.Rejected)
            {
                return RedirectToAction("AccessDenied", "Login");
            }

            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Business")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PartName,PartType,SKU,UnitCost,Quantity,Location,Status")] Inventory inventory)
        {
            Console.WriteLine("Inventory edit");

            if (id != inventory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.ID))
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
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Console.WriteLine("Inventory delete");
            Console.WriteLine(id);

            if (id == null)
            {
                Console.WriteLine("not found 1");
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                Console.WriteLine("not found 2");
                return NotFound();
            }

            Console.WriteLine("found");
            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            _context.Inventory.Remove(inventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.ID == id);
        }
    }
}
