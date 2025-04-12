using UnityEngine;

// Скрипт нанесения урона от стены
public class LaserDamage : MonoBehaviour
{
    private bool enterPlayer = false;
    private float time = 0f;
    private PlayerScript player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerScript>();
            enterPlayer = true;
        }    
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enterPlayer = false;
    }

    private void Update()
    {
        if (enterPlayer)
        {
            time -= Time.deltaTime;
            if (time <= 0f)
            {
                player.TakeDamage(5);
                time = 1f;
            }            
        }

    }
}
