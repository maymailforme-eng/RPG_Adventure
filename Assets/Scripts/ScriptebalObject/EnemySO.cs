using UnityEngine;


[CreateAssetMenu()] //позволяет создвавть в меню такие объекты
public class EnemySO : ScriptableObject // наследуемся от ScriptableObject
{
    public string enemyName;
    public int enemyHealthl;
    public int enemyDamageAmount;


}
