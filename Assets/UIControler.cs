using UnityEngine;

public class UIControler : MonoBehaviour
{
    public GameObject Setting;

    [Header("Chose Paint UI")]

    public GameObject UI;
    public GameObject PaintsScrollView;
    

    public void ShowChosingPaintsUI()
    {
        PaintsScrollView.SetActive(true) ;
        Setting.SetActive(false);
    }

    public void CloseChosingPaintsUI()
    {
        PaintsScrollView.SetActive(false);
        Setting.SetActive(true);
    }


}
