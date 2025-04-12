using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("Префабы для спавна")]
    public GameObject[] prefabs;

    [Header("Размер площадки (X,Z)")]
    public Vector2 areaSize = new Vector2(10f, 10f);

    [Header("Интервал спавна")]
    public float startSpawnDelay = 2f;       // начальная задержка между спавнами
    public float minSpawnDelay = 0.3f;       // минимальная задержка между спавнами
    public float spawnAcceleration = 0.01f;  // ускорение спавна

    private float currentDelay;
    private float timer;

    private void Start()
    {
        currentDelay = startSpawnDelay;
        timer = currentDelay;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnObject();

            // Сброс таймера
            timer = currentDelay;

            // Ускорение спавна
            currentDelay = Mathf.Max(minSpawnDelay, currentDelay - spawnAcceleration);
        }
    }

    // Спавн объектов в заданом поле
    void SpawnObject()
    {
        if (prefabs.Length == 0) return;

        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        float randomX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float randomZ = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
        Vector3 spawnPos = new Vector3(randomX, 0f, randomZ) + transform.position;

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    // Очертание границ спавнера для удобства в размещении
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 0.2f, areaSize.y));
    }
}
