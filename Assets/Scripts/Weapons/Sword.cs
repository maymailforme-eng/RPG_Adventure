using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]


public class Sword : MonoBehaviour
{
    //поля ...................................................................................................................................
    [SerializeField] private int _damageAmount;    //урон от меча
    
    public event EventHandler OnSwordSwing; 
    private PolygonCollider2D _polygonCollider2D;



    //Life-cycle методы .....................................................................................................................
    public void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>(); //кэшируем ПолигонКоллайдер
    }




    private void Start()
    {
        AttackColliderTurnOff(); //отключаем коллайдер на старте
    }



    //обработчики событий .............................................................................................................
    public void AttacK()//инициатор события
    {
        if (_polygonCollider2D.enabled == true) { AttackColliderTurnOff(); }
        AttackColliderTurnOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
        
    }



    //Коллайдер .........................................................................................................................
    public void AttackColliderTurnOff() //отключить коллайдер
    {
        _polygonCollider2D.enabled = false;
    }

    private void AttackColliderTurnOn() //включить коллайдер
    {
        _polygonCollider2D.enabled = true;
    }



    private void OnTriggerEnter2D(Collider2D collision) //метод Unity взаимодействие с колайдорами объектов
    {
        //когда коллайдер меча сталкивается с другим коллайдером проверяем:
        if (collision.TryGetComponent(out EnemyEntity enemyEntity)) //являеться ли объект врагом
        {
            enemyEntity.TakeDamage(_damageAmount);
            //enemyEntity.DetectedDeath(); //проверка на смерть
        }
    }




}
