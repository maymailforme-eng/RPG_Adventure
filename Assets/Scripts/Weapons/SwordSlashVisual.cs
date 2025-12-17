using System;
using UnityEngine;

public class SwordSlashVisual : MonoBehaviour
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
        _sword.OnSwordSwing += SwordSlash_OnSwordSwing;
    }


    private void OnDestroy()
    {
        if (_sword != null) 
        {
            _sword.OnSwordSwing -= SwordSlash_OnSwordSwing;
        }
    }

    //приватные методы ......................................................................................................
    private void SwordSlash_OnSwordSwing(object sender, EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }



}
