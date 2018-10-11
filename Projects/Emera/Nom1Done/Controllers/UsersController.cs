using Nom.ViewModel;
using Nom1Done.Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        IUserService _userService;
        IPipelineService _pipelineService;
        public UsersController(IUserService _userService, IPipelineService _pipelineService) : base(_pipelineService)
        {
            this._userService = _userService;
            this._pipelineService = _pipelineService;
        }

        public ActionResult Index()
        {
            UsersDTO model = new UsersDTO();
            model.ShipperCompanyId = GetCurrentCompanyID();            
            List<string> filteredUsers = _userService.GetAllByShipperID(model.ShipperCompanyId, UserManager.Users.Select(a => a.Id).ToList());
            model.userList = UserManager.Users.Where(a => filteredUsers.Contains(a.Id)).Select(a => new UserDTO
            {
                Username = a.UserName,
                Email = a.Email,
                PhoneNo = a.PhoneNumber
            }).ToList();
            return View(model);
        }
    }
}