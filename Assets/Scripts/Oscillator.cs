using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 m_MovementVector = Vector3.zero;
    [SerializeField] private float m_MovementSpeed = 1f;

    private float m_MovementTime = 0f;
    private Vector3 m_StartingPosition;

    void Start()
    {
        m_StartingPosition = transform.position;
    }

    void Update()
    {
        m_MovementTime += Time.deltaTime;
        float movementFactor = MathF.Sin(m_MovementTime * m_MovementSpeed);
        float movementFactorNormalized = (movementFactor + 1f) / 2f;
        transform.position = m_StartingPosition + m_MovementVector * movementFactorNormalized;
    }
}
