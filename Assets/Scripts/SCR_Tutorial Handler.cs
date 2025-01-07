using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_TutorialHandler : MonoBehaviour
{
    private bool isGamePaused;
    public bool isStarterTutorial;
    public GameObject Canvas, Slides;
    [SerializeField] private TextMeshProUGUI slidenumberText;
    private int amountSlides, currentSlide;
    
    void Start()
    {
        if (isStarterTutorial)
        {
            PauseGame();
        }
        foreach (Transform child in Slides.transform)
        {
            amountSlides++;
        }

        currentSlide = 1;
        updateSlides();
    }
    
    //When the button to open/close the tutorial is pressed
    //check the timescale of the game
    public void Tutorialbutton()
    {
        if (!isStarterTutorial)
        {
            if (!isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        else
        {
            ResumeGame();
        }
        
    }

    private void PauseGame()
    {
        Canvas.gameObject.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0;
        
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        Canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        
    }

    private void setText(int current, int amount)
    {
        slidenumberText.text = current.ToString() + "/" + amount.ToString();
    }

    public void NextSlide()
    {
        if (currentSlide < amountSlides)
        {
            currentSlide++;
            updateSlides();
        }
    }

    public void PreviousSlide()
    {
        if (currentSlide > 1)
        {
            currentSlide--;
            updateSlides();
        }
    }

    private void updateSlides()
    {
        setText(currentSlide, amountSlides);
        
        for (int i = 1; i <= amountSlides; i++)
        {
            if (i == currentSlide)
            {
                Slides.transform.GetChild(i-1).gameObject.SetActive(true);
            }
            else
            {
                Slides.transform.GetChild(i-1).gameObject.SetActive(false);
            }
        }
    }
}
