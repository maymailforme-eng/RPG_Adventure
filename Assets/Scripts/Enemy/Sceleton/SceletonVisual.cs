using UnityEngine;




[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class SceletonVisual : MonoBehaviour
{
    //ссылочные объекты
    [SerializeField] private EnemyAI _enemyAI; //переменная логику движения врага
    [SerializeField] private EnemyEntity _enemyEntity;//переменная под сущность врага
    [SerializeField] private GameObject _enemyShadow; //объект под тень 

    private SpriteRenderer _spriteRenderer; //ссылка на текщий SpriteRenderer




    //поля ..................................................................................................
    private const string IS_RUNNING = "isRunning"; // название параметра в Animator
    private const string CHASING_SPEED_MULTIPLIER = "CasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string TAKE_HIT = "TakeHit";
    private const string IS_DIE = "IsDie";


    private Animator _animator; //переменная под Аниматор



    //святая троица .......................................................................................................................

    private void Awake()
    {
        _animator = GetComponent<Animator>(); //назначаем ссылку на this Animator
        _spriteRenderer = GetComponent<SpriteRenderer>(); // назначаем ссылку на this SpriteRenderer

    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack; //подписка на событие атаки
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit; //подписка на событие получения урона
        _enemyEntity.OnDeath += _enemyEntity_OnDeath; //подписка на событие смерти
    }



    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed);
    }


    private void OnDestroy() //не обязательно, но считаеться хорошей практикой, + страховка на тот случай если подписчик переживает издателя 
    {
        if (_enemyAI != null)
            _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;

        if (_enemyEntity != null)
        {
            _enemyEntity.OnTakeHit -= _enemyEntity_OnTakeHit;
            _enemyEntity.OnDeath -= _enemyEntity_OnDeath;
        }

    }


    //публичные методы ....................................................................................................................

    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonCollaiderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonCollaiderTurnOn();
    }



    //приватные методы ....................................................................................................................

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e) // метод-обработчик для события атаки
    {
        _animator.SetTrigger(ATTACK); //вызываем тригер аттак
    }

    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e) // метод-обработчик для события получения урона
    {
        _animator.SetTrigger(TAKE_HIT);//спускаем триггер TAKE_HIT 
    }

    private void _enemyEntity_OnDeath(object sender, System.EventArgs e) //метод-обработчик для события смерти врага
    { 
        _animator.SetBool(IS_DIE, true); //устанавливаем параметр аниматора "IsDie" = true
        _spriteRenderer.sortingOrder = -1; //перемещаем уровень отрисовки останков на нижний слой
        _enemyShadow.SetActive(false); //отключаем тень
    }
}
