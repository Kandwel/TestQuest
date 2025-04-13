using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;


public class PlayerScript : MonoBehaviour
{
    [Header("������������ �������")]
    public Transform cameraTransform;  // ������� ������
    public Animator anim;  // ����������� ���������
    public Text txtHealth;  // ������ ��� ������ ��������
    public Text txtScore;  // ������ ��� ������ ���-�� ��������� �������
    public GameObject pageOnOff;  // ��������� �������
    public GameObject failMenu;  // ��������� �������� ���������
    public GameObject pauseMenu;  // ��������� �������� �����
    public GameObject startMenu;  // ��������� ��������� ��������
    private Rigidbody rb;  // ����������� ����������� ����


    [Header("������������� ���������")]
    public float speed = 4f; // �������� ���������
    public float speedRun = 10f; // �������� ��������� ��� ���������
    public float mouseSensitivity = 100f; // ���������������� ����
    public int health = 100;  //���������� ��������
    public float timeSpeed = 0f;  // ����� ���������
    private int countPage = 0;  // ���-�� ��������� �������

    public float deadTime = 3f;  // ������� ��� ������ �� ��������� ������� ����� ������

    private float xRotation = 0f;  // ������� ������ �����/����
    
    bool bonus = false;  // ��������� ������

    Vector3 newPosition = new Vector3();  // ������ ��� ��������� ����� ������� ���������
    Vector3 move = new Vector3();  // ������ ����������� ��������

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // ��������� ������ ����������� ����
        Cursor.lockState = CursorLockMode.Confined;  // ������������� �������
        pageOnOff.SetActive(false);
        failMenu.SetActive(false);
        startMenu.SetActive(true);

        Time.timeScale = 0f;

        txtHealth.text = "HP: " + health.ToString();
        txtScore.text = "������� �������: " + countPage.ToString();
    }

    void FixedUpdate()
    {
        if (health > 0)
        {
            // -------------- ������ �������� --------------
            if (timeSpeed > 0)
            {
                timeSpeed -= Time.deltaTime;  // �������� ������ ������� �������� ������
                bonus = true;  // ����� �����������
            }
            else
            {
                timeSpeed = 0f;  // ��������� �������� ������
                bonus = false;  // ����� �������������
            }
            // ---------------------------------------------

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

            if (bonus)
                newPosition = rb.position + move.normalized * speedRun * Time.fixedDeltaTime;  // �������� ����
            else
                newPosition = rb.position + move.normalized * speed * Time.fixedDeltaTime;  // �������� ������

            rb.MovePosition(newPosition);

            // ---------------------------------------------

            // ------------ ���������� �������� ------------
            if (pageOnOff.activeSelf && Input.GetKey(KeyCode.Q))
                pageOnOff.SetActive(false);
            // ---------------------------------------------

            // ------------ �������� ��������� -------------

            if (Input.GetKey(KeyCode.W) && !bonus)
                anim.SetInteger("State", 1);  // ����� ������

            else if (Input.GetKey(KeyCode.S))
                anim.SetInteger("State", 2);  // �����

            else if (Input.GetKey(KeyCode.D))
                anim.SetInteger("State", 4);  // ������

            else if (Input.GetKey(KeyCode.A))
                anim.SetInteger("State", 5);  // �����

            else if (Input.GetKey(KeyCode.W) && bonus)
                anim.SetInteger("State", 3);  // ����� ���

            else
                anim.SetInteger("State", 0);  // ������� �� �����

            // ---------------------------------------------

            // ---------------- ���� ����� -----------------
            if (Input.GetKey(KeyCode.Escape) && !pauseMenu.activeSelf)
                PauseMenuFun(true, 0.1f);
            else if (Input.GetKey(KeyCode.Escape) && pauseMenu.activeSelf)
                PauseMenuFun(false, 1f);

            // ---------------------------------------------

        }
        else
        {
            anim.SetInteger("State", 6); // �������� ������

            deadTime -= Time.deltaTime; // ������ �� ��������� �������
            if (deadTime < 0)
                Time.timeScale = 0; // ��������� �������
            Cursor.lockState = CursorLockMode.Confined;  // ������������� �������
            failMenu.SetActive(true);
        }
    }

    // ������� ��� ��������� �����, ������������ �� ������ �������
    public void TakeDamage(int damage)
    {
        health -= damage;  // ��������� �����

        txtHealth.text = "HP: " + health.ToString();  // ����� ���������� ��������
    }

    // ���������� ��������
    public void BonusSpeed(float time)
    {
        timeSpeed = time;  // ��������� �������� �������� ������
        bonus = true;  // ��������� ������
    }

    // ��������� ������� �� ������
    public void PageUp (bool up)
    { 
        pageOnOff.SetActive(up);  // ��������� �������

        countPage++;
        txtScore.text = "������� �������: " + countPage.ToString();
    }

    // ���������� �����
    public void RestartScene()
    {
        Time.timeScale = 1f;  // ����� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // ���������� �����
    }

    public void PauseMenuFun(bool active, float timeScale)
    {
        pauseMenu.SetActive(active);
        if (active)
            Cursor.lockState = CursorLockMode.Confined;  // ������������� �������
        else
            Cursor.lockState = CursorLockMode.Locked;  // ���������� �������
        Time.timeScale = timeScale;
    }

    public void PauseMenuFunForBt()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;  // ���������� �������
        Time.timeScale = 1f;
    }

    public void StartMenuFunForBt()
    {
        startMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;  // ���������� �������
        Time.timeScale = 1f;
    }

}
