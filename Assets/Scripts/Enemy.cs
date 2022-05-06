using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Settings
    [SerializeField]
    private float _speed = -4f;
    private Vector3 _shotOffset = new Vector3(0, -0.95f, 0);

    // Damage
    private Animator _animator;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    // Laser
    private float _fireRate;
    private float _canFire;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserSound;

    // References
    private Player _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!_player)
        {
            Debug.LogError("Failed to find Game Object with tag: Player");
        }

        _animator = gameObject.GetComponent<Animator>();
        if (!_animator)
        {
            Debug.LogError("Failed to find Animator on Enemy");
        }

        _audioSource = GetComponent<AudioSource>();
        if(!_audioSource)
        {
            Debug.LogError("Failed to find Audio Source on Enemy");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
    }

    void Update()
    {
        Movement();
        CheckInBounds();
        Fire();
    }

    void Movement() 
    {
        transform.Translate(new Vector3(0, _speed, 0) * Time.deltaTime);
    }

    void CheckInBounds() 
    {
        if (transform.position.y <= -6)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 7.1f, 0);
        }
    }

     void Fire()
    {
        _fireRate = Random.Range(3f, 8f);

        if (this.GetComponent<BoxCollider2D>().enabled == false)
        {
            // Do nothing
        }
        else if (Time.time > _canFire)
        {
            Instantiate(_laserPrefab, transform.position + _shotOffset, Quaternion.identity);
            _canFire = Time.time + _fireRate;
            AudioSource.PlayClipAtPoint(_laserSound, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            Player _player = other.gameObject.GetComponent<Player>();
            _player.TakeDamage();
            this.GetComponent<BoxCollider2D>().enabled = false;
            _speed = -1f;
            _animator.SetTrigger("onEnemyDestroy");
            _audioSource.Play();
            Destroy(this.gameObject, 3.018f);
        }

        // Need to determine which player shot the laser for competitive mode
        if (other.tag == "Laser")
        {
            _player.AddScore(10);
            Destroy(other.gameObject);
            this.GetComponent<BoxCollider2D>().enabled = false;
            _speed = -1f;
            _animator.SetTrigger("onEnemyDestroy");
            _audioSource.Play();
            Destroy(this.gameObject, 3.018f);
        }
    }
}
