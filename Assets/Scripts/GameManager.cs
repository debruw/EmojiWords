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
    public int currentLevel, starCount;
    int MaxLevelNumber = 30;

    public bool isTexting;
    public Image lastText;
    public Sprite trueSprite, falseSprite;

    #region UIElements
    public Text LevelText, StarText;
    public GameObject LeanCanvas;
    public GameObject WinPanel, LosePanel;
    public GameObject TutorialCanvas;
    #endregion

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("LevelId");
        starCount = PlayerPrefs.GetInt("StarCount");
        StarText.text = starCount.ToString();
        LevelText.text = "Level " + currentLevel;
        if (currentLevel == 1 || currentLevel == 7 || currentLevel == 13)
        {
            if (TutorialCanvas != null)
            {
                TutorialCanvas.SetActive(true);
            }
        }
    }

    public void NextLevelClick()
    {
        if (currentLevel > MaxLevelNumber)
        {
            int rand = Random.Range(1, MaxLevelNumber);
            if (rand == PlayerPrefs.GetInt("LastRandomLevel"))
            {
                rand = Random.Range(1, MaxLevelNumber);
            }
            else
            {
                PlayerPrefs.SetInt("LastRandomLevel", rand);
            }
            SceneManager.LoadScene("Level" + rand);
        }
        else
        {
            SceneManager.LoadScene("Level" + currentLevel);
        }
    }

    public void RetryClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator WaitAndOpenWinPanel()
    {
        Confetti.SetActive(true);
        if (TutorialCanvas != null)
        {
            if (TutorialCanvas.activeSelf)
            {
                TutorialCanvas.SetActive(false);
            }
        }
        yield return new WaitForSeconds(1.5f);
        LeanCanvas.SetActive(false);
        WinPanel.SetActive(true);
        currentLevel++;
        PlayerPrefs.SetInt("LevelId", currentLevel);
        starCount += 2;
        PlayerPrefs.SetInt("StarCount", starCount);
        StarText.text = starCount.ToString();
    }

    IEnumerator WaitAndOpenLosePanel()
    {
        if (TutorialCanvas != null)
        {
            if (TutorialCanvas.activeSelf)
            {
                TutorialCanvas.SetActive(false);
            }
        }
        yield return new WaitForSeconds(1.5f);
        LosePanel.SetActive(true);
        LeanCanvas.SetActive(false);
    }

    #region Game type 1
    public void RightImageClicked()
    {
        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
        if (isTexting)
        {
            lastText.GetComponent<Animator>().enabled = false;
            lastText.GetComponent<Image>().sprite = trueSprite;
        }
        StartCoroutine(WaitAndOpenWinPanel());
    }

    public void WrongImageClicked()
    {
        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Medium);
        if (isTexting)
        {
            lastText.GetComponent<Animator>().enabled = false;
            lastText.GetComponent<Image>().sprite = falseSprite;
        }
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
                if (PlayerPrefs.GetInt("VIBRATION") == 1)
                    TapticManager.Impact(ImpactFeedback.Light);
                StartCoroutine(WaitAndOpenWinPanel());
            }
            else
            {//Wrong word
                if (PlayerPrefs.GetInt("VIBRATION") == 1)
                    TapticManager.Impact(ImpactFeedback.Medium);
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
