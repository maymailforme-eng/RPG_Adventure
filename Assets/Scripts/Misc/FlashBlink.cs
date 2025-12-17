using UnityEngine;

//[RequireComponent(typeof(Material))]
//[RequireComponent(typeof(SpriteRenderer))]
public class FlashBlink : MonoBehaviour
{
    //ссылочные объекты .......................................................................................
    [SerializeField] private MonoBehaviour damagableObject;
    [SerializeField] private Material blinkMaterial;

    private Material _defaultMaterial; //материал по умолчанию
    private SpriteRenderer _spriteRenderer; 



    //поля .................................................................................................
    [SerializeField] private float blinkDuration = 0.2f;

    private float _blinkTimer;
    private bool _isBlinking; //флаг - может ли мигать



    //Life-cycle методы .................................................................................................

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;

        _isBlinking = true;
        
    }


    private void Start()
    {
        if (damagableObject is Player player)
        {
            player.OnFlashBlink += DamagableObject_OnFlashBlink;
        }
    }



    private void Update()
    {
        if (_isBlinking)
        {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer < 0)
            {
                SetDefaultMaterial();
            }
        }
    }


    private void OnDestroy() //выполниться при разрушении объекта 
    {
        if (damagableObject is Player player)
        {
            player.OnFlashBlink -= DamagableObject_OnFlashBlink; // отписываемся от события 
        }
    }

    //публичные методы ..........................................................................................................................

    public void StopBlinking() //останавливает мигание
    {
        SetDefaultMaterial();
        _isBlinking = false;
    }


    //приватные методы ......................................................................................................................

    private void SetDefaultMaterial()
    {
        _spriteRenderer.material = _defaultMaterial;  
    }

    private void SetBlinkingMaterial()
    {
        _blinkTimer = blinkDuration;
        _spriteRenderer.material = blinkMaterial;
    }

    private void DamagableObject_OnFlashBlink(object sender, System.EventArgs e) //метод-обработчик событи мигания 
    {
        SetBlinkingMaterial();
    }


}
