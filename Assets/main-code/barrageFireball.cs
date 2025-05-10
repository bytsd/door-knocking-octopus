using UnityEngine;

public class barrageFireball : MonoBehaviour
{

    [SerializeField] Vector2 startForceMinMax;
    [SerializeField] float turnSpeed = 0.5f;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4f);
        rb.AddForce(transform.right * Random.Range(startForceMinMax.x, startForceMinMax.y), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        var _dir = rb.linearVelocity;

        if(_dir != Vector2.zero)
        {
            Vector3 _frontVector = Vector3.right;

            Quaternion _targetRotation = Quaternion.FromToRotation(_frontVector, _dir - (Vector2)transform.position);
            if(_dir.x > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, turnSpeed);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, turnSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.tag == "Player")
        {
            _other.GetComponent<PlayerController>().TakeDamage(TestBoss.Instance.damage);
            Destroy(gameObject);
        }
        
    }
}
