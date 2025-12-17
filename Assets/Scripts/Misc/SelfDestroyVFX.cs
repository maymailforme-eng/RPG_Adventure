using UnityEngine;

public class SelfDestroyVFX : MonoBehaviour
{
    //поля ссылочные объекты ...............................................................................................
    private ParticleSystem _ps; //объект эффектов



    //Live cycle методы ...............................................................................................................
    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (_ps && !_ps.IsAlive()) // для ParticleSystem метод IsAlive() вернет tru если частицы проигрываються
        {
            DestroySelf();
        }
    }



    //приватные методы .............................................................................................................

    private void DestroySelf()
    { 
        Destroy(gameObject);
    }

}
