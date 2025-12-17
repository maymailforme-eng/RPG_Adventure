using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    //статические поля .........................................................................................................
    public static GameInput Instance { get; private set; } //одиночка

    //ссылочные поля ..............................................................................................................
    private PlayerInputAction playerInpuActions; //хранит PlayerInputAction  

    //события .........................................................................................................................
    public event EventHandler OnPlayerAttack;//список хранящий в себе методы (событие)


    //Life-cycle методы........................................................................................................................

    private void Awake()
    {
        Instance = this; // записываем в статическую переменную ссылку на класс который ее вызвал.
        playerInpuActions = new PlayerInputAction();// создаем объект класса playerInpuActions (класс описан в библиотеке)
        playerInpuActions.Enable(); //включает все Action внутри  playerInpuActions;

        playerInpuActions.Combat.Attac.started += PlayerAttac_started; //playerInpuActions.Combat.Attac.started - событие нажатие левой клавиши мыши
        //подписываем на него метод PlayerAttac_started
    }

    //публичные методы.................................................................................................................................
    public Vector2 GetMovementVector() //вернет положение персонажа
    {
        Vector2 inputVector = playerInpuActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }


    public Vector2 GetMousePosition()//вернет положение курсора мыши в экранных координатах
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }

    public void DisableMovement()
    {
        playerInpuActions.Disable();
    }





    //приватные методы ....................................................................................................................................
    //обработчик события атаки
    private void PlayerAttac_started(InputAction.CallbackContext obj) // InputAction.CallbackContext obj параметр обязателен в сигнатуре InputAction.Combat.Attac.started
                                                                      //InputAction.Combat.Attac.started может вмещать только методы с параметром(InputAction.CallbackContext obj)
                                                                      //метод будет запускать событие OnPlayerAttack на которое будем подписываться из других классов
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty); //все методы внутри события запустяться с параметрами this, EventArgs.Empty
        //EventHandler может вмещать в себя только методы  принимающие (object sender, EventArgs e)
    }

   
}
