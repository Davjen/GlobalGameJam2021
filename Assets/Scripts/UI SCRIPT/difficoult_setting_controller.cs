using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class difficoult_setting_controller : MonoBehaviour
{
    private bool updateRot = false;
    private float tgtRot = 0;
    public float rotSpeed = 0;
    public float diff1, diff2, diff3;
    public Transform setting_toggle;
    private int currDiff = 0;
    private int newDiff = 0;
    public Menu_Mgr menu_Mgr;
    public Vector3 InitRotation;

    // Start is called before the first frame update
    void Start()
    {
        InitRotation = new Vector3(setting_toggle.rotation.x, setting_toggle.rotation.y, setting_toggle.rotation.z);
        tgtRot = diff3;
        currDiff = 1;
    }
    public void setDiff_1()
    {
        Debug.Log("ciao");
        tgtRot = diff1;
        menu_Mgr.level = 1;
        newDiff = 1;
    }
    public void setDiff_2()
    {
        tgtRot = diff2;
        menu_Mgr.level = 2;
        newDiff = 2;
    }
    public void setDiff_3()
    {
        tgtRot = diff3;
        menu_Mgr.level = 3;
        newDiff = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (tgtRot != setting_toggle.rotation.eulerAngles.z)
        {

            setting_toggle.rotation = Quaternion.Lerp(setting_toggle.rotation, Quaternion.Euler(new Vector3(InitRotation.x, InitRotation.y, rotSpeed)), rotSpeed * Time.deltaTime);
            currDiff = newDiff;
        }
    }
    public void NextLevel()
    {
        if (!menu_Mgr.Selection.isPlaying)
        {
            menu_Mgr.Selection.Play();
        }
        currDiff++;
        if (currDiff > 3)
        {

        }
    }



}
