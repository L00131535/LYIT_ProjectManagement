using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using crowsoftmvc.Data;
using crowsoftmvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace crowsoftmvc.Controllers
{
    [Authorize]
    public class BuildingImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider fileProvider;

        public BuildingImagesController(ApplicationDbContext context, IFileProvider fileProvider)
        {
            _context = context;
            this.fileProvider = fileProvider;
        }

        // GET: BuildingImages
        public async Task<IActionResult> Index(int idBuildingQoute)
        {
            ViewData["BuildingQuoteId"] = idBuildingQoute;
            return View(await _context.BuildingImage.Where(b => b.BuildingQuoteIdBuildingQuote == idBuildingQoute).ToListAsync());
        }

        // GET: BuildingImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingImage = await _context.BuildingImage
                .FirstOrDefaultAsync(m => m.IdBuildingImage == id);
            if (buildingImage == null)
            {
                return NotFound();
            }

            return View(buildingImage);
        }

        // GET: BuildingImages/Create
        public IActionResult Create(int idBuildingQoute)
        {
            BuildingImage buildingImage = new BuildingImage();
            buildingImage.BuildingQuoteIdBuildingQuote = idBuildingQoute;
            //buildingImage.FileToUpload ; 
            return View(buildingImage);
        }

        // POST: BuildingImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBuildingImage,Description,FileToUpload,ImagePath,BuildingQuoteIdBuildingQuote")] BuildingImage buildingImage, IFormFile FileToUpload)
        {
            if (ModelState.IsValid)
            {
                if (FileToUpload == null || FileToUpload.Length == 0)
                    return Content("file not selected");

                var path = await UploadFile(FileToUpload);
                if (path != "")
                {
                    buildingImage.ImagePath = path;
                    _context.Add(buildingImage);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
                
            }
            return View(buildingImage);
        }

        // GET: BuildingImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingImage = await _context.BuildingImage.FindAsync(id);
            if (buildingImage == null)
            {
                return NotFound();
            }
            return View(buildingImage);
        }

        // POST: BuildingImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBuildingImage,Description,FileToUpload,ImagePath,BuildingQuoteIdBuildingQuote")] BuildingImage buildingImage)
        {
            if (id != buildingImage.IdBuildingImage)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buildingImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingImageExists(buildingImage.IdBuildingImage))
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
            return View(buildingImage);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "";

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "ImageFiles",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return path;
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "/ImageFiles", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats/officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        // GET: BuildingImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingImage = await _context.BuildingImage
                .FirstOrDefaultAsync(m => m.IdBuildingImage == id);
            if (buildingImage == null)
            {
                return NotFound();
            }

            return View(buildingImage);
        }

        // POST: BuildingImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buildingImage = await _context.BuildingImage.FindAsync(id);
            _context.BuildingImage.Remove(buildingImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingImageExists(int id)
        {
            return _context.BuildingImage.Any(e => e.IdBuildingImage == id);
        }
    }
}
