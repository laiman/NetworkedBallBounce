using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

public class NetworkConfig : MonoBehaviour
{
    [SerializeField] string httpAddress;
    [SerializeField] int updatesPerSecond = 1;
    [SerializeField] static bool isSending = false;

    DateTime startTime;
    private Ping ping;
    private string testAddress = "8.8.8.8";
    public static NetworkConfig instance;

    //public delegate void NetworkEvents();

    public delegate void NetworkEventsCompletedEventHandler();

    public static event NetworkEventsCompletedEventHandler SendData;

    public void Awake()
    {
        startTime = DateTime.Now;
        if (instance!= null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    public void Update()
    {
        if (!isSending)
        {
            return;
        }

        TimeSpan tSpan = DateTime.Now - startTime;
        if (tSpan.TotalSeconds >= (double)(1/updatesPerSecond))
        {
            SendData?.Invoke();
            startTime = DateTime.Now;
        }
    }

    public void Start()
    {
        PingForConnection(testAddress);
    }

    #region CheckInternetStatus Helper funcs

    /// <summary>
    /// Asynchronous ping request to check if connected.
    /// </summary>
    /// <param name="who">IP address to be pinged</param> 
    public void PingForConnection(string who)
    {


        AutoResetEvent waiter = new AutoResetEvent(false);
        ping = new Ping();
        // When the PingCompleted event is raised,
        // the PingCompletedCallback method is called.
        ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);


        // Create a buffer of 32 bytes of data to be transmitted.
        string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] buffer = Encoding.ASCII.GetBytes(data);

        // Wait 3 seconds for a reply.
        int timeout = 3000;
        PingOptions options = new PingOptions(64, true);
        Debug.LogFormat("Time to live: {0}", options.Ttl);
        ping.SendAsync(who, timeout, buffer, options, waiter);

    }
    private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
    {
        // If the operation was canceled, display a message to the user.
        if (e.Cancelled)
        {
            Console.WriteLine("Ping canceled.");

            // Let the main thread resume. 
            // UserToken is the AutoResetEvent object that the main thread 
            // is waiting for.
            ((AutoResetEvent)e.UserState).Set();
        }

        // If an error occurred, display the exception to the user.
        if (e.Error != null)
        {
            Console.WriteLine("Ping failed:");
            Console.WriteLine(e.Error.ToString());

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
        }
        else
        {
            isSending = false;
        }
    }
    #endregion


}
