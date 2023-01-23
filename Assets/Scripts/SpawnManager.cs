using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    private bool _stopSpawning = true;
    
    
    private void Start()
    {
        _stopSpawning = false;
        StartCoroutine(EnemySpawnRoutine());
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

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
