using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers {
    public class CategoryController : Controller {

        private readonly ICategoryRepository _db;

        public CategoryController(ICategoryRepository db) {
            _db = db;
        }

        public IActionResult Index() {
            IEnumerable<Category> objCategoryList = _db.GetAll();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create() {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj) {
            if(obj.Name == obj.DisplayOrder.ToString()) {
                ModelState.AddModelError("NameEqualToDisplayOrder", "The Display Order cannot exactly match the Name.");
            }
            if(ModelState.IsValid) {
                _db.Add(obj);
                _db.Save();
                TempData["success"] = "Category created sucessfully";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id) {
            if(id == null || id == 0) {
                return NotFound();
            }
            
            var categoryFromDbFirst = _db.GetFirstOrDefault(u=>u.Id==id);

            if(categoryFromDbFirst == null) {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }

        //PUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj) {
            if (obj.Name == obj.DisplayOrder.ToString()) {
                ModelState.AddModelError("NameEqualToDisplayOrder", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid) {
                _db.Update(obj);
                _db.Save();
                TempData["success"] = "Category updated sucessfully";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        //GET
        public IActionResult Delete(int? id) {
            if (id == null || id == 0) {
                return NotFound();
            }

            var categoryFromDbFirst = _db.GetFirstOrDefault(u=>u.Id==id);

            if (categoryFromDbFirst == null) {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }

        //DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST (int? id) {
            var categoryFromDbFirst = _db.GetFirstOrDefault(u => u.Id == id);
            if (categoryFromDbFirst == null) {
                return NotFound();
            }
            
            _db.Remove(categoryFromDbFirst);
            _db.Save();
            TempData["success"] = "Category deleted sucessfully";
            return RedirectToAction("Index");
        }
    }
}
