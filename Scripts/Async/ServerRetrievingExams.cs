using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class ServerRetrievingExams : MonoBehaviour
{
    [Tooltip("Esto guarda la textura online")][SerializeField] Texture texture;
    [Space()]
    [SerializeField] RawImage myImg;
    public Texture errorImg;
    public GameObject loadingObj;
    [TextArea()]
    public string routeLocal;
    [Space()]
    public Text convenient;

    public void GetDriveImage()
    {
        //routeLocal = (Application.isMobilePlatform ? Application.persistentDataPath : Application.dataPath) + "/Drive";
        routeLocal = (Application.isMobilePlatform) ? Application.persistentDataPath : Application.dataPath + "/Drive";
        StartCoroutine(RetrieveTexture());
    }

    public IEnumerator RetrieveTexture()
    {
        string linkToDownloadImage = "https://preview.redd.it/should-you-skip-the-first-devil-deal-based-on-character-v0-jc3m8cck4dsc1.png?width=1080&crop=smart&auto=webp&s=183cc5fff8a963268667176005fe75a68138b7bc";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(linkToDownloadImage, true);
        loadingObj.SetActive(true);
        yield return www.SendWebRequest();
        
        if (www.result != UnityWebRequest.Result.Success)
        {
            myImg.texture = errorImg;
            Debug.Log($"WebRequest: {www.error}");
        }
        else
        {
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            myImg.texture = texture;
            ResizeRawImage(myImg, 260);
            
            try
            {
                File.WriteAllBytes(routeLocal + "/Nose.jpg", www.downloadHandler.data);
            }
            catch (System.Exception e)
            {
                convenient.text = e.ToString();
            }
        }

        loadingObj.SetActive(false);
        www.Dispose();
    }

    public void ResizeRawImage(RawImage rawImage, int imgWidth)
    {
        //Width = 312
        if (rawImage != null)
        {
            //size.x * mult = width
            //mult = width / size.x
            rawImage.SetNativeSize();
            float mult = imgWidth/rawImage.rectTransform.sizeDelta.x;
            rawImage.rectTransform.sizeDelta *= mult;
        }
    }
}
