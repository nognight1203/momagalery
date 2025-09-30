using System;
using System.Collections;
using UnityEngine;

public class PaintSetPointsSetter : MonoBehaviour
{
    public float PointHight;
    public float PointWight;

    public int HorizenCount;
    public int VerticleCount;

    //基準點
    public GameObject BasePosition;

    GameObject ConerLeftBottom;
    GameObject ConerLeftTop;
    GameObject ConerRightBottom;
    GameObject ConerRightTop;

    public GameObject PointGizmo;

    
    public IDictionary VerticleList;
    public IDictionary HorizenList;

    void Start()
    {
        ConerLeftBottom = Instantiate(PointGizmo, this.transform);
        ConerLeftTop = Instantiate(PointGizmo, this.transform); 
        ConerRightBottom = Instantiate(PointGizmo, this.transform); 
        ConerRightTop = Instantiate(PointGizmo, this.transform);

        ActiveHorizenList(HorizenCount);


    }

    private void Update()
    {
        SetHightAndWight();
    }

    void SetHightAndWight() {
        float rightBottomX =  PointWight;
        float rightBottomY = 0;
        float rightTopX =  PointWight;
        float rightTopY =   PointHight;
        float leftBottomX = 0;
        float leftBottomY = 0;
        float leftTopX = 0;
        float leftTopY =  PointHight;

        ConerLeftBottom.transform.SetLocalPositionAndRotation(new Vector3(leftBottomX, leftBottomY), new Quaternion(0, 0, 0, 0));
        ConerLeftTop.transform.SetLocalPositionAndRotation(new Vector3(leftTopX, leftTopY), new Quaternion(0, 0, 0, 0));
        ConerRightBottom.transform.SetLocalPositionAndRotation(new Vector3(rightBottomX, rightBottomY), new Quaternion(0, 0, 0, 0));
        ConerRightTop.transform.SetLocalPositionAndRotation(new Vector3(rightTopX, rightTopY), new Quaternion(0, 0, 0, 0));
        
    }

    void ActiveHorizenList(int value)
    {
        print("in");
        float tolerance = PointWight / (value - 1);
        for(int i=0; i < value; i++)
        {
            print(i);
            GameObject gameObject = Instantiate(PointGizmo, this.transform);
            gameObject.transform.SetLocalPositionAndRotation(new Vector3(tolerance * i, 0), new Quaternion(0, 0, 0, 0));
        }

    }

}
