using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController), typeof(AudioSource), typeof(Rigidbody))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float m_LevelLoadDelay = 1.5f;
    [SerializeField] private float m_CrashVelocityThreshold = 10f;

    [Header("SFX")]
    [SerializeField] private AudioClip m_CrashSound;
    [SerializeField] private AudioClip m_SuccessSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem m_CrashParticles;
    [SerializeField] private ParticleSystem m_SuccessParticles;

    private AudioSource m_AudioSource;
    private PlayerController m_PlayerController;
    private Rigidbody m_Rigidbody;

    private bool m_IsTransitioning = false;
    private bool m_CollisionHandlingEnabled = true;

    private void Awake()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            HandleComplete();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            m_CollisionHandlingEnabled = !m_CollisionHandlingEnabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_IsTransitioning || !m_CollisionHandlingEnabled) return;

        if(collision.relativeVelocity.magnitude > m_CrashVelocityThreshold)
        {
            HandleCrash();
            return;
        }

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
        if (m_IsTransitioning) return;

        m_IsTransitioning = true;

        m_PlayerController.enabled = false;
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(m_SuccessSound);
        m_SuccessParticles.Play();
        Debug.Log("You've completed the level");

        int currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelBuildIndex = SceneManager.sceneCountInBuildSettings <= currentLevelBuildIndex + 1 ? 0 : currentLevelBuildIndex + 1;
        StartCoroutine(LoadLevelAfterDelay(nextLevelBuildIndex));
    }

    private void HandleCrash()
    {
        if (m_IsTransitioning) return;

        m_IsTransitioning = true;
        
        m_PlayerController.enabled = false;
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(m_CrashSound);
        m_CrashParticles.Play();
        Debug.Log("You've crashed");

        int currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevelAfterDelay(currentLevelBuildIndex));
    }

    private IEnumerator LoadLevelAfterDelay(int levelBuildIndex)
    {
        yield return new WaitForSeconds(m_LevelLoadDelay);
        SceneManager.LoadScene(levelBuildIndex);
    }
}
