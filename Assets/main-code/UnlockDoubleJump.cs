using UnityEngine;

public class UnlockDoubleJump : MonoBehaviour
{
    private bool used = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!used && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddExtraAirJump();
                used = true;
                Destroy(gameObject);
            }
        }
    }
}
