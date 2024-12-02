using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECcalc : MonoBehaviour
{
    [Header("Notas y la EC")]
    public float sumNotesPer = 0f;   //La suma de todas la Note-Per de la lista (Nota de toda la EC)
    public float noteEC = 0f;        //Nota de la EC * porcentaje
    public float roundEC = 0f;       //Nota de la EC redondeada * porcentaje    Tambien la Nota Final
   
   [Header("Componentes")]
    public InputField inputEC;      //El texto que sale en la parte de EC: "EC"
    public Text textNoteEC;         //El texto que sale en la parte de EC: "Nota EC:"

    [Header("Lista de NPs")]
    public List<NoteCalculator> listNP = new List<NoteCalculator>();
    int maxListNumber = 7;
    public string defaultECnote = "50";     //El porcentaje de cada EVC, en el OneNote vale 100

    [Header("Posicion de los objetos")]
    public float spaceMargin = 44f; //El espaciado entre cada NP y Add
    public Vector3 localNPPos;      //Posicion del Note-Per inicial
    public Vector3 localAddPos;     //Posicion del Add en su posicion inicial

    [Header("Pointers a otros objetos")]
    public GameObject prefabNP;
    public Button add;
    public FinalPercentage finalNoteText;
    public Text stringNameCourse;
    
    private void Awake()
    {
        inputEC.text = defaultECnote;
        if (listNP.Count == 1) listNP[0].delete.enabled = false;
    }

    public void CalculateEC()
    {
        sumNotesPer = 0f;
        foreach(NoteCalculator noteC in listNP)
        {
            sumNotesPer += noteC.note_per;
        }

        //En el caso de que se nos olvide de poner el porcentaje en la EVC
        if (inputEC.text == "") inputEC.text = defaultECnote;

        //Calcular
        noteEC = sumNotesPer * float.Parse(inputEC.text) / 100f;
        roundEC = MyRound(sumNotesPer) * float.Parse(inputEC.text) / 100f;
            //Mathf.Round(0,5f) == 1f

        if (textNoteEC != null)
        {
            textNoteEC.text = $"Nota EC: \n {sumNotesPer} \n ({MyRound(sumNotesPer)})";
        }

        //Actualizamos
        finalNoteText.CalculateFinalNote();
    }

    public float MyRound(float rounded)
    {
        //Debug.Log($"rounded:{rounded}   rounded % 1:{rounded%1}");
        if (rounded % 1 >= 0.5f) rounded = Mathf.Ceil(rounded);
        else rounded = Mathf.Floor(rounded);

        //Debug.Log($"new rounded:{rounded}");
        return rounded;
    }

    public void AddNotePercentageSimplified()
    {
        AddNotePercentage();
    }

    public void AddNotePercentage(int note = 15, int perc = 20)
    {
        //Crear el Prefab, posicionarlo, adoptarlo
        Vector3 newPos = localNPPos - Vector3.up * spaceMargin * listNP.Count;

        GameObject newNP = Instantiate(prefabNP, newPos, Quaternion.identity);
        newNP.transform.SetParent(gameObject.transform, false);
        
        //Establecer su parentEC y agregar a la lista
        NoteCalculator newNoteCalc = newNP.GetComponent<NoteCalculator>();
        newNoteCalc.parentEC = gameObject;
        newNoteCalc.defaultNote = note.ToString();
        newNoteCalc.defaultPesoNum = perc.ToString();
        listNP.Add(newNoteCalc);

        //Reposicionar el boton Add
        add.transform.localPosition = localAddPos - Vector3.up* spaceMargin * (listNP.Count - 1);

        //Desactivar el boton Add
        if (listNP.Count == maxListNumber) add.enabled = false;
        //Reactivar el boton de eliminar si hay 2 NPs
        if (listNP.Count == 2) listNP[0].delete.enabled = true;
        //Actualizamos
        CalculateEC();
    }

    public void DeleteNotePercentage(NoteCalculator deleteNote)
    {
        //Quitar la NP de la lista y de la existencia
        listNP.Remove(deleteNote);
        Destroy(deleteNote.gameObject);

        //Reposicionar los NPs y el boton Add
        for(int i = 0; i < listNP.Count; i++)
        {
            listNP[i].transform.localPosition = localNPPos - Vector3.up * spaceMargin * i;
        }
        add.transform.localPosition = localAddPos - Vector3.up* spaceMargin * (listNP.Count - 1);

        //Desactivar el boton de eliminar si solo hay un NP
        if (listNP.Count == 1) listNP[0].delete.enabled = false;
        //Activar el boton Add, no es necesario saber cuanto
        if (!add.enabled) add.enabled = true;
        //Actualizamos
        CalculateEC();
    }
}

// [CustomEditor(typeof(ECcalc))]
// public class ECcalcEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         ECcalc ec = (ECcalc)target;
        
//         // ec.spaceMargin = EditorGUILayout.FloatField("spaceMargin", ec.spaceMargin);
//         //     GUILayout.Space(24f);
//         // ec.localNPPos = EditorGUILayout.Vector3Field("localNPPos", ec.localNPPos);
//         // EditorGUILayout.Vector3Field("add.LocalPosition", ec.listNP[0].transform.localPosition);
//         // EditorGUILayout.Vector3Field("add.Position", ec.listNP[0].transform.position);
//         //     GUILayout.Space(24f);
//         // ec.localAddPos = EditorGUILayout.Vector3Field("localAddPos", ec.localAddPos);
//         // EditorGUILayout.Vector3Field("add.LocalPosition", ec.add.transform.localPosition);
//         // EditorGUILayout.Vector3Field("add.Position", ec.add.transform.position);
//     }
// }