using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeInputFieldWithSpaces : MonoBehaviour
{
/*
    Empieza como Text, con 2 lineas
    Cuando lo presionas, se convierte en Input
    Luego vuelve a Text
*/
    public InputField input;
    public Text text2;

    private void Start()
    {
        ActivateText2();
    }

    public void ActivateInput()
    {
        input.textComponent.enabled = true;
        text2.enabled = false;
    }
    
    public void ActivateText2()
    {
        input.textComponent.enabled = false;
        text2.enabled = true;
        text2.text = input.text;
    }
}
