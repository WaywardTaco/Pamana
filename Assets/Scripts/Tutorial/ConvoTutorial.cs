using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConvoTutorial : TutorialScript
{
    [SerializeField] private TextMeshProUGUI primaryText;
    [SerializeField] private TextMeshProUGUI secondaryText;
    [SerializeField] private TextMeshProUGUI clickText;
    private int count = 0;

    void OnEnable()
    {
        if (!hasOpenedPage)
        {
            hasOpenedPage = true;
        }
    }

    public void OnClick()
    {
        if(count == 0)
        {
            primaryText.gameObject.SetActive(false);
            secondaryText.gameObject.SetActive(true);
            clickText.text = "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nClick on this box to continue...";
        }
        else gameObject.SetActive(false);

        count++;
    }
}
