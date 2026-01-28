using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PasswordInput : MonoBehaviour, IPointerDownHandler
{
    public TMP_InputField input;
    static public string passwordCode = "123";
    static public bool SafePass = false;
    static public string password;
    

    void Update()
    {
       /* if (input != null)
        {
            string inputText = input.text;
            print(inputText);
        }*/
    }

    void Awake()
    {
       // input = GetComponent<TMP_InputField>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        input.ActivateInputField(); // 取得焦點
        input.Select();             // 手機跳鍵盤
    }

    public void PressLogingButton()
    {
        if (input.text == passwordCode)
        {
            SafePass = true;
        }
        if (input.text == password)
        {
            SafePass = true;
        }

    }

    public void PressChangePassword()
    {
        if (input.text == password)
        {
            
        }
    }
}