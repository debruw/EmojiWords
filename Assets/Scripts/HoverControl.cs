using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverControl : MonoBehaviour
{
    public GameObject Object1, Object2, Object3;
    public GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void TriggerPanelOpen()
    {
        if (gm.currentLevel == 13)
        {
            if (gm.TutorialCanvas != null)
            {
                gm.TutorialCanvas.SetActive(true);
            }
        }
        Object1.SetActive(true);
        Object2.SetActive(true);
        Object3.SetActive(true);
    }
}
