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
        //InitRotation = new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
        tgtRot = diff3;
        currDiff = 1;
        setDiff_1();
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
        if (tgtRot != transform.localRotation.eulerAngles.z)
        {
            
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(InitRotation.x, InitRotation.y, tgtRot)), rotSpeed * Time.deltaTime);
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
            currDiff = 1;
        }
        SetLevel(currDiff);

        
    }
    private void SetLevel(int level)
    {
        switch (level)
        {
            case 1:
                setDiff_1();
                break;
            case 2:
                setDiff_2();
                break;
            case 3:
                setDiff_3();
                break;
            default:
                break;
        }
    }



}
