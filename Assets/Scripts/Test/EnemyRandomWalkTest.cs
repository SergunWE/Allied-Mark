using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyRandomWalkTest : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float walkTime = 15f;

    private Rigidbody _rb;
    private float _time;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();

        _time += Time.deltaTime;
        if (!(_time > walkTime)) return;
        _time = 0;
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rotate();
    }
    
    private void Move()
    {
        var movement = transform.forward * (speed * Time.deltaTime);
        _rb.MovePosition(_rb.position + movement);
    }
    
    private void Rotate()
    {
        transform.Rotate(Vector3.up, Random.Range(0, 180));
    }
}