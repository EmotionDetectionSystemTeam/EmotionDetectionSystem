
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
                        usecaseJson["entryCode"].ToString());
                    if (res.ErrorOccured)
                        throw new Exception("Failed to parse the JoinLesson " + res.ErrorMessage);
                    break;
                }
            default:
                throw new Exception("Unsupported tag in the InitFile ");
        }
    }

}