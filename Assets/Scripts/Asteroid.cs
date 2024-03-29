using System;
using Unity.VisualScripting;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 20.0f;
    [SerializeField] private GameObject _explosion;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;


    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is Null");
        }
        
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is Null");
        }
    }

    void Update()
    {
      transform.Rotate(Vector3.back * _rotationSpeed * Time.deltaTime);  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser") || other.CompareTag("PlayerMissile"))
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);
            _uiManager.ControlsOff();
            _spawnManager.StartSpawning();
        }
    }
}
