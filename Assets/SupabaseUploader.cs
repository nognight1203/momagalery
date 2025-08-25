using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Supabase;
using Supabase.Storage;

public class SupabaseUploader : MonoBehaviour
{
    [Header("Supabase Settings")]
    public string projectUrl = "https://xxxx.supabase.co";   // 改成你的 Supabase 專案 URL
    public string anonKey = "your-anon-key";                 // 改成你的 anon key
    public string bucketName = "galerypicture";              // 改成你的 bucket 名稱

    [Header("UI")]
    public RawImage displayImage;  // 用來顯示下載到的圖片
    public Texture2D imageToUpload; // 你要上傳的圖片

    private Supabase.Client client;

    async void Start()
    {
        // 初始化 Supabase
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = false
        };
        client = new Supabase.Client(projectUrl, anonKey, options);
        await client.InitializeAsync();
        Debug.Log("✅ Supabase 初始化完成");
    }

    // 點按鈕呼叫
    public async void UploadImage()
    {
        print(imageToUpload.name);
        try
        {
            Texture2D readable = MakeTextureReadable(imageToUpload);
            byte[] pngData = readable.EncodeToPNG();
            string fileName = "TestIamge.jpg";

            var result = await client.Storage
                .From(bucketName)
                .Upload(pngData, fileName, new FileOptions { ContentType = "image/png" });

            Debug.Log($"✅ 上傳成功: {fileName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ 上傳失敗: {e.Message}");
        }
    }

    // 點按鈕呼叫
    public async void DownloadImage()
    {
        try
        {
            string fileName = "TestIamge.jpg";

            byte[] fileBytes = await client.Storage
                .From(bucketName)
                .Download(fileName, (TransformOptions?)null);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileBytes);

            if (displayImage != null)
            {
                displayImage.texture = tex;
            }

            Debug.Log("✅ 下載成功並顯示圖片");
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ 下載失敗: {e.Message}");
        }
    }

    Texture2D MakeTextureReadable(Texture2D tex)
    {
        RenderTexture rt = RenderTexture.GetTemporary(
            tex.width,
            tex.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );

        Graphics.Blit(tex, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readableTex = new Texture2D(tex.width, tex.height);
        readableTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTex.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTex;
    }
}