using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Models;
using ceenq.com.Users.Models;
using ceenq.com.Users.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Users.Models;

namespace ceenq.com.ManagementAPI.Controllers
{
    public class UserApiController : ApiController
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IUserService _userService;
        public UserApiController(IOrchardServices orchardServices, IUserService userService)
        {
            _orchardServices = orchardServices;
            _userService = userService;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public HttpResponseMessage Get()
        {
            
            var users = _userService.GetUsers();

            return Request.CreateResponse(HttpStatusCode.OK, users);//return model
        }
        public IHttpActionResult Get(int id)
        {
            try
            {
                var user = _userService.Get(id);

                if (user == null) return BadRequest(T("Bad Request").Text);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult Put(UserModel model)
        {
            return Save(model);
        }

        public IHttpActionResult Post(UserModel model)
        {
            return Save(model);
        }

        private IHttpActionResult Save(UserModel userModel)
        {
            if (userModel == null) return BadRequest(T("Bad Request").Text);

            try
            {
                if (userModel.Id > 0)
                {
                    _userService.UpdateUser(userModel);
                    return Ok();
                }
                else
                {
                    var user = _userService.CreateUser(userModel);
                    user.As<UserPart>().EmailStatus = UserStatus.Approved;
                    return Created(Request.RequestUri, userModel);
                }

            }
            catch (Exception ex)
            {

                return BadRequest(T("Bad Request").Text);
            }
        }

        public IHttpActionResult Delete(int id)
        {
            try
            {
                var user = _userService.Get(id);

                if (user.Id == _orchardServices.WorkContext.CurrentUser.Id) {
                    return BadRequest(T("You cannot delete your own user account.").Text);
                }

                _userService.DeleteUser(user);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
