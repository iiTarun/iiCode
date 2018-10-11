using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nom1Done.Data;
using Nom1Done.Model;

namespace Nom1Done.Controllers
{
    public class TradingPartnerWorksheetsController : Controller
    {
        private NomEntities db = new NomEntities();

        // GET: TradingPartnerWorksheets
        public ActionResult Index()
        {
            return View(db.TradingPartnerWorksheet.ToList());
        }

        // GET: TradingPartnerWorksheets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TradingPartnerWorksheet tradingPartnerWorksheet = db.TradingPartnerWorksheet.Find(id);
            if (tradingPartnerWorksheet == null)
            {
                return HttpNotFound();
            }
            return View(tradingPartnerWorksheet);
        }

        // GET: TradingPartnerWorksheets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TradingPartnerWorksheets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,PipelineID,UsernameLive,PasswordLive,URLLive,KeyLive,UsernameTest,PasswordTest,URLTest,KeyTest,ReceiveSubSeperator,ReceiveDataSeperator,ReceiveSegmentSeperator,SendSubSeperator,SendDataSeperator,SendSegmentSeperator,IsTest,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,PipeDuns")] TradingPartnerWorksheet tradingPartnerWorksheet)
        {
            if (ModelState.IsValid)
            {
                db.TradingPartnerWorksheet.Add(tradingPartnerWorksheet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tradingPartnerWorksheet);
        }

        // GET: TradingPartnerWorksheets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TradingPartnerWorksheet tradingPartnerWorksheet = db.TradingPartnerWorksheet.Find(id);
            if (tradingPartnerWorksheet == null)
            {
                return HttpNotFound();
            }
            return View(tradingPartnerWorksheet);
        }

        // POST: TradingPartnerWorksheets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,PipelineID,UsernameLive,PasswordLive,URLLive,KeyLive,UsernameTest,PasswordTest,URLTest,KeyTest,ReceiveSubSeperator,ReceiveDataSeperator,ReceiveSegmentSeperator,SendSubSeperator,SendDataSeperator,SendSegmentSeperator,IsTest,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,PipeDuns")] TradingPartnerWorksheet tradingPartnerWorksheet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tradingPartnerWorksheet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tradingPartnerWorksheet);
        }

        // GET: TradingPartnerWorksheets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TradingPartnerWorksheet tradingPartnerWorksheet = db.TradingPartnerWorksheet.Find(id);
            if (tradingPartnerWorksheet == null)
            {
                return HttpNotFound();
            }
            return View(tradingPartnerWorksheet);
        }

        // POST: TradingPartnerWorksheets/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TradingPartnerWorksheet tradingPartnerWorksheet = db.TradingPartnerWorksheet.Find(id);
            db.TradingPartnerWorksheet.Remove(tradingPartnerWorksheet);
            db.SaveChanges();
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
