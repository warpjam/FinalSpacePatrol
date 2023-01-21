using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerSpeed = 5;
    [SerializeField] private GameObject _laserPrefab;
    private bool _canFireLaser = true;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

    }


    void Update()
    {
        PlayerMovement();
        FireMainWeapons();
    }

    private void FireMainWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canFireLaser)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            _canFireLaser = false;
            StartCoroutine(ReloadLaserTimer());
        }
    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _playerSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.0f, 5.0f), 0);

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    IEnumerator ReloadLaserTimer()
    {
        yield return new WaitForSeconds(0.5f);
        _canFireLaser = true;
    }
}
