using UnityEngine;

namespace LEXRCN.Networking
{
    ///<summary>
    ///Allows network behaviour on a gameObject. Requires a SyncComponent.
    /// </summary>
    public class NetworkSocket : MonoBehaviour
    {
        [SerializeField] SyncComponent[] syncComponents;
        [SerializeField] bool clientOwned;
        [SerializeField] int ID;

        private void OnEnable()
        {
            NetworkConfig.SendData += OnSendData;
        }
        private void OnDisable()
        {
            NetworkConfig.SendData -= OnSendData;
        }
        private void OnSendData()
        {
            if (syncComponents == null)
            {
                return;
            }
            foreach (SyncComponent comp in syncComponents)
            {
                comp.SendData(ID);
            }
        }

    }
}
