using UnityEngine;

public class PlayerFlipScript : MonoBehaviour
{
    private float xAxis;

    public bool IsFacingRight => transform.localScale.x > 0;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        xAxis = PlayerController.xAxis;

        Flip();
        
        // Debug.Log("IsFacingRight: " + IsFacingRight);
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
}
