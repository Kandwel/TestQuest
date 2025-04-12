using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Header("������������ �������")]
    public Transform cameraTransform;  // ������� ������
    public Animator anim;  // ����������� ���������
    public Text txtHealth;  // ������ ��� ������ ��������
    public GameObject pageOnOff;  // ��������� �������
    private Rigidbody rb;  // ����������� ����������� ����

    [Header("������������� ���������")]
    public float speed = 4f; // �������� ���������
    public float speedRun = 10f; // �������� ��������� ��� ���������
    public float mouseSensitivity = 100f; // ���������������� ����
    public int health = 100;  //���������� ��������
    public float timeSpeed = 0f;  // ����� ���������

    private float xRotation = 0f;  // ������� ������ �����/����
    
    bool bonus = false;  // ��������� ������

    Vector3 newPosition = new Vector3();
    Vector3 move = new Vector3();

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // ��������� ������ ����������� ����
        Cursor.lockState = CursorLockMode.Locked;  // ���������� �������

        txtHealth.text = "HP: " + health.ToString();
    }

    void FixedUpdate()
    {
        // -------------- ������� ������ --------------- 

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(0f, mouseX, 0f);

        // ---------------------------------------------

        // ----------- ����������� ��������� -----------

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        move = transform.right * moveX + transform.forward * moveZ;

        // -------------- ������ �������� --------------
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

        // ------------ ���������� �������� ------------
        if (pageOnOff.activeSelf && Input.GetKey(KeyCode.Q))
            PageUp(false);
        // ---------------------------------------------

        // ------------ �������� ��������� -------------

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

    // ������� ��� ��������� �����, ������������ �� ������ �������
    public void TakeDamage(int damage)
    {
        health -= damage;

        txtHealth.text = "HP: " + health.ToString();
    }

    // ���������� ��������
    public void BonusSpeed(float time)
    {
        timeSpeed = time;
        bonus = true;
    }

    // ��������� ������� �� ������
    public void PageUp (bool up)
    { 
        pageOnOff.SetActive(up);
    }
}
