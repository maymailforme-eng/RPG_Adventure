using UnityEngine;
using System;





public class SwordVisual : MonoBehaviour
{
    //ссылочные объекты ..................................................................................
    private Animator _animator;
    private Sword _sword; //parent node


    //поля ...................................................................................................
    private const string ATTACK = "Attack";


    //Life-cycle методы ..................................................................................................

    private void Awake()
    {   
        _sword = GetComponentInParent<Sword>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _sword.OnSwordSwing += Sword_OnSwordSwing;
    }


    private void OnDestroy()
    {
        if (_sword != null)
        {
            _sword.OnSwordSwing -= Sword_OnSwordSwing;
        }
    }

    //публичные методы ...............................................................................................

    public void TriggerEndAttackAnimation() //метод активирующий AttackColliderTurnOff
    {
        _sword.AttackColliderTurnOff(); //выключает коллайдер меча
    }

    //приватные методы ......................................................................................................
    private void Sword_OnSwordSwing(object sender, EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }


}
