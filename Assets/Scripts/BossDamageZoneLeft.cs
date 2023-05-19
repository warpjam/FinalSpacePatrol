using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageZoneLeft : MonoBehaviour
{
    [SerializeField] private float _damageMultiplier = 2;
    private FinalBoss _boss;

    void Start()
    {
        _boss = GetComponentInParent<FinalBoss>();
        if (_boss == null)
        {
            Debug.Log("Boss component not found on parent of " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other.gameObject);
    }

    private void OnHit(GameObject hitObject)
    {
        if (hitObject.CompareTag("PlayerMissile") || hitObject.CompareTag("UniBeam"))
        {
            _boss.TakeDamage(5 * _damageMultiplier, hitObject.transform.position);
        }
        else if (hitObject.CompareTag("Laser"))
        {
            _boss.TakeDamage(2 * _damageMultiplier, hitObject.transform.position);
        }
        Destroy(hitObject, 0.5f);
    }
}
