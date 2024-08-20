using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Rocket Movement")]
    [SerializeField] private float m_ThrustForce = 775.0f;
    [SerializeField] private float m_RotationTorque = 50.0f;

    [Header("SFX")]
    [SerializeField] private AudioClip m_ThrusterSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem m_MainThrusterParticles;
    [SerializeField] private ParticleSystem m_RightThrusterParticles;
    [SerializeField] private ParticleSystem m_LeftThrusterParticles;

    private Rigidbody m_Rigidbody;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateThrusterAudio();
        UpdateThrusterParticles();
    }

    private void UpdateThrusterParticles()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!m_MainThrusterParticles.isPlaying)
            {
                m_MainThrusterParticles.Play();
            }
        }
        else
        {
            m_MainThrusterParticles.Stop();
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!m_LeftThrusterParticles.isPlaying)
            {
                m_LeftThrusterParticles.Play();
            }
        }
        else
        {
            m_LeftThrusterParticles.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (!m_RightThrusterParticles.isPlaying)
            {
                m_RightThrusterParticles.Play();
            }
        }
        else
        {
            m_RightThrusterParticles.Stop();
        }
    }

    private void UpdateThrusterAudio()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space))
        {
            if (!m_AudioSource.isPlaying) 
            {
                m_AudioSource.PlayOneShot(m_ThrusterSound);
            }
        }
        else if (m_AudioSource.isPlaying)
        {
            m_AudioSource.Stop();
        }
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessRotation()
    {
        float rotationTorque = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            rotationTorque += m_RotationTorque;
            
        }

        if (Input.GetKey(KeyCode.D))
        {
            rotationTorque -= m_RotationTorque;
        }

        m_Rigidbody.AddRelativeTorque(Vector3.forward * rotationTorque * Time.deltaTime);
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            m_Rigidbody.AddRelativeForce(Vector3.up * m_ThrustForce * Time.deltaTime);
        }
    }
}
