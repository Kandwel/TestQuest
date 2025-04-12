using UnityEngine;

// скрипт получения урона 
public class DamageScripts : MonoBehaviour
{
    public float speedSphere = 10f;
    private Vector3 playerPosition;
    private Vector3 moveSphere;
    private float lifeTime = 40f;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            moveSphere = (playerPosition - transform.position).normalized;
        }
    }

    private void Update()
    {
        transform.position += moveSphere * speedSphere * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0 )
            Destroy(gameObject);
    }

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
