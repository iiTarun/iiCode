using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class DefaultController : ApiController
    {
        public List<string> Get()
        {
            return new List<string>() {"hello" };
        }
    }
}
