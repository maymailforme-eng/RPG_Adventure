using System;
using UnityEngine;

public class DestractiblePlant : MonoBehaviour
{
    //события.......................................................................................................
    public event EventHandler OnDestractibleTakeDamage;


    //приватные методы ..........................................................................................
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Sword>())
        {
            OnDestractibleTakeDamage?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            NavMeshSurfaseManagment.Instance.RebakeNavmeshSurface(); //пересчитываем карту для прохода
        }
    }
}
