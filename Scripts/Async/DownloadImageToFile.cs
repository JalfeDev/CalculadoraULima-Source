using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum AsyncState {Ask, Progress, Success, Error}

public class DownloadImageToFile : MonoBehaviour
{
    [TextArea()]
    public string routeLocal;
    private string currentPath;
    public TextAsset textAsset;
    private CloudImages cloudImages;
    private CloudImages localImages;

    //public ConvertImagesRedirected convertor;
    
    public AsyncState state = AsyncState.Ask;
    private string isError = "";
    private int countLinks = 0;
    
    public GameObject currentActive;
    private GameObject childAsk;
    private GameObject childProgress;
    private GameObject childSuccess;
    private GameObject childError;
    public Text textConvenient;
    public Slider progressJSON;
    public Text progressPNG;

    void Awake()
    {
        //Escoge la plataforma que se va a usar
        currentPath = CameraTransition.CreateJsonInAllPaths("DriveImagesInLocal.json", textAsset);
        routeLocal = (Application.isMobilePlatform) ? Application.persistentDataPath : Application.dataPath + "/Drive";
        localImages = new CloudImages();

        childAsk = transform.GetChild(0).gameObject;
        childProgress = transform.GetChild(1).gameObject;
        childSuccess = transform.GetChild(2).gameObject;
        childError = transform.GetChild(3).gameObject;
    }

    public static string FromShareToDownload(string url)
    {
        //Compartir enlace: https://drive.google.com/file/d/FILE_ID/view?usp=drive_link
        //Descargar imagen: https://drive.google.com/uc?export=download&id=FILE_ID
        string fileID = url.Substring(32, url.Length-52);
        return "https://drive.google.com/uc?export=download&id=" + fileID;
    }
    
    public void GetDriveJSON(bool downloadEverything)
    {
        StartCoroutine(RetrieveData(downloadEverything));
    }

    private IEnumerator RetrieveData(bool downloadEverything)
    {        
        string jsonDriveShare = "https://drive.google.com/file/d/17b_s9InsrGkfJn8ozPQqm2iVMu0zqYUF/view?usp=drive_link";
        UnityWebRequest www = UnityWebRequest.Get(FromShareToDownload(jsonDriveShare));
        
        UpdateAsyncState(AsyncState.Progress);
        //SendMessage
        progressJSON.SendMessage("TakeUnityWebRequest", www);
        yield return www.SendWebRequest();
        
        if (www.result != UnityWebRequest.Result.Success)
        {
            isError = www.error;
            Debug.Log($"WebRequest: {isError}");
            UpdateAsyncState(AsyncState.Error, isError);
        }
        else
        {
            cloudImages = JsonUtility.FromJson<CloudImages>(www.downloadHandler.text);

            if (cloudImages.Ciclos.Count != cloudImages.Horarios.Count)
            {
                isError = "Error: Las imagenes de ciclos y horarios no son iguales";
                Debug.Log(isError);
            }

            int startWhere;
            if (downloadEverything) startWhere = 0;
            else
            {
                startWhere = PlayerPrefs.GetInt("NumCyclesImages");
                countLinks = startWhere*2;
                progressPNG.text = $"PNG ({countLinks}/{cloudImages.CountLinks()})";
            }
            
            for(int i = startWhere; i < cloudImages.Ciclos.Count; i++)
            {
                StartCoroutine(GetDrivePNG(cloudImages.Ciclos[i], $"ciclo examenes {i+1}" + CloudImages.ciclosFormat));
                StartCoroutine(GetDrivePNG(cloudImages.Horarios[i], $"horario {i+1}" + CloudImages.horariosFormat));
            }
            yield return new WaitUntil( () => countLinks >= cloudImages.CountLinks() );
        }
        
        //SendMessage
        progressJSON.SendMessage("LeaveUnityWebRequest");
        
        if (isError != string.Empty) UpdateAsyncState(AsyncState.Error, isError);
        else UpdateAsyncState(AsyncState.Success);
        
        isError = string.Empty;
        Debug.Log("     HHHEEECHCHOOO");
        www.Dispose();
        if (!FromCloudToLocal())
        {
            textConvenient.text = "Error: No se descargaron las imagenes por completo";
        }
        // else por los LoadImages
        // {
        //     if (!convertor.RewriteImages(localImages)) textConvenient.text = "Error: Se descargaron las imagenes por completo, pero hubo un error al guardarlas";
        //     else textConvenient.text = "";
        // }
    }

