using System.Collections;
using UnityEngine;
using System;

//[RequireComponent(typeof(KnockBack))]

public class Player : MonoBehaviour
{

    //ссылочные объекты ....................................................................................................
    private KnockBack _knockBack; //скрипт отталкивания
    private Rigidbody2D rb; //создаем переменную для храненния Rigidbody2D
    private Camera _mainCamera;

    //поля ..................................................................................................................
    [SerializeField] private float movingSpeed = 1f; //переменная для изменения скорости
    [SerializeField] private int maxHealth = 100; //максимальное здоровье
    [SerializeField] private float damageRecoveryTime = 0.5f; //ограничение частоты получения урона 



    private float _minMovingSpeed = 0.1f; //хранит нижнее значения скорости, если скорость объекта ниже - считаем что он стоит
    private bool _isRunning = false; //флаг - бежит или нет
    Vector2 _inputVector;//вектор движения
    private int _currentHealth; //текущее здоровье
    private bool _canTakeDamage; //флаг - возможность получения урона
    private bool _isAlive; //флаг - живой 




    //свойстйства .............................................................................................................................
    //патерн одиночка
    public static Player Instance { get; private set; }


    //события ...................................................................................................................
    public event EventHandler OnPlayerDeath;//событие смерти персонажа
    public event EventHandler OnFlashBlink; //событие мигания




    //Live cycle методы ..........................................................................................................................................
    private void Awake()//выполняется до Start, инициализируем нужные переменные
    {
        Instance = this; //инициализируем стат.переменную. объектом который ее вызвал 
        rb = GetComponent<Rigidbody2D>();//записываем ссылку на Rigidbody2D, объекта
                                         //на котором висит скрипт
        _knockBack = GetComponent<KnockBack>(); //проинициализировали ссылкой на скрипт 
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _isAlive = true;
        _canTakeDamage = true; //флаг получения урона активен
        _currentHealth = maxHealth;
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack; //подписываемся на событие атаки привязанное к клавиши мыши
    }

    private void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVector();//получаем Vector2 из функции GetMovementVector
        // которая вынесена в класс GameInput - класс одиночка-паттерн
    }

    private void FixedUpdate() //метод вызывается через равные промежутки времени ()
    {
        if (_knockBack.IsGettingRnockBack) return; //если находимся в состоянии отлета, прерывем обычную логику

        HandleMovement();
    }

    private void OnDestroy()
    {
        if (GameInput.Instance != null) { GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack; }

    }


    //публичные методы ...........................................................................................................
    public void TakeDamage(Transform damageSource, int damage) //получение урона
    {
        if (_canTakeDamage && _isAlive) 
        {
            _canTakeDamage = false; //флаг получения урона - деактивирован
            _currentHealth = Mathf.Max(0, _currentHealth -= damage); // Mathf.Max - возвращает большее из двух значений
            _knockBack.GetKnockedBack(damageSource);

            OnFlashBlink?.Invoke(this, EventArgs.Empty); //запуск события мигания 


            StartCoroutine(DamageRecoveryRoutine());//запускаем выполнение корутины;
        }

        DetectDeath();
    }

    public bool IsRunning() { return _isRunning; }


    public Vector2 GetPlayerScreenPosition()
    {
        return _mainCamera.WorldToScreenPoint(transform.position); //считываем положение героя

        //return Camera.main.WorldToScreenPoint(transform.position); //считываем положение героя
        //через камеру, которая привязана к нему, относительно экрана
    }

    public bool IsAlive() { return _isAlive; }


    //приватные методы ...............................................................................................................

    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopRnockBackMovement();
            GameInput.Instance.DisableMovement(); //отключаем считывание ввода 

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRoutine() //корутина - отложенное выполннение 
    {
        yield return new WaitForSeconds(damageRecoveryTime); //останавливаем выполнение данного метода на время _damageRecoveryTime без блокировки основного потока
        _canTakeDamage = true;
    }


    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e) //метод для атаки (сработает при нажатии левой кл.мыши)
        //параметры передадуться из Invoke определенном в PlayerAttac_started класса GameInput
    {
        ActiveWeapon.Instance.GetActiveWeapon().AttacK();
    }



    private void HandleMovement()
    {
        

        rb.MovePosition(rb.position + _inputVector * Time.fixedDeltaTime * movingSpeed); //перемещение объекта

        if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || Mathf.Abs(_inputVector.y) > _minMovingSpeed) //контроль флага движения
        {
            _isRunning = true;
        }
        else { _isRunning = false; }
    }



   

    //первый вариант.....................................................................................................
    /*

    private void Awake()//выполняется до Start, инициализируем нужные переменные
    {
        rb = GetComponent<Rigidbody2D>();//записываем ссылку на Rigidbody2D, объекта
                                         //на котором висит скрипт
    }


private void FixedUpdate() //метод вызывается через равные промежутки времени ()
    {

        Vector2 inputVector = new Vector2(0, 0); //объявлеяем сдесь, что бы не было аккамулирующего эффекта

        if (Input.GetKey(KeyCode.W)) { inputVector.y = 1f; } //KeyCode.W - объект enum
        //Input.GetKey() - true если зажата кнопка переданная в параметры ("" | enum);
        if (Input.GetKey(KeyCode.S)) { inputVector.y = -1f; }
        if (Input.GetKey(KeyCode.A)) { inputVector.x = -1f; }
        if (Input.GetKey(KeyCode.D)) { inputVector.x = 1f; }

        inputVector = inputVector.normalized; //нормализует движение по диагонали, длина вектора
        //не будет больше 1


        rb.MovePosition(rb.position + inputVector * Time.fixedDeltaTime*speed); 
        //функция передвижения (принимает вектор куда переместиться)
        //Time.fixedDeltaTime - фиксированное время, с которой обнавляется метод FixUpdate()
       



    }

     */
    //конец первого варианта ...........................................................................................................



}
