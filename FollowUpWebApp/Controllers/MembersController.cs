using FollowUpWebApp.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FollowUpWebApp.Controllers
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MembersController()
        {
            _db = new ApplicationDbContext();
        }

        // GET: Members
        public async Task<ActionResult> Index()
        {
            var members = _db.Members.Include(m => m.ChurchData);
            return View(await members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = await _db.Members.FindAsync(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.ChurchDataId = new SelectList(_db.ChurchDatas, "ChurchDataId", "ParishName");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MemberId,ChurchDataId,Prefix,FirstName,MiddleName,LastName,Gender,DateOfBirth,MaritalStatus,MaritalDate,Email,PhoneNumber,AddressHouseNo,AddressStreetName,AddressTown,AddressState,AddressPostalCode,TownOfBirth,StateOfOrigin,Tribe,Nationality,CountryOfBirth,IsANewMember,Age,Passport")] Member member)
        {
            if (ModelState.IsValid)
            {
                _db.Members.Add(member);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ChurchDataId = new SelectList(_db.ChurchDatas, "ChurchDataId", "ParishName", member.ChurchDataId);
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = await _db.Members.FindAsync(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChurchDataId = new SelectList(_db.ChurchDatas, "ChurchDataId", "ParishName", member.ChurchDataId);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MemberId,ChurchDataId,Prefix,FirstName,MiddleName,LastName,Gender,DateOfBirth,MaritalStatus,MaritalDate,Email,PhoneNumber,AddressHouseNo,AddressStreetName,AddressTown,AddressState,AddressPostalCode,TownOfBirth,StateOfOrigin,Tribe,Nationality,CountryOfBirth,IsANewMember,Age,Passport")] Member member)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(member).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ChurchDataId = new SelectList(_db.ChurchDatas, "ChurchDataId", "ParishName", member.ChurchDataId);
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = await _db.Members.FindAsync(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Member member = await _db.Members.FindAsync(id);
            _db.Members.Remove(member);
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
