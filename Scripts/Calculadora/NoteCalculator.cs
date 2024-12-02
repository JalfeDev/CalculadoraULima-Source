using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteCalculator : MonoBehaviour
{
    #region Notas
    //Es: nota * porcentaje / 100
    public float note_per = 0f;
    public string defaultNote = "15";
    public string defaultPesoNum = "30";
    [Space()]
    public InputField note;
    public InputField percentage;
    #endregion
    
    public Button delete;

    public GameObject parentEC;
    
    private void Start()
    {
        note.text = defaultNote;
        percentage.text = defaultPesoNum;
        CalculateNotePercentage();
    }

    public void CalculateNotePercentage()
    {
        if (note.text == "") note.text = defaultNote;
        if (percentage.text == "") percentage.text = defaultPesoNum;

        note_per = float.Parse(note.text) * float.Parse(percentage.text) / 100f;
        
        parentEC.SendMessage("CalculateEC");    //Lo actualizamos
    }

    public void CallParent()
    {
        //Jesse, escribe el void OnClick
        //Mr White, no existe, y si llamamos al EC desde el NP para enviarle el mensaje de destruir este NP con su script como argumento?
        parentEC.SendMessage("DeleteNotePercentage", this);
    }
}
