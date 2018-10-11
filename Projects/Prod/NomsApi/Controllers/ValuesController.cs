using Microsoft.AspNet.Identity;
using NomsApi.DTO;
using NomsApi.Service;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NomsApi.Controllers
{
    public class ValuesController : ApiController
    {
        [Authorize]
        [HttpPost]
        public HttpResponseMessage PostForNNG(PathedNonPathedHybridDTO obj)
        {
            var response=new Object();
            try
            {
                var UserId = User.Identity.GetUserId();
                PathedNonpathedValidation validate = new PathedNonpathedValidation();
                response = validate.ValidationSaveSendNoms(obj, UserId);
            }
            catch(Exception ex)
            {
                response = new { ResponseMessage = "Something went wrong." };
            }
            var res = Request.CreateResponse(HttpStatusCode.OK, response);
            return res;
        }
    }
}
