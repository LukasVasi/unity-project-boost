using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Rocket Movement")]
    [SerializeField] private float m_thrustForce = 775.0f;
    [SerializeField] private float m_rotationTorque = 175.0f;

    [Header("SFX")]
    [SerializeField] private AudioClip m_thrusterSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem m_mainThrusterParticles;
    [SerializeField] private ParticleSystem m_rightThrusterParticles;
    [SerializeField] private ParticleSystem m_leftThrusterParticles;

    private Rigidbody m_rigidbody;
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();
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
            if (!m_mainThrusterParticles.isPlaying)
            {
                m_mainThrusterParticles.Play();
            }
        }
        else
        {
            m_mainThrusterParticles.Stop();
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!m_leftThrusterParticles.isPlaying)
            {
                m_leftThrusterParticles.Play();
            }
        }
        else
        {
            m_leftThrusterParticles.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (!m_rightThrusterParticles.isPlaying)
            {
                m_rightThrusterParticles.Play();
            }
        }
        else
        {
            m_rightThrusterParticles.Stop();
        }
    }

    private void UpdateThrusterAudio()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space))
        {
            if (!m_audioSource.isPlaying) 
            {
                m_audioSource.PlayOneShot(m_thrusterSound);
            }
        }
        else if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
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
            rotationTorque += m_rotationTorque;
            
        }

        if (Input.GetKey(KeyCode.D))
        {
            rotationTorque -= m_rotationTorque;
        }

        m_rigidbody.AddRelativeTorque(Vector3.forward * rotationTorque * Time.deltaTime);
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            m_rigidbody.AddRelativeForce(Vector3.up * m_thrustForce * Time.deltaTime);
        }
    }
}
