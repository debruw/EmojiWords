using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Confetti;

    #region UIElements
    public GameObject LeanCanvas;
    public GameObject WinPanel, LosePanel;

    #endregion

    public void NextLevelClick()
    {

    }

    public void RetryClick()
    {

    }

    public void RightImageClicked()
    {        
        Confetti.SetActive(true);
        StartCoroutine(WaitAndOpenWinPanel());
    }

    IEnumerator WaitAndOpenWinPanel()
    {
        yield return new WaitForSeconds(1);
        LeanCanvas.SetActive(false);
        WinPanel.SetActive(true);
    }

    public void WrongImageClicked()
    {
        StartCoroutine(WaitAndOpenLosePanel());
    }

    IEnumerator WaitAndOpenLosePanel()
    {
        yield return new WaitForSeconds(1);
        LosePanel.SetActive(true);
        LeanCanvas.SetActive(false);
    }

    public GameObject VibrationButtonImage;
    public Sprite on, off;
    public void VibrateButtonClick()
    {
        if (PlayerPrefs.GetInt("VIBRATION").Equals(1))
        {//Vibration is on
            PlayerPrefs.SetInt("VIBRATION", 0);
            VibrationButtonImage.GetComponent<Image>().sprite = off;
        }
        else
        {//Vibration is off
            PlayerPrefs.SetInt("VIBRATION", 1);
            VibrationButtonImage.GetComponent<Image>().sprite = on;
        }

        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
    }
}
