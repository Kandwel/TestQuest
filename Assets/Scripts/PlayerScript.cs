using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;


public class PlayerScript : MonoBehaviour
{
    [Header("Подключаемые объекты")]
    public Transform cameraTransform;  // позиция камеры
    public Animator anim;  // подключение аниматора
    public Text txtHealth;  // объект для вывода здоровья
    public Text txtScore;  // объект для вывода кол-ва собранных записок
    public GameObject pageOnOff;  // активация записки
    public GameObject failMenu;  // активация страницы поражения
    public GameObject pauseMenu;  // активация страницы паузы
    public GameObject startMenu;  // активация стартовой страницы
    private Rigidbody rb;  // определение физического тела


    [Header("Редактируемые параметры")]
    public float speed = 4f; // скорость персонажа
    public float speedRun = 10f; // скорость персонажа при ускорении
    public float mouseSensitivity = 100f; // чувствительность мыши
    public int health = 100;  //количество здоровья
    public float timeSpeed = 0f;  // время ускорения
    private int countPage = 0;  // кол-во собранных записок

    public float deadTime = 3f;  // счётчик для отсчёт до остановки времени после смерти

    private float xRotation = 0f;  // поворот камеры вверх/вниз
    
    bool bonus = false;  // активация бонуса

    Vector3 newPosition = new Vector3();  // вектор для установки новой позиции персонажа
    Vector3 move = new Vector3();  // вектор направления движения

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // получение данных физического тела
        Cursor.lockState = CursorLockMode.Confined;  // разблокировка курсора
        pageOnOff.SetActive(false);
        failMenu.SetActive(false);
        startMenu.SetActive(true);

        Time.timeScale = 0f;

        txtHealth.text = "HP: " + health.ToString();
        txtScore.text = "Собрано записок: " + countPage.ToString();
    }

    void FixedUpdate()
    {
        if (health > 0)
        {
            // -------------- Таймер скорости --------------
            if (timeSpeed > 0)
            {
                timeSpeed -= Time.deltaTime;  // обратный отсчёт времени действия бонуса
                bonus = true;  // бонус активирован
            }
            else
            {
                timeSpeed = 0f;  // окончание действия бонуса
                bonus = false;  // бонус деактивирован
            }
            // ---------------------------------------------

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

            if (bonus)
                newPosition = rb.position + move.normalized * speedRun * Time.fixedDeltaTime;  // скорость бега
            else
                newPosition = rb.position + move.normalized * speed * Time.fixedDeltaTime;  // скорость ходьбы

            rb.MovePosition(newPosition);

            // ---------------------------------------------

            // ------------ Выключение страницы ------------
            if (pageOnOff.activeSelf && Input.GetKey(KeyCode.Q))
                pageOnOff.SetActive(false);
            // ---------------------------------------------

            // ------------ Анимация персонажа -------------

            if (Input.GetKey(KeyCode.W) && !bonus)
                anim.SetInteger("State", 1);  // вперёд ходьба

            else if (Input.GetKey(KeyCode.S))
                anim.SetInteger("State", 2);  // назад

            else if (Input.GetKey(KeyCode.D))
                anim.SetInteger("State", 4);  // вправо

            else if (Input.GetKey(KeyCode.A))
                anim.SetInteger("State", 5);  // влево

            else if (Input.GetKey(KeyCode.W) && bonus)
                anim.SetInteger("State", 3);  // вперёд бег

            else
                anim.SetInteger("State", 0);  // простой на месте

            // ---------------------------------------------

            // ---------------- Меню паузы -----------------
            if (Input.GetKey(KeyCode.Escape) && !pauseMenu.activeSelf)
                PauseMenuFun(true, 0.1f);
            else if (Input.GetKey(KeyCode.Escape) && pauseMenu.activeSelf)
                PauseMenuFun(false, 1f);

            // ---------------------------------------------

        }
        else
        {
            anim.SetInteger("State", 6); // анимация смерти

            deadTime -= Time.deltaTime; // отсчёт до остановки времени
            if (deadTime < 0)
                Time.timeScale = 0; // остановка времени
            Cursor.lockState = CursorLockMode.Confined;  // разблокировка курсора
            failMenu.SetActive(true);
        }
    }

    // функция для нанесения урона, активируемая из других классов
    public void TakeDamage(int damage)
    {
        health -= damage;  // нанесение урона

        txtHealth.text = "HP: " + health.ToString();  // вывод количества здоровья
    }

    // увеличение скорости
    public void BonusSpeed(float time)
    {
        timeSpeed = time;  // установка счётчика действия бонуса
        bonus = true;  // активация бонуса
    }

    // активация записки на экране
    public void PageUp (bool up)
    { 
        pageOnOff.SetActive(up);  // активация записки

        countPage++;
        txtScore.text = "Собрано записок: " + countPage.ToString();
    }

    // перезапуск сцены
    public void RestartScene()
    {
        Time.timeScale = 1f;  // сброс паузы
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // перезапуск сцены
    }

    public void PauseMenuFun(bool active, float timeScale)
    {
        pauseMenu.SetActive(active);
        if (active)
            Cursor.lockState = CursorLockMode.Confined;  // разблокировка курсора
        else
            Cursor.lockState = CursorLockMode.Locked;  // блокировка курсора
        Time.timeScale = timeScale;
    }

    public void PauseMenuFunForBt()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;  // блокировка курсора
        Time.timeScale = 1f;
    }

    public void StartMenuFunForBt()
    {
        startMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;  // блокировка курсора
        Time.timeScale = 1f;
    }

}
