using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float reloadDelayTime = 1f;
    [SerializeField] float nextLevelDelayTime = 1f;

    [SerializeField] AudioClip crashAudioClip;
    [SerializeField] AudioClip successAudioClip;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;


    Movement movement;
    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisable = false;


    void Start()
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
            return;
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            collisionDisable = !collisionDisable;
        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisable) return;
        
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
        crashParticles.Play();
        Invoke("ReloadLevel", reloadDelayTime);
    }

    void StartNextLevelSequence()
    {
        isTransitioning = true;

        movement.enabled = false;
        PlayAudioEffect(successAudioClip);
        successParticles.Play();
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
