using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip jumping;
    [SerializeField] private AudioClip walking;
    [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;

    private Rigidbody2D move;
    private Transform body;

    private bool isActive = false;
    private float resetMusic = 27f;
    private float timer = 0f; //for looping background music
    private bool isWalking = false;
    public bool notflying = false;

    private void Awake()
    {
        body = GetComponent<Transform>();
        move = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Audio.Instance.SetMusicVolume(0.75f);
        Audio.Instance.SetSFXVolume(0.45f);

        timer = Time.deltaTime + timer;

        if(timer > resetMusic)
        {
            timer = 0f;
            isActive = false;
        }
        //TODO
        if(body.position.y < 87 && !isActive)
        {
            Audio.Instance.PlayMusic(music1);
            isActive = true;
        }
        if(body.position.y > 87 && isActive)
        {
            Audio.Instance.PlayMusicWithFade(music1);
            Audio.Instance.PlayMusic(music2);
            isActive = false;
        }
        if(!PlayerMovement.Instance.isGrounded() && !notflying)
        {
            Audio.Instance.PlaySFX(jumping);
            notflying = !notflying;
        }

        if (PlayerMovement.Instance.isGrounded() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space)))
        {
            notflying = false;
        }

    }
}
