using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Settings
    [SerializeField]
    private int _score = 0;
    private int _highScore = 0;
    private int _health = 3;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    public bool isPlayerOne = false;
    [SerializeField]
    public bool isPlayerTwo = false;

    // Laser
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire;
    private float _playerTwoCanFire;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserSound;
    private Vector3 _shotOffset = new Vector3(0, 0.95f, 0);

    // Powerups
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private bool _speedBoostActive = false;
    [SerializeField]
    private float _speedBoost = 3f;
    [SerializeField]
    private bool _shieldActive = false;
    [SerializeField]
    private GameObject _shieldVisual;

    // Damage
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _rightWingDamaged;
    [SerializeField]
    private GameObject _leftWingDamaged;

    // Managers
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    void Start()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnManager>();
        if (!_spawnManager)
        {
            Debug.LogError("Failed to find Game Object with tag: Spawner");
        }

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (!_uiManager)
        {
            Debug.LogError("Failed to find Game Object: UI_Manager");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (!_gameManager)
        {
            Debug.LogError("Failed to find Game Object: Game_Manager");
        }

        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource)
        {
            Debug.LogError("Failed to find Audio Source on Game Object: Player");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }

    void Update()
    {
        if (isPlayerOne == true)
        {
            Movement();
            Fire();
        }

        if (isPlayerTwo == true)
        {
            PlayerTwoMovement();
            PlayerTwoFire();
        }
    }

    void Movement()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        if (_speedBoostActive)
        {
            transform.Translate(new Vector3(xInput, yInput, 0) * (_speed + _speedBoost) * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(xInput, yInput, 0) * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -9, 9), 
            Mathf.Clamp(transform.position.y, -3.8f, 0), 
            transform.position.z);
    }

    // This is messy, clean up at some point
    void PlayerTwoMovement()
    {
        if (_speedBoostActive == true)
        {
            if (Input.GetKey(KeyCode.I))
            {
                transform.Translate(Vector3.up * (_speed + _speedBoost) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.K))
            {
                transform.Translate(Vector3.down * (_speed + _speedBoost) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.J))
            {
                transform.Translate(Vector3.left * (_speed + _speedBoost) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.L))
            {
                transform.Translate(Vector3.right * (_speed + _speedBoost) * Time.deltaTime);
            }
        }
        else {
            if (Input.GetKey(KeyCode.I))
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.K))
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.J))
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.L))
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -9, 9), 
            Mathf.Clamp(transform.position.y, -3.8f, 0), 
            transform.position.z);
    }

    // These Fire methods probably don't need to be seperate
    // Triple Shot is bugged, prefab not lining up with ship
    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_tripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _audioSource.Play(0);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + _shotOffset, Quaternion.identity);
                _audioSource.Play(0);
            }

            _canFire = Time.time + _fireRate;
        }
    }

    void PlayerTwoFire()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon) && Time.time > _playerTwoCanFire)
        {
            if (_tripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _audioSource.Play(0);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + _shotOffset, Quaternion.identity);
                _audioSource.Play(0);
            }

             _playerTwoCanFire = Time.time + _fireRate;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void CheckHighScore()
    {
        if (_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("HighScore", _score);
            _uiManager.UpdateHighScore(_highScore);
        }
    }

    public void TakeDamage()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }
        else
        {
            _health--;
            _uiManager.UpdateHealthDisplay(_health);

            if (_health == 2)
            {
                _rightWingDamaged.SetActive(true);
            }

            if (_health == 1)
            {
                _leftWingDamaged.SetActive(true);
            }

            if (_health <= 0)
            {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject, 0.25f);
                CheckHighScore();
                _spawnManager.OnPlayerDeath();
                _gameManager.GameOver();
            }
        }
    }

    public void EnablePowerup(int powerupID) {
        switch (powerupID)
        {
            case 0:
                _tripleShotActive = true;
                break;
            case 1:
                _speedBoostActive = true;
                break;
            case 2:
                _shieldActive = true;
                _shieldVisual.SetActive(true);
                break;
            default:
                Debug.LogError("Powerup ID not found: " + powerupID);
                break;
        }
        StartCoroutine(PowerupCooldown(powerupID));
    }

    IEnumerator PowerupCooldown(int powerupID)
    {
        yield return new WaitForSeconds(5);
        switch (powerupID)
        {
            case 0:
                _tripleShotActive = false;
                break;
            case 1:
                _speedBoostActive = false;
                break;
            case 2:
                _shieldActive = false;
                _shieldVisual.SetActive(false);
                break;
            default:
                Debug.LogError("Powerup ID not found: " + powerupID);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {

        // FIX: This interaction is sloppy
        if (other.tag == "Enemy Laser" && _shieldActive == true)
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
        else if (other.tag == "Enemy Laser")
        {
            TakeDamage();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
