using Microsoft.AspNetCore.Mvc;
using EmotionDetectionSystem.ServiceLayer;
using WebSocketSharp.Server;
using EmotionDetectionSystem.ServiceLayer.Responses;

namespace EmotionDetectionServer.API
{
    [ApiController]
    [Route("api/eds")]
    public class EdsController : ControllerBase
    {
        private IEdsService service;
        private WebSocketServer notificationServer;
        private WebSocketServer logserver;

        public EdsController(WebSocketServer notificationServer, WebSocketServer lgs)
        {
            this.service = new EdsService();
            this.notificationServer = notificationServer;
            this.logserver = lgs;

        }
        public class logsService : WebSocketBehavior
        {

        }

        [HttpPost]
        [Route("register")]
        public async Task<ObjectResult> Register([FromBody] RegisterRequest request)
        {
            Response response = await Task.Run(() => service.Register(request.email, request.firstName, request.lastName,
                request.password, request.confirmPassword, request.isStudent));
            if (response.ErrorOccured)
            {
                var RegisterResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(RegisterResponse);
            }
            else
            {
                var RegisterResponse = new ServerResponse<string>
                {
                    value = "Registered successfully",
                };
                return Ok(RegisterResponse);
            }
        }

        [HttpPost]
        [Route("enter-as-guest")]
        public async Task<ObjectResult> EnterAsGuest([FromBody] EnterAsGuestRequest request)
        {
            string session = HttpContext.Session.Id;
            Response response = await Task.Run(() => service.EnterAsGuest(session));
            if (response.ErrorOccured)
            {
                var enterAsGuestResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(enterAsGuestResponse);
            }
            else
            {
                var enterAsGuestResponse = new ServerResponse<string>
                {
                    value = session,
                };
                return Ok(enterAsGuestResponse);
            }
        }

    }
}