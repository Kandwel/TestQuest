using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Header("Подключаемые объекты")]
    public Transform cameraTransform;  // позиция камеры
    public Animator anim;  // подключение аниматора
    public TextMeshPro txtHealth;  // объект для вывода здоровья
    public GameObject pageOnOff;  // активация записки
    public GameObject hintE;  // всплывающая подсказа о взаимодействии
    private Rigidbody rb;  // определение физического тела

    [Header("Редактируемые параметры")]
    public float speed = 2.5f; // скорость персонажа
    public float mouseSensitivity = 100f; // чувствительность мыши
    public int health = 100;  //количество здоровья

    private float xRotation = 0f;  // поворот камеры вверх/вниз

    bool bonus = false;  // активация бонуса

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // получение данных физического тела
        Cursor.lockState = CursorLockMode.Locked;  // блокировка курсора

        txtHealth.text = "HP: " + health;
    }

    void Update()
    {
        // -------------- Поворот камеры --------------- 

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(0f, mouseX, 0f);

        // ---------------------------------------------

        // ----------- Перемещение персонажа -----------

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 newPosition = rb.position + move.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);

        // ---------------------------------------------

        // ------------- Анимация персонажа ------------

        if (Input.GetKey(KeyCode.W))
            anim.SetInteger("State", 1);

        else if (Input.GetKey(KeyCode.S))
            anim.SetInteger("State", 2);

        else if (Input.GetKey(KeyCode.D))
            anim.SetInteger("State", 4);

        else if (Input.GetKey(KeyCode.A))
            anim.SetInteger("State", 5);

        else if (Input.GetKey(KeyCode.W) && bonus)
            anim.SetInteger("State", 3);

        else
            anim.SetInteger("State", 0);

        // ---------------------------------------------
    }

    // функция для нанесения урона, активируемая из других классов
    public void TakeDamage(int damage)
    {
        health -= damage;

        txtHealth.text = health.ToString("HP: " + health);
    }

}
