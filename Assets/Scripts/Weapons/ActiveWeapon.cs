using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }
    [SerializeField] private Sword sword;//инициализация в инспекторе

    //Life-cycle методы .........................................................................................................

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if (Player.Instance.IsAlive()) //проверка жив ли персонаж
        {
            FollowMousePosition(); //отслеживает поаорот персонажа за курсором
        }

    }

    public Sword GetActiveWeapon ()
    {
        return sword;
    }

    private void FollowMousePosition() // отслежиает поворот персонажа за курсором
    {
        Vector2 mousePos = GameInput.Instance.GetMousePosition(); //получаем позицию мыши
        Vector2 playerPosition = Player.Instance.GetPlayerScreenPosition(); //позицию персонажа

        if (mousePos.x < playerPosition.x) //сравниваем
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); //поворачиваем весь объект через угол 
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); //возвращаем в исходное положение
        }

    }

}
