using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Sound
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource)
        {
            Debug.LogError("Failed to find Audio Source on Explosion Prefab");
        }
        _audioSource.Play();
        Destroy(this.gameObject, 3.018f); 
    }
}
