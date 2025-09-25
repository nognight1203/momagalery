using UnityEngine;
using System.IO;

public class SetPaintings : MonoBehaviour
{

    public Vector3 PaintPosToSave;
    public Texture2D PaintTexture;

    public GameObject PaintToSet;
    public Camera MainCamera;

    RaycastHit hit;
    Ray ray;
    public LayerMask HangingWall;

    public string imageName ;
    public string imagePath ;
    Sprite sprite;
    Texture2D tex = null;
    public  Texture2D ChosingTexure;
    byte[] fileData;

    public bool TextureSwitch;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PaintToSet.transform.GetComponent<MeshRenderer>().enabled = false;

       //string fullPath = string.IsNullOrEmpty(imagePath) ? imageName : $"{imagePath}/{imageName}";

        if (File.Exists(imagePath))
        {
            fileData = File.ReadAllBytes(imagePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
           // print(tex.width);
           // print(tex.height);
        }
        else
        {
            print("fail");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TextureSwitch == true)
        {
            imagePath = "/Users/hilight/Desktop/Myimage/testImage.jpg";
        }
        else
        {
            imagePath = "/Users/hilight/Desktop/Myimage/下載.png";
        }
        if (File.Exists(imagePath))
        {
            fileData = File.ReadAllBytes(imagePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
           // print(tex.width);
            //print(tex.height);
        }

        ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        //print(PaintToSet.GetComponent<Renderer>().material.mainTexture.width);
        //print(PaintToSet.GetComponent<Renderer>().material.mainTexture.height);
        
        //print((float)((float)tex.width/(float)tex.height));

        //HitAngle

        if (Physics.Raycast(ray, out hit ,100f,HangingWall)== true)
        {
            //print(hit.normal);
        }

            if (Physics.Raycast(ray, out hit, 100f, HangingWall) == true)
        {
            PaintToSet.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            PaintToSet.transform.GetComponent<MeshRenderer>().enabled = true;

            PaintToSet.transform.localScale = new Vector3(2f* (float)((float)ChosingTexure.width / (float)ChosingTexure.height), 2f , 0.25f);
                
            Renderer rend = PaintToSet.GetComponent<Renderer>();
            rend.material.mainTexture = ChosingTexure;

            PaintToSet.transform.LookAt(PaintToSet.transform.position + hit.normal);

        }
        else
        {
            PaintToSet.transform.GetComponent<MeshRenderer>().enabled = false;
        }

        //print(Physics.Raycast(ray, out hit, 100f, HangingWall));

        if (Input.GetButtonDown("Fire1"))
        {
            
            if (Physics.Raycast(ray, out hit, 100f, HangingWall)==true)
            {
                SetPaint();
                print("set");
            }
        }
    }

    void SetPaint()
    {
        GameObject SettingPaint = Instantiate(PaintToSet);
        Material newMaterial = new Material(PaintToSet.GetComponent<Renderer>().material);
        SettingPaint.GetComponent<Renderer>().material = newMaterial;
        SettingPaint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        Vector3 Setpos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        PaintPosToSave = Setpos;
    }
}
