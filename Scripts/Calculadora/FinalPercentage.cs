using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalPercentage : MonoBehaviour
{
    Text nf;
    public ECcalc ec1;
    public ECcalc ec2;

    private void Awake()
    {
        nf = GetComponent<Text>();
    }

    public void CalculateFinalNote()
    {
        //16,8  -> 8,5
        //18,1  -> 9
        //          -> 17,5  -> 18  El 18 no es necesario
        //  -> 8,4 + 9,05 -> 17,45
        float final = ec1.noteEC;   //No lo hago porque puedo y por debug
        if (ec2 != null) final = ec1.roundEC + ec2.roundEC; //Como se suman tenemos que redondearlos previamente

        nf.text = "Nota final : " + final.ToString();
    }
}