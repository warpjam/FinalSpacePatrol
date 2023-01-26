using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int _powerUpSpeed = 2;
    


    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            player.TripleShotActive();
            Destroy(this.gameObject);
        }
    }
}
