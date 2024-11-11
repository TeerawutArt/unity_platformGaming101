using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip collectSound; // เสียงเอฟเฟกต์
    private AudioSource audioSource;
    // Start is called before the first frame update

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

    }
    public void PlayCollectSoundEffect()
    {
        if (collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
