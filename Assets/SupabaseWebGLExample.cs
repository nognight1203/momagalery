using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;



public class SupabaseWebGLExample : MonoBehaviour
{
    [Header("Supabase Settings")]
    public string projectUrl = "https://ekxltzwemixjlndwqujw.supabase.co";
    public string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImVreGx0endlbWl4amxuZHdxdWp3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTQ5NzUzNTQsImV4cCI6MjA3MDU1MTM1NH0.O2JDGPxwyimQC0eSCfj3fNMKK7BL54knWBYYx0HCaeY";
    public string bucketName = "galerypicture";

    [Header("UI")]
    public RawImage displayImage; // 顯示下載的圖片

    // 範例圖片（你可以用任何 Texture2D）
    public Texture2D imageToUpload;

    public TextMeshProUGUI infor;

    /// <summary>
    /// 上傳圖片
    /// </summary>
    public void UploadImage()
    {
        
        
        Texture2D readable = MakeTextureReadable(imageToUpload);
        StartCoroutine(UploadCoroutine(readable, "TestIamge.png"));
    }

    /// <summary>
    /// 從 Supabase 下載圖片
    /// </summary>
    public void DownloadImage()
    {
        StartCoroutine(DownloadCoroutine("TestIamge.png", texture =>
        {
            displayImage.texture = texture;
        }));
    }

    IEnumerator UploadCoroutine(Texture2D texture, string fileName)
    {

        byte[] pngData = texture.EncodeToPNG();

        string url = $"{projectUrl}/storage/v1/object/{bucketName}/{fileName}";

        UnityWebRequest request = UnityWebRequest.Put(url, pngData);
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", $"Bearer {anonKey}");
        request.SetRequestHeader("Content-Type", "image/png");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ 上傳成功");
        }
        else
        {
            Debug.LogError($"❌ 上傳失敗: {request.error}");
        }
    }

    IEnumerator DownloadCoroutine(string fileName, Action<Texture2D> onDownloaded)
    {
        // 如果 bucket 是 public，就可以直接用 public URL
        string publicUrl = $"{projectUrl}/storage/v1/object/public/{bucketName}/{fileName}";

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(publicUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            onDownloaded?.Invoke(texture);
            Debug.Log("✅ 下載成功");
        }
        else
        {
            Debug.LogError($"❌ 下載失敗: {request.error}");
        }
    }

    public void ListBucketFiles()
    {
        StartCoroutine(ListBucketCoroutine());
    }

    IEnumerator ListBucketCoroutine()
    {
        // REST API: 列出 bucket 檔案
        string url = $"{projectUrl}/storage/v1/object/list/{bucketName}?limit=100";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", $"Bearer {anonKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("✅ Bucket 檔案列表: " + json);

            infor.text = request.downloadHandler.text;

            // 解析 JSON 範例（用 Newtonsoft.Json）
            //Array files = Array.Parse(json);
            /*foreach (var file in files)
            {
                Debug.Log("檔案名稱: " + file["name"]);
            }*/
        }
        else
        {
            Debug.LogError($"❌ 列出 Bucket 失敗: {request.responseCode} {request.error}");
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
