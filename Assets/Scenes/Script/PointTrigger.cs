using UnityEngine;
using UnityEngine.EventSystems;

public class PointTrigger : MonoBehaviour, IPointerClickHandler
{
    public string pointID;
    public static GameObject selectedPaint;
    public static GameObject newPaint;

    public static Texture2D textureForPaint;
    public static GameObject PaintSet;

    public GameObject testBall;

    public static string testName;

    public static bool AddMode;
    public static bool PickingMode = true;
    public static bool MoveMode;
    public  bool addMode;
    public  bool pickingMode ;
    public  bool moveMode;

    public void OnPointerClick(PointerEventData eventData)
    {
        print(pointID);


        //if(AddMode == true)
        {
        /*GameObject SettingPaint = Instantiate(PaintToSet);
        Material newMaterial = new Material(PaintToSet.GetComponent<Renderer>().material);
        SettingPaint.GetComponent<Renderer>().material = newMaterial;*/

        print(testName);

            //PaintToSet = testBall;
           // PaintToSet.transform.position = this.transform.position;

        }

        if(PickingMode == true)
        {
            if(newPaint == null)
            {
                GameObject SettingPaint = Instantiate(PaintSet);
                Material newMaterial = new Material(PaintSet.GetComponent<Renderer>().material);
                SettingPaint.GetComponent<Renderer>().material = newMaterial;
                SettingPaint.GetComponent<Renderer>().material.mainTexture = textureForPaint;
                SettingPaint.transform.position = this.transform.position;
                newPaint = SettingPaint;
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
