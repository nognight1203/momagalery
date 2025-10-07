using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GalleryItem : MonoBehaviour
{
    public Button button;
    public RawImage rawImage;
    //public SetPaintings setPaintings;
    [HideInInspector] public string fileName;
    public SetPaintings setPaintings;
    Texture2D texture2D;
    GameObject GetObject;
    public RawImage selectedPicture;
    

    public void Init( string fileName,Texture2D texture,GameObject gameObject)
    {
        this.fileName = fileName;
        this.texture2D = texture;
        this.GetObject = gameObject;

        // Button 點擊事件
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            OnClickItem();
            selectedPicture.texture = texture;
        });
    }

    private void OnClickItem()
    {
        Debug.Log("點擊圖片檔名: " + fileName);
        // 如果要傳給其他 script，可以用事件或呼叫方法


        setPaintings.ChosingTexure = texture2D;

        PointTrigger.testName = fileName;

        PointTrigger.PaintSet = GetObject;
        PointTrigger.textureForPaint = texture2D;
        if(PointTrigger.newPaint != null)
        {
            PointTrigger.newPaint.GetComponent<Renderer>().material.mainTexture = texture2D;
        }
        
    }
}