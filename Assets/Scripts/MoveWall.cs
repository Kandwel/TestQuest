using UnityEngine;

// —крипт подн€ти€ стены при подходе игрока на определЄнное рассто€ние до кра€
public class MoveWall : MonoBehaviour
{
    [Header("Ёлементы дл€ передвижени€ стены")]
    public Transform wall;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public Transform endPositionObj;
    public float speedUp = 15f;

    private bool moveWall = false;

    private void Start()
    {
        startPosition = wall.position;
        endPosition = endPositionObj.position;
    }

    private void Update()
    {
        if (moveWall)
            wall.position = Vector3.MoveTowards(wall.position, endPosition, speedUp * Time.deltaTime);
        else
            wall.position = Vector3.MoveTowards(wall.position, startPosition, speedUp * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerWall"))
            moveWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerWall"))
            moveWall = false;
    }
}
