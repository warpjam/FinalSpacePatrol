using UnityEngine;

public class TestZigZag : MonoBehaviour
{
    [SerializeField] private int _speed = 1;
    private float _sinCenterX;
    private float _xAmplitude;
    private float _xFrequency;
    private float _sinMove;
    private Vector2 _pos;
    private float _initialXPosition;

    void Start()
    {
        _sinCenterX = transform.position.x;
        _xAmplitude = Random.Range(1f, 2f);
        _xFrequency = Random.Range(1f, 2f);
        _initialXPosition = Random.Range(-7.2f, 8.9f); // Store the initial X position
        transform.position = new Vector3(_initialXPosition, 6f, 0f); // Set the initial position
    }

    void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        _pos = transform.position;
        _sinMove = Mathf.Sin(_pos.y * _xFrequency) * _xAmplitude;
        _pos.x = _sinCenterX + _sinMove + _initialXPosition; // Add the initial X position offset

        transform.position = _pos;
    }

    private void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.4f)
        {
            float _randomX = Random.Range(-11f, 11f);
            transform.position = new Vector3(_randomX, 6f, 0f);
            _initialXPosition = _randomX; // Update the initial X position
        }
    }
}