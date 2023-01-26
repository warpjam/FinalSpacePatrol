using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _laserSpeed = 6;


    void Update()
    {
        MoveUp();
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y > 7.3f)
        {
            Destroy(gameObject);
        }
    }
}
