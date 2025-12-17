using UnityEngine;

public class DestractiblePlantVisual : MonoBehaviour
{
    //поля ссылочные объекты ..............................................................................................
    [SerializeField] private DestractiblePlant _destructiblePlant;
    [SerializeField] private GameObject _bushDeathVFXPrefab;

    //поля ...........................................................................................................

    //Life-cycle методы ....................................................................................................

    private void Start()
    {
        _destructiblePlant.OnDestractibleTakeDamage += DestractiblePlant_OnDestractibleTakeDamage;
    }

    private void OnDestroy()
    {
        if (_destructiblePlant != null)
        {
            _destructiblePlant.OnDestractibleTakeDamage -= DestractiblePlant_OnDestractibleTakeDamage;
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
        Instantiate(_bushDeathVFXPrefab, _destructiblePlant.transform.position, Quaternion.identity);

    }

}
