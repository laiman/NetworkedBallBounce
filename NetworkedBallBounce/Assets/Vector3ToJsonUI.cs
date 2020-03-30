using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vector3ToJsonUI : MonoBehaviour
{
    [SerializeField] Vector3 vec;
    [SerializeField] TextMeshProUGUI Text;
    void OnEnable() 
    {
       if(vec == null)
        {
            Debug.LogError("No vector assigned to UI text");
        }
    }
    void Update()
    {
        Text.text = vec.ToString();
    }
}

