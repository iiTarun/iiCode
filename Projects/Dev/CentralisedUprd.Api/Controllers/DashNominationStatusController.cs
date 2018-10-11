using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CentralisedUprd.Api.Models;

namespace CentralisedUprd.Api.Controllers
{
    public class DashNominationStatusController : ApiController
    {
        private UprdDbEntities1 db = new UprdDbEntities1();

        //// GET: api/DashNominationStatus
        //public IQueryable<DashNominationStatu> GetDashNominationStatus()
        //{
        //    return db.DashNominationStatus;
        //}

        //// GET: api/DashNominationStatus/5
        //[ResponseType(typeof(DashNominationStatu))]
        //public IHttpActionResult GetDashNominationStatu(int id)
        //{
        //    DashNominationStatu dashNominationStatu = db.DashNominationStatus.Find(id);
        //    if (dashNominationStatu == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(dashNominationStatu);
        //}

        //PUT: api/DashNominationStatus/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDashNominationStatu(Guid TransactionId, int statusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var obj = db.DashNominationStatus.Where(a => a.TransactionId == TransactionId).FirstOrDefault();
            if (obj==null)
            {
                return BadRequest();
            }
            obj.StatusId = statusId;
            switch (statusId)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    obj.NomStatus = "In-Process";
                    break;
                case 5:
                    obj.NomStatus = "Submitted";
                    break;
                case 7:
                    obj.NomStatus = "Accepted";
                    break;
                case 8:
                case 9:
                    obj.NomStatus = "Exception Occured";
                    break;
                case 10:
                    obj.NomStatus = "Rejected";
                    break;
                case 0:
                    obj.NomStatus = "GISB Unprocessed";
                    break;
                default:
                    obj.NomStatus = "Not Defined";
                    break;
            }
            db.Entry(obj).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DashNominationStatuExists(TransactionId))
                {
                    ApplicationLog log = new ApplicationLog();
                    log.CreatedDate = DateTime.Now;
                    log.Description = "Nomination not found TransactionId:- " + TransactionId;
                    log.Source = "Update";
                    log.Type = "Error";
                    db.ApplicationLogs.Add(log);
                    db.SaveChanges();
                    return NotFound();
                }
                else
                {
                    return Ok("fail");
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DashNominationStatus
        [ResponseType(typeof(DashNominationStatu))]
        public IHttpActionResult PostDashNominationStatu(DashNominationStatu dashNominationStatu)
        {
            string result = string.Empty;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.DashNominationStatus.Add(dashNominationStatu);
                db.SaveChanges();
                result = "success";
            }
            catch(Exception ex)
            {
                result = "fail";
            }

            return Ok();
        }

        // DELETE: api/DashNominationStatus/5
        //[ResponseType(typeof(DashNominationStatu))]
        //public IHttpActionResult DeleteDashNominationStatu(int id)
        //{
        //    DashNominationStatu dashNominationStatu = db.DashNominationStatus.Find(id);
        //    if (dashNominationStatu == null)
        //    {
        //        return NotFound();
        //    }

        //    db.DashNominationStatus.Remove(dashNominationStatu);
        //    db.SaveChanges();

        //    return Ok(dashNominationStatu);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DashNominationStatuExists(Guid TransactionId)
        {
            return db.DashNominationStatus.Count(e => e.TransactionId == TransactionId) > 0;
        }
    }
}