    private IEnumerator GetDrivePNG(string url, string fileName)
    {
        UnityWebRequest www = UnityWebRequest.Get(FromShareToDownload(url));
        
        yield return www.SendWebRequest();

        if (isError == string.Empty && countLinks < cloudImages.CountLinks())
        {
            if (www.result != UnityWebRequest.Result.Success)
            {
                isError = www.error;
                Debug.Log($"PNG WebRequest: {isError}");
                countLinks = cloudImages.CountLinks();
            }
            else
            {
                // Debug.Log("            Facilito el " + fileName);
                File.WriteAllBytes(routeLocal + "/" + fileName, www.downloadHandler.data);
                countLinks++;
                progressPNG.text = $"PNG ({countLinks}/{cloudImages.CountLinks()})";
            }
        }
        
        www.Dispose();
    }

    /// <summary>
    /// Pasa todos los archivos descargados a la clase y al json, si alguno no existe, devolvera falso
    /// </summary>
    private bool FromCloudToLocal()
    {        
        int minCount = Math.Min(cloudImages.Ciclos.Count, cloudImages.Horarios.Count);
        for(int i=0; i < minCount; i++)
        {
            if (File.Exists(routeLocal + $"/ciclo examenes {i+1}" + CloudImages.ciclosFormat) && File.Exists(routeLocal + $"/horario {i+1}" + CloudImages.horariosFormat))
            {
                localImages.Ciclos.Add(routeLocal + $"/ciclo examenes {i+1}" + CloudImages.ciclosFormat);
                localImages.Horarios.Add(routeLocal + $"/horario {i+1}" + CloudImages.horariosFormat);
            }
            else
            {
                File.WriteAllText(currentPath, JsonUtility.ToJson(localImages));
                PlayerPrefs.SetInt("NumCyclesImages", i);
                return false;
            }
        }
        File.WriteAllText(currentPath, JsonUtility.ToJson(localImages));
        PlayerPrefs.SetInt("NumCyclesImages", minCount);
        return true;
    }

    public void SetAsyncStateToDefault()
    {
        UpdateAsyncState(AsyncState.Ask);
        //Lo pongo aqui porque en la Coroutine principal hace que se descargue todo aunque haya error,
        //esto es por el WaitUntil que puse en caso de error
        //Por que hago tanto para el caso de un error si es una app simple, solo mia y que no puede salir mal?
        countLinks = 0;
    }

    private void UpdateAsyncState(AsyncState newState, string message = "")
    {
        state = newState;
        currentActive.SetActive(false);
        switch(newState)
        {
            case AsyncState.Ask:
                childAsk.SetActive(true);
                currentActive = childAsk;
            break;

            case AsyncState.Progress:
                childProgress.SetActive(true);
                currentActive = childProgress;
            break;

            case AsyncState.Success:
                childSuccess.SetActive(true);
                currentActive = childSuccess;
            break;

            case AsyncState.Error:
                childError.SetActive(true);
                currentActive = childError;

                childError.GetComponent<Text>().text += "\n" + message;
            break;
        }
    }
}

[System.Serializable]
public class CloudImages
{
    public List<string> Ciclos;
    public List<string> Horarios;
    public const string ciclosFormat = ".jpg";
    public const string horariosFormat = ".png";

    public int CountLinks()
    {
        return 1 + Ciclos.Count + Horarios.Count;
    }

    public CloudImages()
    {
        Ciclos = new List<string>();
        Horarios = new List<string>();
    }
}