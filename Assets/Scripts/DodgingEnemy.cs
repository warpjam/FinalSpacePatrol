using UnityEngine;

public class DodgingEnemy : Enemy
{
    [SerializeField] private float _dodgeDistance = 5.0f;

    // Optional: you could randomize dodge direction so it's not always moving to the same side.
    private bool _dodgeLeft = true;

    protected override void Start()
    {
        base.Start();
        // Randomly decide if this enemy should dodge left or right when it is time to dodge.
        _dodgeLeft = Random.value < 0.5f;
    }

    // This method will be called by the Laser script when it detects a DodgingEnemy in its path.
    public void Dodge()
    {
        // Dodges by teleporting enemy along the x-axis.
        // It can dodge to left or right depending on _dodgeLeft.
        float dodgeDirection = _dodgeLeft ? -1 : 1;
        Vector3 dodgeVector = new Vector3(dodgeDirection * _dodgeDistance, 0, 0);
        transform.position += dodgeVector;

        // After dodging, it should dodge to the other direction next time.
        _dodgeLeft = !_dodgeLeft;
    }
}