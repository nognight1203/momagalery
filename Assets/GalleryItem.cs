using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

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
    public float selectedPicHeight;
    public float selectedPicWidth;
    public static GameObject selectedGalleryItem;
    public GameObject container;

    

    public void Init( string fileName,Texture2D texture,GameObject gameObject)
    {
        this.fileName = fileName;
        this.texture2D = texture;
        this.GetObject = gameObject;
        this.texture2D.name = fileName;

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
            PointTrigger.TextureTolerence = (float)texture2D.width / (float)texture2D.height;
            PointTrigger.newPaint.transform.localScale = new Vector3(PointTrigger.SeletScale * (float)(PointTrigger.TextureTolerence), PointTrigger.SeletScale, 0.25f);
        }
        
        SupabaseWebGLExample.SelectGalleryName = fileName;
        selectedGalleryItem = this.gameObject;
        float fixedWidth = 100;
        float aspect = (float)texture2D.height / texture2D.width;
        float targetHeight = fixedWidth * aspect;
        selectedPicture.transform.localScale = new Vector3((float)1.822,(float) 1.822 * aspect, (float) 1.822);
    }
}