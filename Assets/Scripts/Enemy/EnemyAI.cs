using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using UnityEngine.EventSystems;
using System;

public class EnemyAI : MonoBehaviour
{
    //константы.........................................................................................................................
    private enum State //константы состояний 
    {
        Idle, //покой
        Roaming, //брожение
        Chasing, //погоня
        Attacking,
        Death
    }

    //поля ............................................................................................................................

    [SerializeField] private State _startingState; //стартовое состояние 
    [SerializeField] private float _roamingDistanceMax = 7f; //минимальное растояние на которое может отходить объект
    [SerializeField] private float _roamingDistanceMin = 3f; //максимальное растояние на которое может отходить объект
    [SerializeField] private float _roaminTimerMax = 2f; //время в течении которого он будет двигаться (обнавлять цели)
    [SerializeField] private float _chasingDistance = 4f; //дистанция с которой начинается преследование
    [SerializeField] private float _chasingSpeedMultiplier = 2f; //увеличение скороти при погоне 
    [SerializeField] private float _attackingDistance = 2f; //дистанция с которой начинается атака
    [SerializeField] private float _attackRate = 2f; //частота атаки

    [SerializeField] private bool _isChasingEnemy = false; //флаг - занимаеться ли враг погоней (или он лучник)
    [SerializeField] private bool _isAttackingEnemy = false; //флаг - являеться ли враг атакующим;


    private NavMeshAgent _navMeshAgent; // хранит ссылку на NavMeshAgent 
    private State _currentState; //хранит текущее состояние объекта
    private float _roamingTimer; //хранит количество времени, которое бродит объект
    private Vector3 _roamPosition; //координаты цели объекта
    private Vector3 _startingPosition; //хранит координаты стартовой позиции


   

    private float _roamingSpeed; //хранит скоротсь врага
    private float _chasingSpeed; // скорость врага при погоне
    private float _nextAttackTime = 0f; //момент времени следующей атаки


    private float _nextCheckDirectionTime = 0f; //время в которое хотим проверить правильно ли мы движемся
    private float _checkDirectionDuration = 0.1f; // частота проверок 
    private Vector3 _lastPosition; //последнее положение врага



    //свойства.........................................................................................................................
    public bool IsRunning => _navMeshAgent.velocity != Vector3.zero; //двигаеться ли враг 

    public float GetRoamingAnimationSpeed //получение текущей скорости 
    {
        get { return _navMeshAgent.speed / _roamingSpeed; }
    }



    //события.........................................................................................................................
    public event EventHandler OnEnemyAttack;

    //святая троица.........................................................................................................................
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>(); //инициализируем объектом NavMeshAgent с того на ком висит скрипт
        _navMeshAgent.updateRotation = false; //запрещаем вращение нашего объекта 
        _navMeshAgent.updateUpAxis = false; //что бы ориентация NavMesh не влияла на ориентацию нашего объекта 
        _currentState = _startingState; //задаем начальное состояние 

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;



    }

    //раскоментировать для приявзки врагов к изначальной точке
    //private void Start()
    //{
    //    startingPosition = transform.position; // задаем изначальную позицию
    //}

    private void Update()
    {
        StateHandler(); //машина состояний
        MovementDirectionHandler(); //проверка ориентации положения врага
    }

    //методы публичные .......................................................................................................

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }



    //методы приватные ..................................................................................

    private void StateHandler()//машина состояний..................................
    {
        switch (_currentState)
        {

            case State.Roaming: //состояние брожения
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming(); //переходим в брожение 
                    _roamingTimer = _roaminTimerMax; //обновляем таймер
                }
                CheckCurrenState(); // обновляем состояние
                break;

            case State.Chasing:
                ChasingTarget();//состояние погони
                CheckCurrenState(); // обновляем состояние
                break;

            case State.Attacking:
                AttackingTarget();
                CheckCurrenState(); // обновляем состояние
                break;

            case State.Death: break;
            default:
            case State.Idle: break;
        }


    }


    private void Roaming() // реализует логику брожения
    {
        _startingPosition = transform.position; //если убрать данную строчку враг будет ходить возле изначальной точки спавна
        _roamPosition = GetRoamingPosition(); //сохраняем новую точку цели
        //ChangeFacingDirection(_startingPosition, _roamPosition); //ориентируем спрайт врага по вектору движения
        _navMeshAgent.SetDestination(_roamPosition);//запускаем движение к точке
    }

    private void ChasingTarget() //переход в состояние погони
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position); //устанавливаем точку назначения (враг гониться за героем) 
    }

    private void AttackingTarget() //переход в состояние атаки
    {
        if (Time.time > _nextAttackTime) //если текущее время больше _nextAttackTime, нужно для того что бы 
            //враг не совершал аттаки с частотой обнавления кадров, т.к. данный метод будет внутри Update
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty); // запускаем собыите аттаки 
            _nextAttackTime = Time.time + _attackRate; //прибавляем к текущему времени _attackRate, что бы получить время срабатывания следующей атаки  
        } 
    } 


    private void CheckCurrenState() // проверка текущего состояния
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position); //Vector3.Distance() - расчитает расстояние между двумя векторами
        State newState = State.Roaming; //по умолчанию бродит


        if (_isChasingEnemy) //если активен флаг погони (враг поддерживает брожение)
        {
            if (distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            } 
        }

        if (_isAttackingEnemy) //если активен флаг атаки (враг поддерживает атаку ББ)
        {
            if (distanceToPlayer <= _attackingDistance)
            {
                if (Player.Instance.IsAlive())
                {
                    newState = State.Attacking;
                }
                else { newState = State.Roaming; }
            } 
        }


        if (newState != _currentState) //если текущее состояние отличаеться от нового
        {
            if (newState == State.Chasing) //если переключились на состояние погони
            {
                _navMeshAgent.ResetPath(); //сбрасываем предыдущую цель передвижения, что бы потом установить целью игрока 
                _navMeshAgent.speed = _chasingSpeed; //устанавливаем скороть для погони
            }
            else if (newState == State.Roaming) //если перешли в состояние брожения
            {
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed; //устанавливаем скороть для погони
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath(); //сбрасываем предыдущую цель передвижения, что бы он перестал идти 

            }
                _currentState = newState; //устанавливем текущее состояние
        }


           
 
    }


    private Vector3 GetRoamingPosition() //поиск новой точки (рандомная точка)
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }


    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition) //метод для разворота спрайта врага 
    {
        if (sourcePosition.x > targetPosition.x) // если текущая позиция правее чем целевая, разворачиваем спрайт
        {
            transform.rotation = Quaternion.Euler(0, -180, 0); //поворот на 180 градусов
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); //возврат в исходное положение
        }
    }


    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime) {
            if (IsRunning)
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);
            }
            _lastPosition = transform.position; 
            _nextCheckDirectionTime =Time.time + _checkDirectionDuration;
        }
    }




    



   


}
