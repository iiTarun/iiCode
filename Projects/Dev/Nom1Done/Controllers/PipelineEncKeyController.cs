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
    public class PipelineEncKeyController : Controller
    {
        private NomEntities db = new NomEntities();

        // GET: PipelineEncKey
        public ActionResult Index()
        {
            return View(db.metadataPipelineEncKeyInfo.ToList());
        }

        // GET: PipelineEncKey/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            metadataPipelineEncKeyInfo metadataPipelineEncKeyInfo = db.metadataPipelineEncKeyInfo.Find(id);
            if (metadataPipelineEncKeyInfo == null)
            {
                return HttpNotFound();
            }
            return View(metadataPipelineEncKeyInfo);
        }

        // GET: PipelineEncKey/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PipelineEncKey/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PipelineId,KeyName,PipeDuns")] metadataPipelineEncKeyInfo metadataPipelineEncKeyInfo)
        {
            if (ModelState.IsValid)
            {
                db.metadataPipelineEncKeyInfo.Add(metadataPipelineEncKeyInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(metadataPipelineEncKeyInfo);
        }

        // GET: PipelineEncKey/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            metadataPipelineEncKeyInfo metadataPipelineEncKeyInfo = db.metadataPipelineEncKeyInfo.Find(id);
            if (metadataPipelineEncKeyInfo == null)
            {
                return HttpNotFound();
            }
            return View(metadataPipelineEncKeyInfo);
        }

        // POST: PipelineEncKey/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PipelineId,KeyName,PipeDuns")] metadataPipelineEncKeyInfo metadataPipelineEncKeyInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metadataPipelineEncKeyInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(metadataPipelineEncKeyInfo);
        }

        // GET: PipelineEncKey/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            metadataPipelineEncKeyInfo metadataPipelineEncKeyInfo = db.metadataPipelineEncKeyInfo.Find(id);
            if (metadataPipelineEncKeyInfo == null)
            {
                return HttpNotFound();
            }
            return View(metadataPipelineEncKeyInfo);
        }

        // POST: PipelineEncKey/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            metadataPipelineEncKeyInfo metadataPipelineEncKeyInfo = db.metadataPipelineEncKeyInfo.Find(id);
            db.metadataPipelineEncKeyInfo.Remove(metadataPipelineEncKeyInfo);
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
