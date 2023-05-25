using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammingEnemy : Enemy
{
    [SerializeField] private float _ramDistance = 5f;
    [SerializeField] private float _ramSpeed = 6f;

    private Player _player;

    protected override void Start()
    {
        base.Start();
        _player = GameObject.FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is not found!");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_player != null && Vector3.Distance(transform.position, _player.transform.position) <= _ramDistance)
        {
            RamPlayer();
        }
    }

    private void RamPlayer()
    {
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        transform.Translate(direction * _ramSpeed * Time.deltaTime);
    }
}
