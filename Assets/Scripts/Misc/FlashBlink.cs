using UnityEngine;

[RequireComponent(typeof(Material))]
[RequireComponent(typeof(SpriteRenderer))]
public class FlashBlink : MonoBehaviour
{
    //ссылочные объекты .......................................................................................
    [SerializeField] private MonoBehaviour _damagableObject;
    [SerializeField] private Material _blinkMaterial;

    private Material _defaultMaterial; //материал по умолчанию
    private SpriteRenderer _spriteRenderer; 



    //поля .................................................................................................
    [SerializeField] private float _blinkDuration = 0.2f;

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
        if (_damagableObject is Player)
        {
            (_damagableObject as Player).OnFlashBlink += DamagableObject_OnFlashBlink;
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
        if (_damagableObject is Player)
        {
            (_damagableObject as Player).OnFlashBlink -= DamagableObject_OnFlashBlink; // отписываемся от события 
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
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }

    private void DamagableObject_OnFlashBlink(object sender, System.EventArgs e) //метод-обработчик событи мигания 
    {
        SetBlinkingMaterial();
    }


}
