
namespace TestEDS.Logic
{
    public class Clients
    {
        HttpClient client = new HttpClient();
        private readonly Dictionary< string, HttpClient> m_restClients = new Dictionary<string, HttpClient>();
        private Clients() { }

        public static Clients Instance { get; } = new Clients();

        public void ClearZvmRestClients()
        {
            m_restClients.Clear();
        }

        public void AddClient(string i_zvmName, HttpClient i_client)
        {
            m_restClients.Add(i_zvmName, i_client);
        }

        public void UpdateClient(string i_zvmName, HttpClient i_newClient)
        {
            m_restClients[i_zvmName] = i_newClient;
        }

        public void RemoveClient(string i_zvmName)
        {
            m_restClients.Remove(i_zvmName);
        }

        public HttpClient GetZvmRestClient(string i_siteName)
        {
            if (m_restClients.ContainsKey(i_siteName))
            {
                return m_restClients[i_siteName];
            }

            throw new KeyNotFoundException($"Site with key name {i_siteName} was not found");
        }

        public bool IsZvmRestClientExist(string i_siteName)
        {
            return m_restClients.ContainsKey(i_siteName);
        }

    }
}
