
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EmotionDetectionSystem.ServiceLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace EmotionDetectionServer;

public class ConfigurationFileHandler
{
    private string adminUserName;
    private string adminPassword;
    private int SessionIds = 0;
    private IEdsService service; 
    private string PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InitFile.json");
    private string entryCode = "";
    private string lessonId = "";

    public ConfigurationFileHandler(IEdsService Service)
    {
        service = Service ?? throw new ArgumentNullException(nameof(Service));
    }


    public void Parse()
    {
        try
        {
            string textJson = File.ReadAllText(PATH);
            dynamic scenarioDtoDict = JObject.Parse(textJson);
            JArray useCasesJson = scenarioDtoDict["UseCases"];
            foreach (var usecase in useCasesJson.ToList())
            {
                string STRusecase = usecase.ToString();
                dynamic useCaseDict = JObject.Parse(STRusecase);
                string tag = useCaseDict["Tag"]!.ToString();
                ParseUseCase(tag, useCaseDict);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    private void ParseUseCase(string tag, JObject usecaseJson)
    {
        switch (tag)
        {
            case "Register":
                {
                    var res = service.Register(
                        usecaseJson["email"].ToString(),
                        usecaseJson["firstName"].ToString(),
                        usecaseJson["lastName"].ToString(),
                        usecaseJson["password"].ToString(),
                        usecaseJson["confirmPassword"].ToString(),
                        int.Parse(usecaseJson["userType"].ToString()));
                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the register " + res.ErrorMessage);
                    break;
                }
            case "Login":
                {

                    var res = service.Login(
                        usecaseJson["sessionid"].ToString(),
                        usecaseJson["email"].ToString(),
                        usecaseJson["password"].ToString());

                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the login: " + res.ErrorMessage);
                    break;
                }
            case "Logout":
                {

                    var res = service.Logout(
                        usecaseJson["sessionid"].ToString(),
                        usecaseJson["email"].ToString());

                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the logout: " + res.ErrorMessage);
                    break;
                }
            case "CreateLesson":
                {
                    var res = service.CreateLesson(
                        usecaseJson["sessionid"].ToString(),
                        usecaseJson["email"].ToString(),
                        usecaseJson["title"].ToString(),
                        usecaseJson["description"].ToString(),
                        usecaseJson["tags"].ToObject<List<string>>().ToArray());

                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the createlesson: " + res.ErrorMessage);
                    entryCode = res.Value.EntryCode;
                    lessonId = res.Value.LessonId;
                    break;
                }
            case "EnterAsGuest":
                {
                    var res = service.EnterAsGuest(
                        usecaseJson["sessionid"].ToString());
                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the eneter as guest " + res.ErrorMessage);
                    break;
                }
            case "JoinLesson":
                {
                    var res = service.JoinLesson(
                        usecaseJson["sessionid"].ToString(),
                        usecaseJson["email"].ToString(),
                        entryCode);
                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the JoinLesson " + res.ErrorMessage);
                    break;
                }
            case "PushEmotionData":
                {
                    var res = service.PushEmotionData(
                        usecaseJson["sessionid"].ToString(),
                        usecaseJson["email"].ToString(),
                        lessonId,
                        new EmotionDetectionSystem.ServiceLayer.objects.ServiceEmotionData(Convert.ToDouble(usecaseJson["neutral"].ToString()),
                        Convert.ToDouble(usecaseJson["happy"].ToString()),
                        Convert.ToDouble(usecaseJson["sad"].ToString()),
                        Convert.ToDouble(usecaseJson["angry"].ToString()),
                        Convert.ToDouble(usecaseJson["suprised"].ToString()),
                        Convert.ToDouble(usecaseJson["disgusted"].ToString()),
                        Convert.ToDouble(usecaseJson["fear"].ToString())));
                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the JoinLesson " + res.ErrorMessage);
                    break;
                }
            case "PushEmotionData2":
                {
                    Random rand = new Random();
                    for (int i = 0; i < Convert.ToInt32(usecaseJson["times"].ToString()); i++)
                    {
                        var res = service.PushEmotionData(
                            usecaseJson["sessionid"].ToString(),
                            usecaseJson["email"].ToString(),
                            lessonId,
                            new EmotionDetectionSystem.ServiceLayer.objects.ServiceEmotionData(rand.NextDouble(),
                                rand.NextDouble(),
                                rand.NextDouble(),
                                rand.NextDouble(),
                                rand.NextDouble(),
                                rand.NextDouble(),
                                rand.NextDouble()));
                        if (res.ErrorOccured)
                            throw new Exception("Failed to parse the JoinLesson " + res.ErrorMessage);
                    }
                    break;
                }
            case "EndLesson":
                {
                    var res = service.EndLesson(
                        usecaseJson["sessionid"].ToString(),
                        usecaseJson["email"].ToString());
                    Thread.Sleep(500);
                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the createlesson: " + res.ErrorMessage);
                    break;
                }
            case "GetStudentData":
                {
                    var res = service.GetAllStudentsData(
                        usecaseJson["sessionid"].ToString(),
                        usecaseJson["email"].ToString());

                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the createlesson: " + res.ErrorMessage);
                    break;
                }
            default:
                throw new Exception("Unsupported tag in the InitFile ");
        }
    }

}