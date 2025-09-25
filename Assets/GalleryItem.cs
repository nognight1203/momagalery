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

    public void Init( string fileName,Texture2D texture)
    {
        this.fileName = fileName;
        this.texture2D = texture;

        // Button 點擊事件
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            OnClickItem();
        });
    }

    private void OnClickItem()
    {
        Debug.Log("點擊圖片檔名: " + fileName);
        // 如果要傳給其他 script，可以用事件或呼叫方法


        setPaintings.ChosingTexure = texture2D;

    }
}