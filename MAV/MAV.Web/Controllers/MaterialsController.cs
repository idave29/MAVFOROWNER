using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MAV.Web.Data;
using MAV.Web.Data.Entities;
using MAV.Web.Data.Repositories;

namespace MAV.Web.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly DataContext _context;

        private readonly IMaterialRepository materialRepository;
        private readonly IStatusRepository statusRepository;

        public MaterialsController(IMaterialRepository materialRepository, IStatusRepository statusRepository)
        {
            this.statusRepository = statusRepository;
            this.materialRepository = materialRepository;
        }
        // GET: Materials
        public IActionResult Index()
        {
           
            return View(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwner());
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await this.materialRepository.GetByIdAsync(id.Value);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Label,Brand,MaterialModel,SerialNum")] Material material)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await this.materialRepository.CreateAsync(material);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(material);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Material material)
        {
            if (ModelState.IsValid)
            {
                await this.materialRepository.CreateAsync(material);
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Label,Brand,MaterialModel,SerialNum")] Material material)
        //{
        //    if (id != material.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await this.materialRepository.UpdateAsync(material);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await this.materialRepository.ExistAsync(material.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(material);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.materialRepository.UpdateAsync(material);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.materialRepository.ExistAsync(material.Id))
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
            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await this.materialRepository.GetByIdAsync(id.Value);
            if (material == null)
            {
                return NotFound();
            }
            await this.materialRepository.DeleteAsync(material);
            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}
