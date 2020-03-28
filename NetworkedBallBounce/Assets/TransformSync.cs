using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

public class TransformSync : SyncComponent
{
    [SerializeField] bool usePosition;
    [SerializeField] bool useRot;
    [SerializeField] bool useScale;



    DataSync dataSync;
    private static readonly HttpClient client = new HttpClient();
    public void Awake()
    {
        dataSync = gameObject.AddComponent<DataSync>();
    }

    public async override Task SendData(int ID)
    {
        await dataSync.SyncData(ID);
        await PostRequestAsync(dataSync);
    }

    private async Task PostRequestAsync(DataSync data)
    {
        string json = await Task.Run(() => JsonUtility.ToJson(data));
        HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://httpbin.org/post", httpContent);
        response.EnsureSuccessStatusCode();
        string responseString = await response.Content.ReadAsStringAsync();
        Debug.Log(responseString);
        

    }
}

[Serializable]
public class DataSync : MonoBehaviour
{
    public Vector3 m_Position;
    public Quaternion m_Rotation;
    public Vector3 m_Scale;
    public string time;
    public int ID;

    public async Task SyncData(int iD)
    {
        m_Position = transform.position;
        m_Rotation = transform.rotation;
        m_Scale = transform.localScale;
        time = DateTime.Now.ToString();
        ID = iD;        
    }
}

