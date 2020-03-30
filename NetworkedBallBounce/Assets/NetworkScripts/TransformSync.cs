using System;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;

namespace LEXRCN.Networking {

    ///<summary>
    /// SyncComponent for sharing transform over network.
    /// </summary>
    [RequireComponent(typeof(NetworkSocket))]
    public class TransformSync : SyncComponent
    {

        DataSync dataSync;
        private static readonly HttpClient client = new HttpClient();
        public void Awake()
        {
            dataSync = gameObject.AddComponent<DataSync>();
        }

        public async override Task SendData(int ID)
        {
            await dataSync.SyncData(ID);
            await Task.Run( async()=>await NetworkConfig.instance.PostRequestAsync(dataSync));
        }
    }

    ///<summary>
    ///Data Class to be serialized for network sync. 
    /// </summary>
    [Serializable]
    public class DataSync : MonoBehaviour
    {
        public Vector3 m_Position;
        //public Quaternion m_Rotation;
        //public Vector3 m_Scale;
        public string time;
        public int ID;

        public async Task SyncData(int iD)
        {
            m_Position = transform.position;
            //m_Rotation = transform.rotation;
            //m_Scale = transform.localScale;
            time = DateTime.Now.ToString();
            ID = iD;
        }
    }

}
