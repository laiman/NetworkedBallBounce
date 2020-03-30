using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

namespace LEXRCN.Networking
{
    /// <summary>
    /// Lazy singleton for storing network configuration and handling HTTP requests.
    /// </summary>
    public class NetworkConfig : MonoBehaviour
    {
        [SerializeField] string httpAddress;
        /// <summary>
        /// http://httpbin.org/post can be used for testing. https://cht.app/coordinates is the app endpoint
        /// </summary>
        [SerializeField] int updatesPerSecond = 1;
        [SerializeField] static bool isSending = false;
        [SerializeField] private string testAddress = "8.8.8.8";

        DateTime startTime;
        private Ping ping;
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false});

        //Network Delegate events.
        public delegate void NetworkEventsCompletedEventHandler();
        public static event NetworkEventsCompletedEventHandler SendData;
        public static event NetworkEventsCompletedEventHandler Connected;

        public static NetworkConfig instance;


        #region MonoBehaviour Callbacks
        void Awake()
        {
            startTime = DateTime.Now;
            if (instance != null)
            {
                Destroy(instance);
            }
            instance = this;
        }
        void Update()
        {
            if (!isSending)
            {
                //Task.Run(async()=>PingForConnection(testAddress));
                

                return;
            }
            TimeSpan tSpan = DateTime.Now - startTime;
            if (tSpan.TotalSeconds >= (double)(1 / updatesPerSecond))
            {
                Debug.Log("Sending");
                SendData?.Invoke();
                startTime = DateTime.Now;
            }
        }

        async void Start()
        {
            await PingForConnection(testAddress);
        }

        #endregion

        #region CheckInternetStatus Helper funcs

        /// <summary>
        /// Asynchronous ping request to check if connected.
        /// </summary>
        /// <param name="who">IP address to be pinged</param> 
        private Task PingForConnection(string who)
        {

            return Task.Factory.StartNew(() =>
            {
                Debug.Log("Starting Ping for connection");
                AutoResetEvent waiter = new AutoResetEvent(false);

                Debug.Log("Creating new ping");
                ping = new Ping();
                // When the PingCompleted event is raised,
                // the PingCompletedCallback method is called.
                Debug.Log("New ping event handler");
                ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);


                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                // Wait 3 seconds for a reply.
                int timeout = 3000;
                Debug.Log("New ping options");
                PingOptions options = new PingOptions(128, false);
                Debug.LogFormat("Time to live: {0}", options.Ttl);
                Debug.Log("Send ping");
                var response = ping.SendPingAsync(who, timeout, buffer, options);
                Debug.Log("Response gained");

            });

                
        }
        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            Debug.Log("Ping complete callback");
            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                Debug.Log("Ping canceled.");

                // Let the main thread resume. 
                // UserToken is the AutoResetEvent object that the main thread 
                // is waiting for.
                ((AutoResetEvent)e.UserState).Set();
            }

            // If an error occurred, display the exception to the user.
            if (e.Error != null)
            {
                Debug.Log("Ping failed:");

                Debug.Log(e.Error.ToString());

                // Let the main thread resume. 
                ((AutoResetEvent)e.UserState).Set();
            }

            PingReply reply = e.Reply;

            DisplayReply(reply);

            // Let the main thread resume.
            ((AutoResetEvent)e.UserState).Set();
        }

        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                Debug.LogFormat("Address: {0}", reply.Address.ToString());
                Debug.LogFormat("RoundTrip time: {0}", reply.RoundtripTime);
                Debug.LogFormat("Time to live: {0}", reply.Options.Ttl);
                Debug.LogFormat("Don't fragment: {0}", reply.Options.DontFragment);
                Debug.LogFormat("Buffer size: {0}", reply.Buffer.Length);
                isSending = true;
                Connected?.Invoke();
            }
            else
            {
                isSending = false;
            }
        }
        #endregion

        #region Network requests

        /// <summary>
        /// Send HTTP POST Request Asynchronously. Called by SyncComponents.
        /// </summary>
        /// <param name="data"> Data to be sent </param>
        /// <returns></returns>
        public async Task PostRequestAsync(DataSync data)
        {
            Debug.Log("Read json");
            string json = await Task.Run(() => JsonUtility.ToJson(data));
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.Log("Awaiting response");
            var response = await client.PostAsync(httpAddress, httpContent).ConfigureAwait(true);
            Debug.Log("Response recieved");
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            Debug.LogFormat("Response:{0}", responseString);

        }

        #endregion
    }
}

