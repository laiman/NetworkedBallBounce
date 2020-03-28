using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkSocket : MonoBehaviour
{
    [SerializeField] SyncComponent[] syncComponents;
    [SerializeField] bool clientOwned;
    [SerializeField] int ID;

    private void OnEnable()
    {
        NetworkConfig.SendData += OnSendData;
    }
    private void OnSendData()
    {
        if(syncComponents == null)
        {
            return;
        }
        foreach(SyncComponent comp in syncComponents)
        {
            comp.SendData(ID);
        }
    }

}
