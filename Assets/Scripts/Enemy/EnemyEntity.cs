using System;
using UnityEngine;



[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    //поля для работы с сылочными объектами ............................................................................
    [SerializeField] private EnemySO _enemySO; //скрипт объект - объект хранящий данные (характеристики врагов и т.д.)
    private PolygonCollider2D _polygomCollider2D;//коллайдер поля атаки врага
    private CapsuleCollider2D _capsuleCollider2D;//коллайдер врага
    private EnemyAI _enemyAI; //скрпит перемещения врага


    //поля .............................................................................................
    //[SerializeField] private int _maxHealth;
    private int _currentHealth;




    //события
    public event EventHandler OnTakeHit; //событие получения урона
    public event EventHandler OnDeath; //собыите - смерть


    //святая троица ......................................................................

    public void Awake()
    {
        _polygomCollider2D = GetComponent<PolygonCollider2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    public void Start()
    {
        _currentHealth = _enemySO.enemyHealthl;
    }




    //публичные методы...................................................................................................................
    public void TakeDamage(int damage) 
    { 
        _currentHealth -= damage; //получение урона
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectedDeath(); //проверка на смерть
    } 


    public void PolygonCollaiderTurnOff()//выключает коллайдер урона
    {
        _polygomCollider2D.enabled = false;
    }


    public void PolygonCollaiderTurnOn()//включает коллайдер урона
    {
        _polygomCollider2D.enabled = true;
    }



    //приватные методы...................................................................................................................
    private void DetectedDeath() //смерть
    {
        if (_currentHealth <= 0)
        {
            _capsuleCollider2D.enabled = false; //отключаем коллайдер
            _polygomCollider2D.enabled = false; //отключаем коллайдер
            _enemyAI.SetDeathState();
            //_enemyAI.enabled = false;

            OnDeath?.Invoke(this, EventArgs.Empty); //запускаем событие смерти
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, _enemySO.enemyDamageAmount);
        }
        
    }



}
