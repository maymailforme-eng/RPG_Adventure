using UnityEngine;

public class DestractiblePlantVisual : MonoBehaviour
{
    //поля ссылочные объекты ..............................................................................................
    [SerializeField] private DestractiblePlant destructiblePlant;
    [SerializeField] private GameObject bushDeathVFXPrefab;

    //поля ...........................................................................................................

    //Life-cycle методы ....................................................................................................

    private void Start()
    {
        destructiblePlant.OnDestractibleTakeDamage += DestractiblePlant_OnDestractibleTakeDamage;
    }

    private void OnDestroy()
    {
        if (destructiblePlant != null)
        {
            destructiblePlant.OnDestractibleTakeDamage -= DestractiblePlant_OnDestractibleTakeDamage;
        }
    }



    //приватные методы ................................................................................................

    //обработчик события 
    private void DestractiblePlant_OnDestractibleTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }


    private void ShowDeathVFX()
    {
        Instantiate(bushDeathVFXPrefab, destructiblePlant.transform.position, Quaternion.identity);

    }

}
