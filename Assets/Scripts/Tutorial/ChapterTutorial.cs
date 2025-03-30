using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChapterTutorial : TutorialScript
{
    void OnEnable()
    {
        if (!hasOpenedPage)
        {
            hasOpenedPage = true;
        }
    }

    public void OnClick()
    {
        this.gameObject.SetActive(false);
    }
}
