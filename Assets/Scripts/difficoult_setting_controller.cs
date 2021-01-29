using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        tgtRot = diff3;
        currDiff = 1;
    }
    public void setDiff_1()
    {
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
        if (tgtRot != setting_toggle.rotation.eulerAngles.x)
        {

            setting_toggle.rotation = Quaternion.Lerp(setting_toggle.rotation,Quaternion.Euler(tgtRot,-90,-90),rotSpeed * Time.deltaTime);
            currDiff = newDiff;
        }
    }
}
