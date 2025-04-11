using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 2.5f; // �������� ���������
    public float mouseSensitivity = 100f; // ���������������� ����

    public Transform cameraTransform;  // ������� ������

    private Rigidbody rb;  // ����������� ����������� ����
    private float xRotation = 0f;  // ������� ������ �����/����

    public Animator anim;  // ����������� ���������

    bool bonus = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // ��������� ������ ����������� ����
        Cursor.lockState = CursorLockMode.Locked;  // ���������� �������
    }

    void Update()
    {
        // -------------- ������� ������ --------------- 

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(0f, mouseX, 0f);

        // ---------------------------------------------

        // ----------- ����������� ��������� -----------

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 newPosition = rb.position + move * speed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);

        // ---------------------------------------------

        // ------------- �������� ��������� ------------

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
