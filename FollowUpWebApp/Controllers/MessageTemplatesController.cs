using FollowUpWebApp.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FollowUpWebApp.Controllers
{
    public class MessageTemplatesController : Controller
    {
        private readonly ApplicationDbContext db;

        public MessageTemplatesController()
        {
            db = new ApplicationDbContext();
        }

        // GET: MessageTemplates
        public async Task<ActionResult> Index()
        {
            return View(await db.MessageTemplates.ToListAsync());
        }

        // GET: MessageTemplates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageTemplate messageTemplate = await db.MessageTemplates.FindAsync(id);
            if (messageTemplate == null)
            {
                return HttpNotFound();
            }
            return View(messageTemplate);
        }

        // GET: MessageTemplates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MessageTemplateId,MessagingType,MessageBody")] MessageTemplate messageTemplate)
        {
            if (ModelState.IsValid)
            {
                db.MessageTemplates.Add(messageTemplate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(messageTemplate);
        }

        // GET: MessageTemplates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageTemplate messageTemplate = await db.MessageTemplates.FindAsync(id);
            if (messageTemplate == null)
            {
                return HttpNotFound();
            }
            return View(messageTemplate);
        }

        // POST: MessageTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MessageTemplateId,MessagingType,MessageBody")] MessageTemplate messageTemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageTemplate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(messageTemplate);
        }

        // GET: MessageTemplates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageTemplate messageTemplate = await db.MessageTemplates.FindAsync(id);
            if (messageTemplate == null)
            {
                return HttpNotFound();
            }
            return View(messageTemplate);
        }

        // POST: MessageTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MessageTemplate messageTemplate = await db.MessageTemplates.FindAsync(id);
            if (messageTemplate != null) db.MessageTemplates.Remove(messageTemplate);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
