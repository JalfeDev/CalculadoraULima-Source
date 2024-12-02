using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetLocalImages : MonoBehaviour
{
    public enum ImageType {Horario, Examenes}
    public ImageType type;

    int numCycles;
    string routeLocal;

    [Tooltip("Aqui se guardaran las imagenes locales")]
    public Texture2D[] tex;
    [Tooltip("Esto son los datos de una imagen local")]
    private byte[] byteTexture;

    void Awake()
    {
        routeLocal = (Application.isMobilePlatform) ? Application.persistentDataPath : Application.dataPath + "/Drive";
        numCycles = PlayerPrefs.GetInt("NumCyclesImages");
        //tex = new Texture2D[numCycles];
        
        switch(type)
        {
            case ImageType.Horario:
                for (int i=0; i < numCycles; i++)
                {
                    byteTexture = File.ReadAllBytes(routeLocal + $"/horario {i+1}.png");
                    tex[i] = new Texture2D(128,64);
                    Debug.Log($"Horario {i+1} Start: " + ImageConversion.LoadImage(tex[i], byteTexture, true).ToString());
                }
            break;

            case ImageType.Examenes:
                DownloadOneExam(numCycles-1);
            break;
        }
    }

    //Esto es el "Primero cargas uno y cargas otro con los botones", FUNCIONA
    public void DownloadOneExam(int index)
    {
        if (tex[index] == null)
        {
            byteTexture = File.ReadAllBytes(routeLocal + $"/ciclo examenes {index+1}.jpg");
            tex[index] = new Texture2D(32,32);
            Debug.Log($"Examenes {index+1}: " + ImageConversion.LoadImage(tex[index], byteTexture, true).ToString());
        }
    }
}
