using UnityEngine;

public class DamageScripts : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            player.TakeDamage(25);

            Destroy(gameObject);
        }
    }
}
