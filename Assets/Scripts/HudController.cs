using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] Rigidbody m_PlayerRigidbody;
    [SerializeField] private TextMeshProUGUI m_VelocityText;

    private void Start()
    {
        if(!m_PlayerRigidbody || !m_VelocityText) enabled = false;
    }

    void Update()
    {
        float velocity = Mathf.Round(m_PlayerRigidbody.velocity.magnitude * 10) / 10;
        m_VelocityText.text = velocity.ToString();
    }
}
