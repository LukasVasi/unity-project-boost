using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_thrustForce = 10.0f;

    [SerializeField]
    private float m_rotationTorque = 10.0f;

    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
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
