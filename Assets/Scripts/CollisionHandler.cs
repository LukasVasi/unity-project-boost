using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider), typeof(PlayerController))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    private float m_handlingDelay = 1.5f;

    private PlayerController m_playerController;

    private void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag) 
        {
            case "Safe":
                Debug.Log("Collided with a safe object");
                break;
            case "Goal":
                m_playerController.enabled = false;
                Debug.Log("You've completed the level");
                Invoke("HandleComplete", m_handlingDelay);
                break;
            default:
                m_playerController.enabled = false;
                Debug.Log("You've crashed");
                Invoke("HandleCrash", m_handlingDelay);
                break;
        }
    }

    private void HandleComplete()
    {
        int currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelBuildIndex = SceneManager.sceneCountInBuildSettings <= currentLevelBuildIndex + 1 ? 0 : currentLevelBuildIndex + 1;
        LoadLevel(nextLevelBuildIndex);
    }

    private void HandleCrash()
    {
        int currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(currentLevelBuildIndex);
    }

    private void LoadLevel(int levelBuildIndex)
    {
        SceneManager.LoadScene(levelBuildIndex);
    }
}
