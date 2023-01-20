using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerSpeed = 5;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

    }


    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _playerSpeed * Time.deltaTime);

        if (transform.position.y >= 5.7f)
        {
            
        }
    }
}
