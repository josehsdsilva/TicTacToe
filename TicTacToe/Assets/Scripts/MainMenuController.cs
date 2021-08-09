using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Slider slider;

    void Start()
    {
        PlayerPrefs.SetInt("difficulty", 0);
        slider.onValueChanged.AddListener((value) => {
            PlayerPrefs.SetInt("difficulty", (int)Mathf.Pow(value, 2));
        });
    }

    public void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

}
