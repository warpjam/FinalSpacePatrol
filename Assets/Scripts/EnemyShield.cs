using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private GameObject _shield;
    [SerializeField] private float _shieldActivationProbability = 0.1f; // 10% chance by default

    private void Start()
    {
        ActivateShields();
    }

    private void ActivateShields()
    {
        if (_shield != null && Random.value < _shieldActivationProbability)
        {
            _shield.SetActive(true);
        }
    }

    public bool IsShieldActive()
    {
        return _shield != null && _shield.activeInHierarchy;
    }

    public void DeactivateShield()
    {
        if (_shield != null)
        {
            _shield.SetActive(false);
        }
    }
}