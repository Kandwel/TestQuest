using UnityEngine;

//Скрипт поднятия ускорения
public class SpeedBonus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            player.BonusSpeed(5f);

            Destroy(gameObject);
        }
    }
}
