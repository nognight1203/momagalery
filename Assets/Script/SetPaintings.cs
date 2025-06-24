using UnityEngine;


public class SetPaintings : MonoBehaviour
{

    public GameObject PaintToSet;
    public Camera MainCamera;

    RaycastHit hit;
    Ray ray;
    public LayerMask HangingWall;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PaintToSet.transform.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, HangingWall) == true)
        {
            PaintToSet.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            PaintToSet.transform.GetComponent<MeshRenderer>().enabled = true;

        }
        else
        {
            PaintToSet.transform.GetComponent<MeshRenderer>().enabled = false;
        }

        print(Physics.Raycast(ray, out hit, 100f, HangingWall));

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
        SettingPaint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
    }
}
