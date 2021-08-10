using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Slider slider;

    void Start()
    {
        slider.value = Mathf.Sqrt(PlayerPrefs.GetInt("difficulty"));
        
        slider.onValueChanged.AddListener((value) => {
            PlayerPrefs.SetInt("difficulty", (int)Mathf.Pow(value, 2));
        });
    }

    public void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void CloseGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
