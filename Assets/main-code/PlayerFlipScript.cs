using UnityEngine;

public class PlayerFlipScript : MonoBehaviour
{
    private float xAxis;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        xAxis = PlayerController.xAxis;

        Flip();
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }
}
