using UnityEngine;

// Скрипт подбора записки
public class PageScript : MonoBehaviour
{    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            player.PageUp(true);

            Destroy(gameObject);
        }
    }
}
