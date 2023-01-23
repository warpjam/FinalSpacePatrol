using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _enemyPrefab;
    
    private void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (true)
        {
            Vector3 _positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(_enemyPrefab, _positionToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

}
