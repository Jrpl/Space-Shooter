using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Settings
    [SerializeField]
    private float _speed = -4f;
    [SerializeField]
    private float _zRotSpeed;

    // Damage
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _explosionSound;

    // References
    private Player _player;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!_player)
        {
            Debug.LogError("Failed to find Game Object with tag: Player");
        }

        _audioSource = GetComponent<AudioSource>();
        if(!_audioSource)
        {
            Debug.LogError("Failed to find Audio Source on Asteroid");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        _zRotSpeed = Random.Range(-3f, 4f);
        transform.Translate(new Vector3(0, _speed, 0) * Time.deltaTime);
        transform.Rotate(new Vector3(0, 0, _zRotSpeed) * Time.deltaTime);
    }

    void CheckInBounds() 
    {
        if (transform.position.y <= -6.2)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 8.1f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            Player _player = other.gameObject.GetComponent<Player>();
            _player.TakeDamage();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        // Need to determine which player shot the laser for competitive mode
        if (other.tag == "Laser")
        {
            _player.AddScore(10);
            Destroy(other.gameObject);
            this.GetComponent<PolygonCollider2D>().enabled = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, .25f);
        }
    }
}
