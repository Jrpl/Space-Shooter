using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Settings
    [SerializeField]
    private float _speed = -3f;
    [SerializeField]
    private int powerupID;

    // Sound
    [SerializeField]
    private AudioClip _pickupSound;

    void Update()
    {
        Movement();
        CheckInBounds();
    }

    void Movement()
    {
        transform.Translate(new Vector3(0, _speed, 0) * Time.deltaTime);
    }

    void CheckInBounds()
    {
        if (transform.position.y <= -5.6)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player") 
        {
            Player _player = other.gameObject.GetComponent<Player>();
            _player.EnablePowerup(powerupID);
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
            Destroy(this.gameObject);
        }
    }
}
