using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public CharacterMovement CharacterMovement ;

    public GameObject Main;
    public GameObject Camera;

    float LastMouseX;
    float LastMouseY;
    
    public float CameraHight ;
    public bool CameraUpAndDown = false;

    public float RotateYspeed = 50;


    

    //public GameObject Camera;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera.transform.parent = Main.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        

        CameraPos();


       /* if (Input.GetButtonDown("Fire1"))
        {
            CameraUpAndDown = true;
            LastMouseY = Input.mousePosition.y;
            

        }

        if (Input.GetButtonUp("Fire1"))
        {
            CameraUpAndDown = false;
        }

        if (CameraUpAndDown == true)
        {
            if (CharacterMovement.CanTeleport == false)
            {
                CameraFacing();
            }
        }*/
    }

  

    void CameraFacing()
    {
        float RotateYadd = Input.mousePosition.y - LastMouseY;
       

        Camera.transform.Rotate(Vector3.right * Time.deltaTime * RotateYadd * RotateYspeed);
        LastMouseY = Input.mousePosition.y;

       
    }

    void CameraPos()
    {
        Camera.transform.localPosition = new Vector3(0,  CameraHight, 0);
    }
}
