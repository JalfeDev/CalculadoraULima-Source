using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NoteStorage : MonoBehaviour
{
    [Header("External Variables")]
    public ECcalc eCcalc;
    private CourseNotes curCourse;   //Solo para escribir menos, un atajo

    [Header("JSON Management")]
    public Degree carrera;
    [TextArea()] public string stringCollegeNotes;

    void Awake()
    {
        stringCollegeNotes = File.ReadAllText( CameraTransition.Instance.currentPath );
        carrera = JsonUtility.FromJson<Degree>(stringCollegeNotes);
        
        curCourse = carrera.ciclos[PlayerPrefs.GetInt("IndexSemester")].Courses[PlayerPrefs.GetInt("IndexCourse")];
    }

    void Start() {
        SetCourseNotes();
        //Actualizamos
        eCcalc.CalculateEC();
    }

    public void SetCourseNotes()
    {
        for(int i = 0; i < curCourse.nota.Count; i++)
        {
            eCcalc.AddNotePercentage(curCourse.nota[i], curCourse.porcentaje[i]);
        }
        eCcalc.DeleteNotePercentage(eCcalc.listNP[0]);

        eCcalc.stringNameCourse.text = curCourse.curso;
    }

    public void SaveJson()
    {
        curCourse.nota.Clear();
        curCourse.porcentaje.Clear();
        for(int i = 0; i < eCcalc.listNP.Count; i++)
        {
            curCourse.nota.Add(int.Parse(eCcalc.listNP[i].note.text));
            curCourse.porcentaje.Add(int.Parse(eCcalc.listNP[i].percentage.text));
        }
        curCourse.notaFinal = (int)eCcalc.roundEC;

        stringCollegeNotes = JsonUtility.ToJson(carrera);
        File.WriteAllText( CameraTransition.Instance.currentPath, stringCollegeNotes);
    }
}