using System.Collections;
using UnityEngine;

public class TransparencyDetection : MonoBehaviour
{
    //ссылочные поля ................................................................................
    SpriteRenderer _spriteRenderer;

    //поля..............................................................................................
    [Range(0f,1f)] //добавит ползунок в инспектор чтобы настраивать значение для указанного ниже поля
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = 0.5f;
    private float _fullNonTransparent = 1.0f;

    //LiveCycle ........................................................................................
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    //приватные методы ....................................................................................
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerVisual>())
        {
            if (collision is CapsuleCollider2D)
            {
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, transparencyAmount));
            }
           
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerVisual>())
        {
            if (collision is CapsuleCollider2D)
            {
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, _fullNonTransparent));
            }
               
        }

    }


    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startTransparencyAmount, float targetTransparencyAmount)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startTransparencyAmount, targetTransparencyAmount, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r,
                                             spriteRenderer.color.g,
                                             spriteRenderer.color.b, 
                                             newAlpha);
            yield return null;
        }
    
    }


}
