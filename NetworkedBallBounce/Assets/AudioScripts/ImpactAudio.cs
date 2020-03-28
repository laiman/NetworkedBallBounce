using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ImpactAudio : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip OnImpactSound;

    [SerializeField]
    [Range(0.0f,2.99f)] 
    float minPitch = 0.8f;

    [SerializeField]
    [Range(0.0f, 2.99f)] 
    float maxPitch = 0.84f;

    private AudioSource m_AudioSource;
    private void OnEnable()
    {
        m_AudioSource = GetComponent<AudioSource>();
        if (OnImpactSound == null)
        {
            Debug.LogWarningFormat("{0} object has no impact clip assigned. Please set in inspector", this.gameObject.name);
        }
        else
        {
            m_AudioSource.clip = OnImpactSound;
        }

    }

    private void OnCollisionEnter(Collision collision)
        {
            m_AudioSource.pitch = Random.Range(minPitch, maxPitch);
            m_AudioSource.Play();
        }
}
