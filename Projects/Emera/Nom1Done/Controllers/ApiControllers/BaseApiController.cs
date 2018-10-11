using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Nom1Done.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nom1Done.Controllers.ApiControllers
{
    public class BaseApiController : ApiController
    {
        private ApplicationUserManager _AppUserManager = null;

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public BaseApiController()
        {
        }  
       
    }
}