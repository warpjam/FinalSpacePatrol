using UnityEngine;

public class BackLaser : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;

    void Update()
    {
        MoveUp();
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 7.3f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}