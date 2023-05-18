using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JangoSpin : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10.0f;


    private void Update()
    {
        transform.Rotate(Vector3.back * _rotationSpeed * Time.deltaTime);
    }
}
