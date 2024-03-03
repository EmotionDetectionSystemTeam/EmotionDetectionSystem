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

        public EdsController(IEdsService edsService, WebSocketServer notificationServer, WebSocketServer lgs)
        {
            this.service = edsService;
            this.notificationServer = notificationServer;
            this.logserver = lgs;

        }
        public class logsService : WebSocketBehavior
        {

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
        [Route("login")]
        public async Task<ObjectResult> Login([FromBody] LoginRequest request)
        {
            Response response = await Task.Run(() => service.Login(request.sessionId, request.email, request.password));
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
                    value = "Logged in successfully",
                };
                return Ok(RegisterResponse);
            }
        }

        [HttpPost]
        [Route("create-lesson")]
        public async Task<ObjectResult> CreateLesson([FromBody] CreateLessonRequest request)
        {
            Response<ServiceLesson> response = await Task.Run(() => service.CreateLesson(request.SessionId, request.Email, request.Title, request.Description, request.Tags));
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
                    value = response.Value,
                };
                return Ok(RegisterResponse);
            }
        }

        [HttpPost]
        [Route("join-lesson")]
        public async Task<ObjectResult> JoinLesson([FromBody] JoinLessonRequest request)
        {
            Response<ServiceLesson> response = await Task.Run(() => service.JoinLesson(request.SessionId, request.Email, request.EntryCode));
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
                var RegisterResponse = new ServerResponse<ServiceLesson>
                {
                    value = response.Value,
                };
                return Ok(RegisterResponse);
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<ObjectResult> Logout([FromBody] LogoutRequest request)
        {
            Response response = await Task.Run(() => service.Logout(request.SessionId, request.Email));
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
                    value = "logged out successfully",
                };
                return Ok(RegisterResponse);
            }
        }

        [HttpPost]
        [Route("end-lesson")]
        public async Task<ObjectResult> EndLesson([FromBody] EndLessonRequest request)
        {
            Response response = await Task.Run(() => service.EndLesson(request.SessionId, request.Email));
            if (response.ErrorOccured)
            {
                var endLessonResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(endLessonResponse);
            }
            else
            {
                var endLessonResponse = new ServerResponse<string>
                {
                    value = "Lesson ended successfully",
                };
                return Ok(endLessonResponse);
            }
        }




    }
}