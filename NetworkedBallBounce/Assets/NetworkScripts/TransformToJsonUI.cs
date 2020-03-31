using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransformToJsonUI : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] bool usePos;
    [SerializeField] bool useRot;
    [SerializeField] bool useScale;

    void OnEnable() 
    {
       if(trans == null)
        {
            Debug.LogError("No transform assigned to UI text");
        }
    }
    void Update()
    {
        string m_text = "";
        if (usePos)
        {
            m_text += trans.position.ToString();
        }
        if (useRot)
        {
            m_text += trans.rotation.ToString();

        }
        if (useScale)
        {
            m_text += trans.localScale.ToString();

        }
        Text.text = m_text;
    }
}

