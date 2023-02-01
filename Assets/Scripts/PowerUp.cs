using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _powerUpSpeed = 2;
    [SerializeField] private int _powerUpID; //0 = TripleShot, 1 = Speed, 2 = Shield 
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _powerUpSound;
    
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
        if (transform.position.y < -7.5f)
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
                switch (_powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsUp();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);
            Destroy(this.gameObject);
        }
    }
}
