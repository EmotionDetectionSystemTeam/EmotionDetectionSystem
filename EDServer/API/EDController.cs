using Microsoft.AspNetCore.Mvc;
using EmotionDetectionSystem.Service;
using WebSocketSharp.Server;

namespace EDServer.API
{
    [ApiController]
    [Route("api/eds")]
    public class EDController : ControllerBase
    {
        private IEmsService service;
        private WebSocketServer notificationServer;
        private WebSocketServer logserver;

        public EDController(WebSocketServer notificationServer, WebSocketServer lgs)
        {
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
            string a = "here";
            Response response = await Task.Run(() => service.Register(request.email, request.firstName, request.lastName, 
                request.password , request.confirmPassword, request.isStudent));
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

    }
}
