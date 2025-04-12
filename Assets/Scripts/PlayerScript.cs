using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Header("Подключаемые объекты")]
    public Transform cameraTransform;  // позиция камеры
    public Animator anim;  // подключение аниматора
    public Text txtHealth;  // объект для вывода здоровья
    public GameObject pageOnOff;  // активация записки
    private Rigidbody rb;  // определение физического тела

    [Header("Редактируемые параметры")]
    public float speed = 4f; // скорость персонажа
    public float speedRun = 10f; // скорость персонажа при ускорении
    public float mouseSensitivity = 100f; // чувствительность мыши
    public int health = 100;  //количество здоровья
    public float timeSpeed = 0f;  // время ускорения

    private float xRotation = 0f;  // поворот камеры вверх/вниз
    
    bool bonus = false;  // активация бонуса

    Vector3 newPosition = new Vector3();
    Vector3 move = new Vector3();

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // получение данных физического тела
        Cursor.lockState = CursorLockMode.Locked;  // блокировка курсора

        txtHealth.text = "HP: " + health.ToString();
    }

    void FixedUpdate()
    {
        // -------------- Поворот камеры --------------- 

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(0f, mouseX, 0f);

        // ---------------------------------------------

        // ----------- Перемещение персонажа -----------

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        move = transform.right * moveX + transform.forward * moveZ;

        // -------------- Таймер скорости --------------
        if (timeSpeed > 0)
        {
            timeSpeed -= Time.deltaTime;
            bonus = true;
        }
        else
        {
            timeSpeed = 0f;
            bonus = false;
        }
        // ---------------------------------------------

        if (bonus == true)
            newPosition = rb.position + move.normalized * speedRun * Time.fixedDeltaTime;            
        else
            newPosition = rb.position + move.normalized * speed * Time.fixedDeltaTime;        

        rb.MovePosition(newPosition);

        // ---------------------------------------------

        // ------------ Выключение страницы ------------
        if (pageOnOff.activeSelf && Input.GetKey(KeyCode.Q))
            PageUp(false);
        // ---------------------------------------------

        // ------------ Анимация персонажа -------------

        if (Input.GetKey(KeyCode.W) && bonus == false)
            anim.SetInteger("State", 1);

        else if (Input.GetKey(KeyCode.S))
            anim.SetInteger("State", 2);

        else if (Input.GetKey(KeyCode.D))
            anim.SetInteger("State", 4);

        else if (Input.GetKey(KeyCode.A))
            anim.SetInteger("State", 5);

        else if (Input.GetKey(KeyCode.W) && bonus == true)
            anim.SetInteger("State", 3);

        else
            anim.SetInteger("State", 0);

        // ---------------------------------------------
    }

    // функция для нанесения урона, активируемая из других классов
    public void TakeDamage(int damage)
    {
        health -= damage;

        txtHealth.text = "HP: " + health.ToString();
    }

    // увеличение скорости
    public void BonusSpeed(float time)
    {
        timeSpeed = time;
        bonus = true;
    }

    // активация записки на экране
    public void PageUp (bool up)
    { 
        pageOnOff.SetActive(up);
    }
}
