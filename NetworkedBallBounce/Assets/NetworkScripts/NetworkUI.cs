using UnityEngine;
using TMPro;
using System;

namespace LEXRCN.Networking
{
    /// <summary>
    /// Update Network info text via callbacks.
    /// </summary>
    /// <remark> 
    /// Uses TMPro for VRs sake. Use OnEditorGUI for 2D display
    /// </remark>
    public class NetworkUI : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI m_ConnectionText;
        string text;

        private void OnEnable()
        {
            NetworkConfig.Connected += UpdateConnectionState;
            m_ConnectionText = GetComponent<TextMeshProUGUI>();
            text = m_ConnectionText.text;


        }
        private void OnDisable()
        {
            NetworkConfig.Connected -= UpdateConnectionState;

        }

        private void Update()
        {
            if(text!= m_ConnectionText.text)
            {
                m_ConnectionText.text = text;
            }
        }

        void UpdateConnectionState()
        {
            Debug.Log("Internet connection available");
            try
            {
                text = "Internet connection available";

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}