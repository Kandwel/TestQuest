using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("������� ��� ������")]
    public GameObject[] prefabs;

    [Header("������ �������� (X,Z)")]
    public Vector2 areaSize = new Vector2(10f, 10f);

    [Header("�������� ������")]
    public float startSpawnDelay = 2f;       // ��������� �������� ����� ��������
    public float minSpawnDelay = 0.3f;       // ����������� �������� ����� ��������
    public float spawnAcceleration = 0.01f;  // ��������� ������

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

            // ����� �������
            timer = currentDelay;

            // ��������� ������
            currentDelay = Mathf.Max(minSpawnDelay, currentDelay - spawnAcceleration);
        }
    }

    // ����� �������� � ������� ����
    void SpawnObject()
    {
        if (prefabs.Length == 0) return;

        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        float randomX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float randomZ = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
        Vector3 spawnPos = new Vector3(randomX, 0f, randomZ) + transform.position;

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    // ��������� ������ �������� ��� �������� � ����������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 0.2f, areaSize.y));
    }
}
