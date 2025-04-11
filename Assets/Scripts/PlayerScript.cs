using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 2.5f; // скорость персонажа
    public float mouseSensitivity = 100f; // чувствительность мыши

    public Transform cameraTransform;  // позиция камеры

    private Rigidbody rb;  // определение физического тела
    private float xRotation = 0f;  // поворот камеры вверх/вниз

    public Animator anim;  // подключение аниматора

    bool bonus = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // получение данных физического тела
        Cursor.lockState = CursorLockMode.Locked;  // блокировка курсора
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
        Vector3 newPosition = rb.position + move * speed * Time.fixedDeltaTime;

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
}
