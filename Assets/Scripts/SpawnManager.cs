using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _tripleShotPowerupPrefab;
    private bool _stopSpawning = true;
    
    
    private void Start()
    {
        _stopSpawning = false;
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 _positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newEnemy = Instantiate(_enemyPrefab, _positionToSpawn, Quaternion.identity);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        //every 3-7 seconds spawn powerup
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 _positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(_tripleShotPowerupPrefab, _positionToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 11));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
