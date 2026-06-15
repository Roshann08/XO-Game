using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public GameObject startPanel; // the UI panel to hide
    public GameObject boardContainer; // the GameObject that contains the Board
    public Main main; // reference to Main
    public Button startButton;
    public Button quitButton;

    void Start()
    {
        // Ensure panel visible and board hidden at scene start
        if (startPanel != null) startPanel.SetActive(true);
        if (boardContainer != null) boardContainer.SetActive(false);

        if (startButton != null) startButton.onClick.AddListener(OnStartPressed);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuitPressed);
    }

    public void OnStartPressed()
    {
        if (startPanel != null) startPanel.SetActive(false);
        if (boardContainer != null) boardContainer.SetActive(true);
        if (main != null) main.InitGame();
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}