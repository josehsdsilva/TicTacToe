using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public void GoToHome()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
