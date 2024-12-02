using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public enum TransitionType {ToLeft, ToRight, ToUp, ToDown, None};

public class CameraTransition : MonoBehaviour
{
    public Animator animator;
    public float transitionTime = 0.3f;
    public TextAsset textAsset;

    public string currentPath;

    //Al ser publica y estatica, pueden accederlo todos
    //Al ser private set, solo un componente podra definirlo
    public static CameraTransition Instance {get; private set;}

    void Awake()
    {
        //Crea el IndexCourse, que es el indice para el array de Cursos
        if (!PlayerPrefs.HasKey("IndexCourse")) PlayerPrefs.SetInt("IndexCourse", 0);
        if (!PlayerPrefs.HasKey("IndexSemester")) PlayerPrefs.SetInt("IndexSemester", 2);
        if (!PlayerPrefs.HasKey("NumCyclesImages")) PlayerPrefs.SetInt("NumCyclesImages", 3);
        
        //Escoge la plataforma que se va a usar
        currentPath = CreateJsonInAllPaths("CollegeNotes.json", textAsset);
        
        //Para los Singletons
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);    
        }
    }

    public static string CreateJsonInAllPaths(string nameFile, TextAsset asset)
    {
        if (Application.isMobilePlatform)
        {
            string curPath = Application.persistentDataPath + "/" + nameFile;
            //The JSON file already exists, altough IDK where :P
            if (!File.Exists(curPath)) File.WriteAllText(curPath, asset.text);
            return curPath;
        }
        else return Application.dataPath + "/" + nameFile;
    }

    public IEnumerator StartTransition(string TransScene)
    {
        TransitionType typeOut = (TransitionType)int.Parse( TransScene[0].ToString() );
        string scene = TransScene.Substring(1);

        if (typeOut != TransitionType.None)
        {
            animator.Play(typeOut.ToString());
            yield return new WaitForSeconds(transitionTime);
        }
        SceneManager.LoadScene(scene);
    }
}

//[SerializeField]
[System.Serializable]
public class Degree
{
    public string degreeName;
    public List<Semester> ciclos;
}

//[SerializeField]
[System.Serializable]
public class Semester
{
    public int numCiclo;
    public float notaSemestre;
    public List<CourseNotes> Courses;

    public Semester(int numCiclo, float notaSemestre, List<CourseNotes> Courses)
    {
        this.numCiclo = numCiclo;
        this.notaSemestre = notaSemestre;
        this.Courses = Courses;
    }
}

//[SerializeField]
[System.Serializable]
public class CourseNotes
{
    public string curso;
    public List<int> nota;
    public List<int> porcentaje; //En %
    public int notaFinal;
    public int credito;

    public CourseNotes(string curso, List<int> nota, List<int> porcentaje, int notaFinal, int credito)
    {
        this.curso = curso;
        this.nota = nota;
        this.porcentaje = porcentaje;
        this.notaFinal = notaFinal;
        this.credito = credito;
    }
}

/*[CustomEditor(typeof(CameraTransition))]
public class CameraTransitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraTransition cameraT = (CameraTransition)target;
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("MoveToLeft") || Input.GetKeyDown(KeyCode.A)) cameraT.animator.Play("ToLeft");
        else if (GUILayout.Button("MoveToRight") || Input.GetKeyDown(KeyCode.D)) cameraT.animator.Play("ToRight");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("MoveToUp") || Input.GetKeyDown(KeyCode.W)) cameraT.animator.Play("ToUp");
        else if (GUILayout.Button("MoveToDown") || Input.GetKeyDown(KeyCode.S)) cameraT.animator.Play("ToDown");
        GUILayout.EndHorizontal();
    }
}*/