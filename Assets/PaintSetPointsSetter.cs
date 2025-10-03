using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSetPointsSetter : MonoBehaviour
{
    public float PointHight;
    public float PointWight;

    public int HorizenCount;
    public int VerticleCount;

    //基準點
    public GameObject BasePosition;

    public GameObject ConerLeftBottom;
    //public GameObject ConerLeftTop;
    //public GameObject ConerRightBottom;
    public GameObject ConerRightTop;

    public GameObject PointGizmo;

    public bool test = false;
    public string testStr;

    static Dictionary<string,GameObject> PointsDic = new Dictionary<string, GameObject>();
    public string WallID;

    

    void Start()
    {
        /*ConerLeftBottom = Instantiate(PointGizmo, this.transform);
        ConerLeftTop = Instantiate(PointGizmo, this.transform); 
        ConerRightBottom = Instantiate(PointGizmo, this.transform); 
        ConerRightTop = Instantiate(PointGizmo, this.transform);*/

        SetHightAndWight();
        ActivePointsDic(HorizenCount,VerticleCount);

       
    }

    private void Update()
    {
        if(test == true)
        {
            PointsDic[testStr].SetActive(false);
        }
        if (PointsDic[testStr] != null)
        {
            if (test == false && PointsDic[testStr].activeSelf == false)
            {
                PointsDic[testStr].SetActive(true);
            }
        }
    }

    void SetHightAndWight() {
        /*float rightBottomX =  PointWight;
        float rightBottomY = 0;
        float rightTopX =  PointWight;
        float rightTopY =   PointHight;
        float leftBottomX = 0;
        float leftBottomY = 0;
        float leftTopX = 0;
        float leftTopY =  PointHight;*/

        //ConerLeftBottom.transform.SetLocalPositionAndRotation(new Vector3(leftBottomX, leftBottomY), new Quaternion(0, 0, 0, 0));
        //ConerLeftTop.transform.SetLocalPositionAndRotation(new Vector3(leftTopX, leftTopY), new Quaternion(0, 0, 0, 0));
        //ConerRightBottom.transform.SetLocalPositionAndRotation(new Vector3(rightBottomX, rightBottomY), new Quaternion(0, 0, 0, 0));
        //ConerRightTop.transform.SetLocalPositionAndRotation(new Vector3(rightTopX, rightTopY), new Quaternion(0, 0, 0, 0));

        //左下右上控制範圍
        PointWight = ConerRightTop.transform.localPosition.x - ConerLeftBottom.transform.localPosition.x;
        PointHight = ConerRightTop.transform.localPosition.y - ConerLeftBottom.transform.localPosition.y;



    }

    void ActivePointsDic(int weight,int hight)
    {
        
        float toleranceWeight = PointWight / (weight - 1);
        float toleranceHeight = PointHight / (hight - 1);
        for(int i=0; i < weight; i++)
        {

            for (int t = 0; t < hight; t++)
            {
                GameObject gameObject = Instantiate(PointGizmo, this.transform);
                gameObject.transform.SetLocalPositionAndRotation(new Vector3(toleranceWeight * i, toleranceHeight * t), new Quaternion(0, 0, 0, 0));
                //print($"id = {WallID}-{i}-{t}");
                string pointID = $"{WallID}-{i}-{t}";
                print(pointID);
                PointsDic.Add(pointID,gameObject);
                gameObject.GetComponent<PointTrigger>().pointID = pointID;
            }
        }

    }

}
