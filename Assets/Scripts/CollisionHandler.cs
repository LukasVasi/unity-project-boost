using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider), typeof(PlayerController), typeof(AudioSource))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float m_levelLoadDelay = 1.5f;

    [Header("SFX")]
    [SerializeField] private AudioClip m_crashSound;
    [SerializeField] private AudioClip m_successSound;

    private AudioSource m_audioSource;
    private PlayerController m_playerController;

    private bool m_isTransitioning = false;

    private void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_isTransitioning) return;

        switch (collision.gameObject.tag) 
        {
            case "Safe":
                Debug.Log("Collided with a safe object");
                break;
            case "Goal":
                HandleComplete();
                break;
            default:
                HandleCrash();
                break;
        }
    }

    private void HandleComplete()
    {
        if (m_isTransitioning) return;

        m_isTransitioning = true;

        m_playerController.enabled = false;
        m_audioSource.Stop();
        m_audioSource.PlayOneShot(m_successSound);
        Debug.Log("You've completed the level");

        int currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelBuildIndex = SceneManager.sceneCountInBuildSettings <= currentLevelBuildIndex + 1 ? 0 : currentLevelBuildIndex + 1;
        StartCoroutine(LoadLevelAfterDelay(nextLevelBuildIndex));
    }

    private void HandleCrash()
    {
        if (m_isTransitioning) return;

        m_isTransitioning = true;
        
        m_playerController.enabled = false;
        m_audioSource.Stop();
        m_audioSource.PlayOneShot(m_crashSound);
        Debug.Log("You've crashed");

        int currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevelAfterDelay(currentLevelBuildIndex));
    }

    private IEnumerator LoadLevelAfterDelay(int levelBuildIndex)
    {
        yield return new WaitForSeconds(m_levelLoadDelay);
        SceneManager.LoadScene(levelBuildIndex);
    }
}
