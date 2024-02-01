using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;


    [SerializeField] ParticleSystem mainThrusterParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }

    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainThrusterParticles.Stop();
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        PlayParticles(mainThrusterParticles);
    }

    private void PlayParticles(ParticleSystem particles)
    {
        if (!particles.isPlaying)
        {
            particles.Play();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopSideThrustersParticles();
        }

    }

    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        PlayParticles(leftThrusterParticles);
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        PlayParticles(rightThrusterParticles);
    }

    private void StopSideThrustersParticles()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
