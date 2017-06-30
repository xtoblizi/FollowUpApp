using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FollowUpWebApp.Models;

namespace FollowUpWebApp.Controllers
{
    public class ChurchDatasController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: ChurchDatas
        public async Task<ActionResult> Index()
        {
            return View(await _db.ChurchDatas.ToListAsync());
        }

        // GET: ChurchDatas/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChurchData churchData = await _db.ChurchDatas.FindAsync(id);
            if (churchData == null)
            {
                return HttpNotFound();
            }
            return View(churchData);
        }

        // GET: ChurchDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChurchDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ChurchDataId,ParishName,ChurchStreetNo,ChurchStreetName,ChurchTown,ChurchState,ChurchPostalCode")] ChurchData churchData)
        {
            if (ModelState.IsValid)
            {
                _db.ChurchDatas.Add(churchData);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(churchData);
        }

        // GET: ChurchDatas/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChurchData churchData = await _db.ChurchDatas.FindAsync(id);
            if (churchData == null)
            {
                return HttpNotFound();
            }
            return View(churchData);
        }

        // POST: ChurchDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ChurchDataId,ParishName,ChurchStreetNo,ChurchStreetName,ChurchTown,ChurchState,ChurchPostalCode")] ChurchData churchData)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(churchData).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(churchData);
        }

        // GET: ChurchDatas/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChurchData churchData = await _db.ChurchDatas.FindAsync(id);
            if (churchData == null)
            {
                return HttpNotFound();
            }
            return View(churchData);
        }

        // POST: ChurchDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ChurchData churchData = await _db.ChurchDatas.FindAsync(id);
            _db.ChurchDatas.Remove(churchData);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
