using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ConvertImagesRedirected : MonoBehaviour
{
    [Tooltip("Aqui se guardaran las imagenes locales")]
    public Texture2D[] examenes;
    public Texture2D[] horario;

    public Text textConvenient;

    [Tooltip("Esto son los datos de una imagen local")]
    private byte[] byteTexture;
    
    public bool RewriteImages(CloudImages cloudImages)
    {
        bool wasWell = true;
        try
        {
            int numCycles = PlayerPrefs.GetInt("NumCyclesImages");

            for (int i=0; i < numCycles; i++)
            {
                byteTexture = File.ReadAllBytes(cloudImages.Ciclos[i]);
                wasWell = wasWell && examenes[i].LoadImage(byteTexture, false);
                examenes[i].Apply(true, false);

                byteTexture = File.ReadAllBytes(cloudImages.Horarios[i]);
                    //wasWell = wasWell && ImageConversion.LoadImage(horario[i], byteTexture, false);
                wasWell = wasWell && horario[i].LoadImage(byteTexture, false);
                horario[i].Apply(true, false);
            }
            Debug.Log($"Estuvo: {wasWell}");
        }
        catch (Exception e)
        {
            wasWell = false;
            textConvenient.text = e.ToString();
        }
        return wasWell;
    }
}
