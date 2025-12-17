using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))] 
public class KnockBack : MonoBehaviour
{

    //ссылочные объекты .............................................................................................
    private Rigidbody2D _rb;

    //пол€................................................................................................................

    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackMovingTimerMax;

    private float _knockBackMovingTimer;

    //свойства .................................................................................................................
    public bool IsGettingRnockBack { get; private set; } //свойство-флаг - находимс€ в состоянии отбрасывани€

    //св€та€ троица ..........................................................................................................

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
        {
            StopRnockBackMovement();
        }
    }

    //публичные методы .........................................................................................................

    public void StopRnockBackMovement() //прекращение отбрасывани€
    {
        _rb.linearVelocity = Vector3.zero;
        IsGettingRnockBack = false;//устанавливаем флаг на неактивный
    }


    public void GetKnockedBack(Transform damageSourse)
    {
        IsGettingRnockBack = true; //устанавливаем флаг на активный
        _knockBackMovingTimer = knockBackMovingTimerMax; //устаналиваем врем€ отлета на максимальное 
        Vector2 difference = (transform.position - damageSourse.position).normalized * knockBackForce / _rb.mass; //определ€ем вектор перемещени€ (направление и силу)
        
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }

    

    //приватные методы ........................................................................................................



}
