using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private Vector2 moveDirection;

    private void Awake()
    {
        moveDirection = (Random.Range(-1f,1f) * Vector2.up) + (Random.Range(-1f, 1f) * Vector2.right);
        moveDirection = moveDirection.normalized;
    }


    private void FixedUpdate()
    {
        transform.position += (Vector3)(_moveSpeed * Time.fixedDeltaTime * moveDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Side"))
        {
            moveDirection.x *= -1f;
        }

        if (collision.CompareTag("Top"))
        {
            moveDirection.y *= -1f;
        }
    }
}
