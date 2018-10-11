using Nom.ViewModel;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        ILocationService ILocationService;
       
        public LocationController(ILocationService ILocationService)
        {
            this.ILocationService = ILocationService;
           
        }

        public ActionResult Index(int pipelineId)
        {
            LocationListDTO model = new LocationListDTO();
            model.LocationList = ILocationService.GetLocations(pipelineId).ToList();           
            return View(model);
        }
    }
}