using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject Main;
    public Camera MainCamera;
    public float horisontalInput;
    public float verticalInput;
    public bool turnLeftAndRight = false;
    float LastMouseX;
    public float ForwardSpeed = 1;
    public float RotateXspeed = 100;

    RaycastHit hit;
    Ray ray ;
    public LayerMask MaskFloor;
    public LayerMask MaskWall;
    public bool CanTeleport = false ;
    public GameObject TeleportMark;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horisontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Main.transform.Translate(Vector3.forward * Time.deltaTime * verticalInput);
        //transform.Translate(-Vector3.right * Time.deltaTime * horisontalInput);

        //左右轉動
        if (Input.GetButtonDown("Fire1"))
        {
            turnLeftAndRight = true;
            LastMouseX = Input.mousePosition.x;
            //
            ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit, 100f,MaskFloor) == true)
            {
                if (Physics.Raycast(ray,out hit, 100f, MaskWall) == true)
                {
                    CanTeleport = false;
                }
                else
                {
                    CanTeleport = true;
                    TeleportMark.transform.position = Main.transform.position;


                }
            }
            else
            {
                CanTeleport = false;
            }
            //
        }


        if(CanTeleport == true)
        {
            ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            
                TeleportMark.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            
                TeleportMark.transform.GetComponent<MeshRenderer>().enabled = true;
        }



        if (Input.GetButtonUp("Fire1"))
        {
            turnLeftAndRight = false;

            if(CanTeleport == true)
            {
                TeleportMark.transform.GetComponent<MeshRenderer>().enabled = false;
                Teleport();
            }

        }

        if(turnLeftAndRight == true)
        {
            if (CanTeleport == false)
            {
                RotateMain();
            }
        }

        //
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = MainCamera.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(MainCamera.transform.position, (mousePos - MainCamera.transform.position), Color.blue);
        //

        

        if (Physics.Raycast(ray,out hit, 100f))
        {
           // print(hit.point);
        }
        //print(Physics.Raycast(ray, out hit, 100f));

    }


    

    void RotateMain()
    {

        float RotateXadd = Input.mousePosition.x - LastMouseX;
        

        Main.transform.Rotate(Vector3.up * Time.deltaTime * - RotateXadd * RotateXspeed);
        LastMouseX = Input.mousePosition.x;



    }

    void Teleport()
    {
        Main.transform.position = new Vector3(hit.point.x, Main.transform.position.y, hit.point.z);
        CanTeleport = false;
    }
    

}
