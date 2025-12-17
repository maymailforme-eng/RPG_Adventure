using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshSurfaseManagment : MonoBehaviour
{
    //статические свойства ................................................................................................................
    public static NavMeshSurfaseManagment Instance { get; private set; } //одиночка


    //поля ссылочные объекты ...........................................................................................................
    private NavMeshSurface _navMeshSurface; 




    //Live cycle методы .........................................................................................................

    private void Awake()
    {
        Instance = this;
        _navMeshSurface = GetComponent<NavMeshSurface>();   
        _navMeshSurface.hideEditorLogs = true; //выключаем вывод логов в консоль, чтобы не засорять
    }


    //публичные методы ...................................................................................................................

    public void RebakeNavmeshSurface()
    { 
        _navMeshSurface.BuildNavMesh();//перезапекаем карту (пересчет гизмо)
    }



}
