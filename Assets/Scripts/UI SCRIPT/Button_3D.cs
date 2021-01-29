using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MenuAction { Start,Quit};
public class Button_3D : MonoBehaviour, IPointerClickHandler
{

    public MenuAction action;
    public Menu_Mgr menuMgr;
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (action)
        {
            case MenuAction.Start:
                menuMgr.StartGame();
                break;
            case MenuAction.Quit:
                Debug.Log("Quit Game"); //Aggiungere chiusura gioco
                break;
            default:
                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
