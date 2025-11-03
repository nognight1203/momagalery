using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PointTrigger : MonoBehaviour, IPointerClickHandler
{
    public string pointID;
    public static GameObject selectedPaint;
    public static GameObject newPaint;
    public static string SelectedPosID;

    public static Texture2D textureForPaint;
    public static GameObject PaintSet;

    public GameObject testBall;

    public static string testName;

    public static bool AddMode ;
    public static bool PickingMode = true;
    public static bool MoveMode;
    public  bool addMode ;
    public  bool pickingMode ;
    public  bool moveMode;

    public static Dictionary<string, GameObject> PaintDataDic = new Dictionary<string, GameObject>();

    public void OnPointerClick(PointerEventData eventData)
    {
        print(pointID);

        SelectedPosID = pointID;
        //if(AddMode == true)
        {
        /*GameObject SettingPaint = Instantiate(PaintToSet);
        Material newMaterial = new Material(PaintToSet.GetComponent<Renderer>().material);
        SettingPaint.GetComponent<Renderer>().material = newMaterial;*/

        //print(testName);

            //PaintToSet = testBall;
           // PaintToSet.transform.position = this.transform.position;

        }
        print(AddMode);
        if(AddMode == true)
        {
            if(newPaint == null)
            {
                GameObject SettingPaint = Instantiate(PaintSet);
                Material newMaterial = new Material(PaintSet.GetComponent<Renderer>().material);
                SettingPaint.GetComponent<Renderer>().material = newMaterial;
                SettingPaint.GetComponent<Renderer>().material.mainTexture = textureForPaint;
                SettingPaint.transform.position = this.transform.position;
                newPaint = SettingPaint;
                newPaint.transform.localScale = new Vector3(2f * (float)((float)textureForPaint.width / (float)textureForPaint.height), 2f, 0.25f);
                //print(textureForPaint.name);
            }
            else
            {
                newPaint.transform.position = this.transform.position;
            }
        }

    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
