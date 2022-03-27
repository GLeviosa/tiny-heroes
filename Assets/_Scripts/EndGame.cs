using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
   
   public void RestartGame()
   {
       SceneManager.LoadScene("Level01");
   }

   public void Menu()
   {
       SceneManager.LoadScene("Menu");
   }
}