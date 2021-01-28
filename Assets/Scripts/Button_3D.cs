using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MenuAction { Start,Quit};
public class Button_3D : MonoBehaviour, IPointerClickHandler
{
    private bool startCameraAnim = false;
    public MenuAction action;
    public Transform tgtCamera;
    public float speed = 10;
    private Transform camera;
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (action)
        {
            case MenuAction.Start:
                startCameraAnim = true;
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
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCameraAnim)
        {
            camera.position = Vector3.Lerp(camera.position, tgtCamera.position, speed * Time.deltaTime);
            camera.rotation = Quaternion.Lerp(camera.rotation, tgtCamera.rotation, speed * Time.deltaTime);
        }
    }
}
