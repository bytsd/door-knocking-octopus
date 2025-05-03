using UnityEngine;

public class PlayerFlipScript : MonoBehaviour
{
    private float xAxis;
    public PlayerStateList pState;

    public bool IsFacingRight => transform.localScale.x > 0;


    void Start()
    {
        pState = GetComponent<PlayerStateList>();
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
            pState.lookingRight = false;
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            pState.lookingRight = true;
        }
    }
}
