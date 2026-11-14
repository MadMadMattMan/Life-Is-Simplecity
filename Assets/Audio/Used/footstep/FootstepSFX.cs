using System.Collections;
using UnityEngine;

public class FootstepSFX : MonoBehaviour
{
    [SerializeField] AudioClip[] footstepLibrary;
    AudioSource audioSource;
    float rate;
    bool play = false;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(float delay = 0.4f) {
        play = true;
        StartCoroutine(footstepWaitLoop(delay));
    }

    public void Stop() {
        play = false;
    }


    IEnumerator footstepWaitLoop(float delay) {
        int lasti = 0;
        while (play) {
            int i = 0;
            while (lasti == i)
                i = Random.Range(0, footstepLibrary.Length);
            audioSource.PlayOneShot(footstepLibrary[i]);
            yield return new WaitForSeconds(delay);

        }
    }
}
