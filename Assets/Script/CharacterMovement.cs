using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject Main;
    public float horisontalInput;
    public float verticalInput;
    public bool turnLeftAndRight = false;
    float LastMouseX;
    public float ForwardSpeed = 1;
    public float RotateXspeed = 100;


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
            
        }

        if (Input.GetButtonUp("Fire1"))
        {
            turnLeftAndRight = false;
        }

        if(turnLeftAndRight == true)
        {
            RotateMain();
        }


    }


    

    void RotateMain()
    {

        float RotateXadd = Input.mousePosition.x - LastMouseX;
        

        Main.transform.Rotate(Vector3.up * Time.deltaTime * - RotateXadd * RotateXspeed);
        LastMouseX = Input.mousePosition.x;



    }
}
