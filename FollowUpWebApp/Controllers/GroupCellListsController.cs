using FollowUpWebApp.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FollowUpWebApp.Controllers
{
    public class GroupCellListsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: GroupCellLists
        public async Task<ActionResult> Index()
        {
            var groupCellLists = _db.GroupCellLists.Include(g => g.Department).Include(g => g.Member);
            return View(await groupCellLists.ToListAsync());
        }

        // GET: GroupCellLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupCellList groupCellList = await _db.GroupCellLists.FindAsync(id);
            if (groupCellList == null)
            {
                return HttpNotFound();
            }
            return View(groupCellList);
        }

        // GET: GroupCellLists/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
            ViewBag.MemberId = new SelectList(_db.Members, "MemberId", "ChurchDataId");
            return View();
        }

        // POST: GroupCellLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GroupCellListId,MemberId,DepartmentId")] GroupCellList groupCellList)
        {
            if (ModelState.IsValid)
            {
                _db.GroupCellLists.Add(groupCellList);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName", groupCellList.DepartmentId);
            ViewBag.MemberId = new SelectList(_db.Members, "MemberId", "ChurchDataId", groupCellList.MemberId);
            return View(groupCellList);
        }

        // GET: GroupCellLists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupCellList groupCellList = await _db.GroupCellLists.FindAsync(id);
            if (groupCellList == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName", groupCellList.DepartmentId);
            ViewBag.MemberId = new SelectList(_db.Members, "MemberId", "ChurchDataId", groupCellList.MemberId);
            return View(groupCellList);
        }

        // POST: GroupCellLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GroupCellListId,MemberId,DepartmentId")] GroupCellList groupCellList)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(groupCellList).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName", groupCellList.DepartmentId);
            ViewBag.MemberId = new SelectList(_db.Members, "MemberId", "ChurchDataId", groupCellList.MemberId);
            return View(groupCellList);
        }

        // GET: GroupCellLists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupCellList groupCellList = await _db.GroupCellLists.FindAsync(id);
            if (groupCellList == null)
            {
                return HttpNotFound();
            }
            return View(groupCellList);
        }

        // POST: GroupCellLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GroupCellList groupCellList = await _db.GroupCellLists.FindAsync(id);
            _db.GroupCellLists.Remove(groupCellList);
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
