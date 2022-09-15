using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [SerializeField] private AudioClip win;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Win")
        {
            SceneManager.LoadScene("Winning Screen");
            Audio.Instance.PlayMusic(win);
        }
    }
}
