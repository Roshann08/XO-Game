using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cell : MonoBehaviour
{
    public TextMeshProUGUI mLabel;
    public Button mButton;
    public Main mMain;

    public void Fill()
    {
        mButton.interactable = false;

        // set label to current character
        string placed = mMain.GetTurnCharacter();
        mLabel.text = placed;

        // play click sound for the placed character (before switching)
        if (mMain != null)
            mMain.PlayClick(placed);

        mMain.Switch();
    }
}