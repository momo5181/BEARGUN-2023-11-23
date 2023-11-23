using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class menuPlay : MonoBehaviour
{
   void Update()
   {
    Cursor.visible=true;
    Cursor.lockState=0;
   } 



    public void PlayGame()
    {
        SceneManager.LoadScene(0);
    }
    public void QUitGame()
    {
        Application.Quit();
        EditorApplication.isPlaying = false ;

    }


}
