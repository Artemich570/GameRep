using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] Toggle toggleBlack, toggleWhite, toggleRed, toggleYellow;
    public void ChangeBlack()
    {
        if (toggleBlack.isOn) { DataHolder.teams[0] = "Black"; }
        else { DataHolder.teams[0] = null; }
    }
    public void ChangeWhite()
    {
        if (toggleWhite.isOn) { DataHolder.teams[1] = "White"; }
        else { DataHolder.teams[1] = null; }
    }
    public void ChangeRed()
    {
        if (toggleRed.isOn) { DataHolder.teams[2] = "Red"; }
        else { DataHolder.teams[2] = null; }
    }
    public void ChangeYellow()
    {
        if (toggleYellow.isOn) { DataHolder.teams[3] = "Yellow"; }
        else { DataHolder.teams[3] = null; }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
