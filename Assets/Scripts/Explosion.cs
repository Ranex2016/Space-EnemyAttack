using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null){
            Debug.LogError("AudioSource is NULL");
        }
        else
        {
            audioSource.Play();
        }
        Destroy(this.gameObject, 3.0f);
    }
}
