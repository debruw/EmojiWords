using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator WaitAndOpenWinPanel()
    {
        Confetti.SetActive(true);
        yield return new WaitForSeconds(1);
        LeanCanvas.SetActive(false);
        WinPanel.SetActive(true);
    }

    IEnumerator WaitAndOpenLosePanel()
    {
        yield return new WaitForSeconds(1);
        LosePanel.SetActive(true);
        LeanCanvas.SetActive(false);
    }

    #region Game type 1
    public void RightImageClicked()
    {
        StartCoroutine(WaitAndOpenWinPanel());
    }

    public void WrongImageClicked()
    {
        StartCoroutine(WaitAndOpenLosePanel());
    }

    #endregion

    #region Game type 2
    Transform firstRectTransform;
    GameObject selectedDragBox;
    bool isOnHower = false;
    bool isHoldingDragBox = false;
    public GameObject WrongPanel;

    public void DragboxOnBegin(GameObject sdb)
    {
        firstRectTransform = sdb.transform;
        selectedDragBox = sdb;
        isHoldingDragBox = true;
    }

    public Text HoverText;
    public void DragboxOnEnd(bool bl)
    {
        isHoldingDragBox = false;
        SendRaycast();

        if (isOnHower)
        {//Right place
            if (bl)
            {//Right word                
                HoverText.text = selectedDragBox.GetComponentInChildren<Text>().text;
                selectedDragBox.SetActive(false);
                StartCoroutine(WaitAndOpenWinPanel());
            }
            else
            {//Wrong word
                WrongPanel.GetComponent<Animation>().Play();
                StartCoroutine(WaitAndOpenLosePanel());
            }
        }
        else
        {//Wrong place
            selectedDragBox.transform.position = firstRectTransform.transform.position;
        }
    }

    public GraphicRaycaster gr;
    public void SendRaycast()
    {
        //Create the PointerEventData with null for the EventSystem
        PointerEventData ped = new PointerEventData(null);
        //Set required parameters, in this case, mouse position
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //Create list to receive all results
        List<RaycastResult> results = new List<RaycastResult>();
        //Raycast it
        gr.Raycast(ped, results);
        foreach (RaycastResult rr in results)
        {
            if (rr.gameObject.CompareTag("Hover"))
            {
                isOnHower = true;
            }
        }
    }

    #endregion

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
