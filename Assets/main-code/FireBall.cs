using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float hitForce;
    [SerializeField] float speed;
    [SerializeField] float lifeTime = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.position += speed * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.tag == "Enemy")
        {
            _other.GetComponent<Enemy>().EnemyGetsHit(damage, (_other.transform.position - transform.position).normalized, -hitForce);
        }
    }
}
