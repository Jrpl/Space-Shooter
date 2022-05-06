using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Enemies
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    // Asteroids
    [SerializeField]
    private GameObject _asteroidPrefab;
    [SerializeField]
    private GameObject _asteroidContainer;

    // Powerups
    [SerializeField]
    private GameObject _tripleShotPowerupPrefab;
    [SerializeField]
    private GameObject _speedBoostPowerupPrefab;
    [SerializeField]
    private GameObject _shieldPowerupPrefab;
    [SerializeField]
    private GameObject _powerupContainer;

    // Settings
    private bool _stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
        StartCoroutine(SpawnAsteroid());
    }

    IEnumerator SpawnEnemy()
    {
        while(_stopSpawning == false)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(-9f, 9f), 7.1f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, SpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnAsteroid()
    {
        while(_stopSpawning == false)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(-9f, 9f), 8.1f, 0);
            GameObject newAsteroid = Instantiate(_asteroidPrefab, SpawnPos, Quaternion.identity);
            newAsteroid.transform.parent = _asteroidContainer.transform;
            yield return new WaitForSeconds(7);
        }
    }

    IEnumerator SpawnPowerup() 
    {
        while(_stopSpawning == false) 
        {
            Vector3 SpawnPos = new Vector3(Random.Range(-9f, 9f), 7.3f, 0);
            GameObject newPowerup = Instantiate(RandomPowerup(), SpawnPos, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    GameObject RandomPowerup()
    {
        int powerupID = Random.Range(0, 3);
        switch(powerupID)
        {
            case 0:
                return _tripleShotPowerupPrefab;
            case 1:
                return _speedBoostPowerupPrefab;
            case 2:
                return _shieldPowerupPrefab;
            default:
                return _shieldPowerupPrefab;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
