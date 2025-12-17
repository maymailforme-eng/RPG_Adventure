using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(FlashBlink))]
public class PlayerVisual : MonoBehaviour
{
    //ссылочные объекты ............................................................................................................
    private Animator _animator; //объект Animator - аниматор контроллер, создаем переменную под него
    private SpriteRenderer _spriteRenderer; // переменная под spriteRenderer объекта 
    private FlashBlink _flashBlink;


    //поля ......................................................................................................................
    private const string IS_DIE = "IsDie"; // константа (имя параметра в аниматоре)
    private const string IS_RUNNING = "isRunning"; // константа (имя параметра в аниматоре)


    //святая троица .................................................................................................................
    private void Awake()
    {
        _animator = GetComponent<Animator>(); //получаем ссылку на Animator висящий 
        //на нашем объекте (к которому прикреплен скрипт)
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlink = GetComponent<FlashBlink>();

    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }


    private void Update()
    {
        _animator.SetBool(IS_RUNNING, Player.Instance.IsRunning()); //устанавливаем значение параметра bool в animator по имени isRunning  
        //за значение отвечает метод Player.Instance.IsRunning() - которая определяет бежит герой или нет
        if (Player.Instance.IsAlive()) 
        {
            AdjustPlayerFacingDirection(); //отслеживает поворот персонажа за курсором
        }

    }

    private void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
        }

    }


    //публичные методы ....................................................................................................

    //приватные методы ....................................................................................................
    private void AdjustPlayerFacingDirection() // отслежиает поворот персонажа за курсором
    {
        Vector2 mousePos = GameInput.Instance.GetMousePosition(); //получаем позицию мыши
        Vector2 playerPosition = Player.Instance.GetPlayerScreenPosition(); //позицию персонажа

        if (mousePos.x < playerPosition.x) //сравниваем
        { 
            _spriteRenderer.flipX = true; //если курсор левее персанажа, отзеркаливаем через  SpriteRenderer
            //SpriteRenderer - объект полученный с нашего персанажа (окно)
        }
        else { _spriteRenderer.flipX = false; }
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e) //метод-обработчик события смерти;
    {
        _animator.SetBool(IS_DIE, true); //устанавливаем значение параметра в аниматоре
        _flashBlink.StopBlinking(); //останавливаем мигание;
    }


}
