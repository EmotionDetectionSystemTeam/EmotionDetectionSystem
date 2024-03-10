using Microsoft.AspNetCore.Mvc;
using EmotionDetectionSystem.ServiceLayer;
using WebSocketSharp.Server;
using EmotionDetectionSystem.ServiceLayer.Responses;
using EmotionDetectionSystem.ServiceLayer.objects;

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
                var LoginResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(LoginResponse);
            }
            else
            {
                var LoginResponse = new ServerResponse<string>
                {
                    value = "Logged in successfully",
                };
                return Ok(LoginResponse);
            }
        }

        [HttpPost]
        [Route("create-lesson")]
        public async Task<ObjectResult> CreateLesson([FromBody] CreateLessonRequest request)
        {
            Response<SActiveLesson> response = await Task.Run(() => service.CreateLesson(request.SessionId, request.Email, request.Title, request.Description, request.Tags));
            if (response.ErrorOccured)
            {
                var CreateLessonResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(CreateLessonResponse);
            }
            else
            {
                var CreateLessonResponse = new ServerResponse<SActiveLesson>
                {
                    value = response.Value,
                };
                return Ok(ServerResponse<SActiveLesson>.sendOkResponse(response.Value));
            }
        }

        [HttpPost]
        [Route("join-lesson")]
        public async Task<ObjectResult> JoinLesson([FromBody] JoinLessonRequest request)
        {
            Response<SActiveLesson> response = await Task.Run(() => service.JoinLesson(request.SessionId, request.Email, request.EntryCode));
            if (response.ErrorOccured)
            {
                var JoinLessonResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(JoinLessonResponse);
            }
            else
            {
                var JoinLessonResponse = new ServerResponse<SActiveLesson>
                {
                    value = response.Value,
                };
                return Ok(JoinLessonResponse);
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<ObjectResult> Logout([FromBody] LogoutRequest request)
        {
            Response response = await Task.Run(() => service.Logout(request.SessionId, request.Email));
            if (response.ErrorOccured)
            {
                var LogoutResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(LogoutResponse);
            }
            else
            {
                var LogoutResponse = new ServerResponse<string>
                {
                    value = "logged out successfully",
                };
                return Ok(LogoutResponse);
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

        [HttpPost]
        [Route("push-emotion-data")]
        public async Task<ObjectResult> PushEmotionData([FromBody] PushEmotionDataRequest request)
        {
            Response response = await Task.Run(() => service.PushEmotionData(request.SessionId, request.Email, request.LessonId, request.EmotionData));
            if (response.ErrorOccured)
            {
                var pushEmotionDataResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(pushEmotionDataResponse);
            }
            else
            {
                var pushEmotionDataResponse = new ServerResponse<string>
                {
                    value = "Data pushed successfuly",
                };
                return Ok(pushEmotionDataResponse);
            }
        }

        [HttpPost]
        [Route("get-last-emotions-data")]
        public async Task<ObjectResult> GetLastEmotionsData([FromBody] GetLastEmotionsDataRequest request)
        {
            Response<List<ServiceRealTimeUser>> response = await Task.Run(() => service.GetLastEmotionsData(request.SessionId, request.Email, request.LessonId));
            if (response.ErrorOccured)
            {
                var ErrorResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(ErrorResponse);
            }
            else
            {
                var SuccessResponse = new ServerResponse<List<ServiceRealTimeUser>>
                {
                    value = response.Value,
                };
                return Ok(SuccessResponse);
            }
        }

        [HttpPost]
        [Route("get-lesson")]
        public async Task<ObjectResult> GetLesson([FromBody] GetLessonRequest request)
        {
            Response<SActiveLesson> response = await Task.Run(() => service.GetLesson(request.SessionId, request.Email, request.LessonId));
            if (response.ErrorOccured)
            {
                var GetLessonResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(GetLessonResponse);
            }
            else
            {
                var GetLessonResponse = new ServerResponse<SActiveLesson>
                {
                    value = response.Value,
                };
                return Ok(GetLessonResponse);
            }
        }






    }
}