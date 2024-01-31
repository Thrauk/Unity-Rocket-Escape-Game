using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float reloadDelayTime = 1f;
    [SerializeField] float nextLevelDelayTime = 1f;
    [SerializeField] AudioClip crashAudioClip;

    [SerializeField] AudioClip successAudioClip;


    Movement movement;
    AudioSource audioSource;

    bool isTransitioning = false;


    void Start()
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning) return;
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartNextLevelSequence();
                break;
            case "Fuel":
                Debug.Log("You picked up some fuel");
                break;
            default:
                StartCrashSequence();
                break;

        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        movement.enabled = false;
        PlayAudioEffect(crashAudioClip);
        Invoke("ReloadLevel", reloadDelayTime);
    }

    void StartNextLevelSequence()
    {
        isTransitioning = true;

        movement.enabled = false;
        PlayAudioEffect(successAudioClip);

        Invoke("LoadNextLevel", nextLevelDelayTime);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }


    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void PlayAudioEffect(AudioClip audioClip)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(audioClip);
    }

}
