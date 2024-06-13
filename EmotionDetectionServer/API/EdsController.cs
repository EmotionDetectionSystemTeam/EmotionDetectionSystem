using Microsoft.AspNetCore.Mvc;
using EmotionDetectionSystem.ServiceLayer;
using WebSocketSharp.Server;
using EmotionDetectionSystem.ServiceLayer.Responses;
using EmotionDetectionSystem.ServiceLayer.objects;
using EmotionDetectionSystem.DomainLayer;
using EmotionDetectionSystem.DomainLayer.Managers;
using EmotionDetectionSystem.ServiceLayer.objects.charts;

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
            NotificationManager.GetInstance().setNotificationServer(notificationServer);
            this.logserver = lgs;

        }
        public class NotificationsService : WebSocketBehavior
        {

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
        [Route("login2")]
        public async Task<ObjectResult> Login2([FromBody] LoginRequest request)
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
        [Route("login")]
        public async Task<ObjectResult> Login([FromBody] LoginRequest request)
        {
            string relativeServicePath = "/" + request.email + "-notifications";
            try
            {
                if (notificationServer.WebSocketServices[relativeServicePath] == null)
                {
                    notificationServer.AddWebSocketService<NotificationsService>(relativeServicePath);

                }
            }
            catch (Exception ex)
            {
                var loginResponse = new ServerResponse<string>
                {
                    errorMessage = ex.Message,
                };
                return BadRequest(loginResponse);
            } // in case the client tries to login again

            string session = HttpContext.Session.Id;
            Response response = await Task.Run(() => service.Login(request.sessionId, request.email, request.password));
            if (response.ErrorOccured)
            {
                notificationServer.RemoveWebSocketService(relativeServicePath);
                var loginResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(loginResponse);
            }
            else
            {
                var createShopResponse = new ServerResponse<string>
                {
                    value = session,
                };
                return Ok(createShopResponse);
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

        [HttpPost]
        [Route("emotion-notification")]
        public async Task<ObjectResult> EmotionNotificationRequest([FromBody] EmotionNotificationRequest request)
        {
            Response response = await Task.Run(() => service.NotifySurpriseStudent(request.SessionId, request.TeacherEmail, request.StudentEmail));
            if (response.ErrorOccured)
            {
                var EmotionNotificationResponse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(EmotionNotificationResponse);
            }
            else
            {
                var EmotionNotificationResponse = new ServerResponse<string>
                {
                    value = "Notification pushed successfuly",
                };
                return Ok(EmotionNotificationResponse);
            }
        }
        [HttpPost]
        [Route("get-enrolled-lessons")]
        public async Task<ObjectResult> GetEnrolledLessonsRequest([FromBody] GetEnrolledLessonsRequest request)
        {
            Response<List<LessonOverview>> response = await Task.Run(() => service.GetEnrolledLessons(request.SessionId, request.TeacherEmail));
            if (response.ErrorOccured)
            {
                var GetEnrolledLessonsRespnse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(GetEnrolledLessonsRespnse);
            }
            else
            {
                var GetEnrolledLessonsRespnse = new ServerResponse<List<LessonOverview>>
                {
                    value = response.Value,
                };
                return Ok(GetEnrolledLessonsRespnse);
            }
        }
        [HttpPost]
        [Route("get-students-data")]
        public async Task<ObjectResult> GetStudentDataByLessonRequest([FromBody] GetStudentDataByLessonRequest request)
        {
            Response<List<StudentInClassOverview>> response = await Task.Run(() => service.GetStudentDataByLesson(request.SessionId, request.Email, request.LessonId));
            if (response.ErrorOccured)
            {
                var GetStudentDataByLessonRespnse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(GetStudentDataByLessonRespnse);
            }
            else
            {
                var GetStudentDataByLessonRespnse = new ServerResponse<List<StudentInClassOverview>>
                {
                    value = response.Value,
                };
                return Ok(GetStudentDataByLessonRespnse);
            }
        }
        [HttpPost]
        [Route("get-students-data")]
        public async Task<ObjectResult> GetStudentDataRequest([FromBody] GetStudentDataRequest request)
        {
            Response<List<StudentOverview>> response = await Task.Run(() => service.GetStudentData(request.SessionId, request.Email));
            if (response.ErrorOccured)
            {
                var GetStudentDataRespnse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(GetStudentDataRespnse);
            }
            else
            {
                var GetStudentDataRespnse = new ServerResponse<List<StudentOverview>>
                {
                    value = response.Value,
                };
                return Ok(GetStudentDataRespnse);
            }
        }
        [HttpPost]
        [Route("leave-lesson")]
        public async Task<ObjectResult> LeaveLessonRequest([FromBody] LeaveLessonRequest request)
        {
            Response response = await Task.Run(() => service.LeaveLesson(request.SessionId, request.Email, request.LessonId));
            if (response.ErrorOccured)
            {
                var LeaveLessonRespnse = new ServerResponse<string>
                {
                    errorMessage = response.ErrorMessage,
                };
                return BadRequest(LeaveLessonRespnse);
            }
            else
            {
                var LeaveLessonRespnse = new ServerResponse<string>
                {
                    value = "Leaved Lesson Successfully",
                };
                return Ok(LeaveLessonRespnse);
            }
        }

    }
}