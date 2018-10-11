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
    public class PipelineEDISettingsController : Controller
    {
        private NomEntities db = new NomEntities();

        // GET: PipelineEDISettings
        public ActionResult Index()
        {
            return View(db.PipelineEDISetting.ToList());
        }

        // GET: PipelineEDISettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipelineEDISetting pipelineEDISetting = db.PipelineEDISetting.Find(id);
            if (pipelineEDISetting == null)
            {
                return HttpNotFound();
            }
            return View(pipelineEDISetting);
        }

        // GET: PipelineEDISettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PipelineEDISettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,PipeDuns,ISA08_segment,ISA06_Segment,ISA11_Segment,ISA12_Segment,ISA16_Segment,GS01_Segment,GS02_Segment,GS03_Segment,GS07_Segment,GS08_Segment,ST01_Segment,DataSeparator,SegmentSeperator,DatasetId,ShipperCompDuns,StartDate,EndDate,SendManually,ForOacy,ForUnsc,ForSwnt")] PipelineEDISetting pipelineEDISetting)
        {
            if (ModelState.IsValid)
            {
                db.PipelineEDISetting.Add(pipelineEDISetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pipelineEDISetting);
        }

        // GET: PipelineEDISettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipelineEDISetting pipelineEDISetting = db.PipelineEDISetting.Find(id);
            if (pipelineEDISetting == null)
            {
                return HttpNotFound();
            }
            return View(pipelineEDISetting);
        }

        // POST: PipelineEDISettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,PipeDuns,ISA08_segment,ISA06_Segment,ISA11_Segment,ISA12_Segment,ISA16_Segment,GS01_Segment,GS02_Segment,GS03_Segment,GS07_Segment,GS08_Segment,ST01_Segment,DataSeparator,SegmentSeperator,DatasetId,ShipperCompDuns,StartDate,EndDate,SendManually,ForOacy,ForUnsc,ForSwnt")] PipelineEDISetting pipelineEDISetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pipelineEDISetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pipelineEDISetting);
        }

        // GET: PipelineEDISettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipelineEDISetting pipelineEDISetting = db.PipelineEDISetting.Find(id);
            if (pipelineEDISetting == null)
            {
                return HttpNotFound();
            }
            return View(pipelineEDISetting);
        }

        // POST: PipelineEDISettings/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PipelineEDISetting pipelineEDISetting = db.PipelineEDISetting.Find(id);
            db.PipelineEDISetting.Remove(pipelineEDISetting);
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
