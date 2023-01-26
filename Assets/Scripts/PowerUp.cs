using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int _powerUpSpeed = 2;
    [SerializeField] private int _powerUpID; //0 = TripleShot, 1 = Speed, 2 = Shield 
    


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
            if (player != null)
            {
                if (_powerUpID == 0)
                {
                    player.TripleShotActive();
                } 
                else if (_powerUpID == 1)
                {
                    player.SpeedBoostActive();
                } 
                else if (_powerUpID == 2)
                {
                    Debug.Log("Collected: Shields");
                }
                
            }
            Destroy(this.gameObject);
        }
    }
}
