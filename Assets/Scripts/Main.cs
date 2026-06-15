using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{
    public Board mBoard;
    public GameObject mWinner;

    // Audio
    public AudioSource mMusicSource; // background music AudioSource (looping)
    public AudioSource mSfxSource;   // SFX AudioSource (used for PlayOneShot)
    public AudioClip mClickX;        // clip to play when X is placed
    public AudioClip mClickO;        // clip to play when O is placed
    public AudioClip mWinClip;       // clip to play when there is a winner
    public AudioClip mDrawClip;      // clip to play when it's a draw (optional)

    private bool mXTurn = true;
    private int mTurnCount = 0;

    void Awake()
    {
        // Hide the OS cursor visually but do NOT lock it so UI mouse clicks still work.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

        // Start panel will control when the game appears.
        if (mWinner != null) mWinner.SetActive(false);
        if (mBoard != null) mBoard.gameObject.SetActive(false);
    }

    void Start()
    {
        // Play background music immediately when the scene starts (before Start button)
        if (mMusicSource != null)
        {
            if (mMusicSource.clip != null)
            {
                mMusicSource.loop = true;
                if (!mMusicSource.isPlaying)
                    mMusicSource.Play();
            }
            else
            {
                Debug.LogWarning("Main.Start: mMusicSource has no AudioClip assigned.");
            }
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Re-apply hiding the cursor (visual only) when focus returns to the app.
        if (hasFocus)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnDestroy()
    {
        // Restore cursor visibility when the scene/object is destroyed (good practice)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Called by StartPanel when Start is pressed
    public void InitGame()
    {
        mXTurn = true;
        mTurnCount = 0;

        if (mBoard != null)
        {
            mBoard.gameObject.SetActive(true);
            mBoard.Build(this);
        }

        if (mWinner != null) mWinner.SetActive(false);

        // Ensure music is playing when the game starts (safe-guard)
        if (mMusicSource != null && mMusicSource.clip != null && !mMusicSource.isPlaying)
        {
            mMusicSource.loop = true;
            mMusicSource.Play();
        }
    }

    public void Switch()
    {
        mTurnCount++;

        bool hasWinner = mBoard.CheckForWinner();

        if (hasWinner || mTurnCount == 9)
        {
            // end game
            StartCoroutine(EndGame(hasWinner));
            return;
        }
        mXTurn = !mXTurn;
    }

    public string GetTurnCharacter()
    {
        return mXTurn ? "X" : "O";
    }

    // Called by cells to play click sounds
    public void PlayClick(string placedChar)
    {
        if (mSfxSource == null)
        {
            Debug.LogWarning("Main.PlayClick: mSfxSource is not assigned.");
            return;
        }

        AudioClip clipToPlay = null;
        if (placedChar == "X")
            clipToPlay = mClickX;
        else if (placedChar == "O")
            clipToPlay = mClickO;

        if (clipToPlay != null)
        {
            mSfxSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.LogWarning($"Main.PlayClick: No clip assigned for '{placedChar}'.");
        }
    }

    private IEnumerator EndGame(bool hasWinner)
    {
        if (mWinner == null)
        {
            Debug.LogError("Main.EndGame: mWinner GameObject is not assigned.");
            yield break;
        }

        TMP_Text winnerLabel = mWinner.GetComponentInChildren<TMP_Text>();
        if (winnerLabel != null)
        {
            if (hasWinner)
            {
                winnerLabel.text = GetTurnCharacter() + " Wins!";
            }
            else
            {
                winnerLabel.text = "It's a Draw!";
            }
        }
        else
        {
            Debug.LogWarning("Main.EndGame: No TMP_Text found under mWinner.");
        }

        mWinner.SetActive(true);

        // Play winning/draw sound when the result is shown
        if (mSfxSource != null)
        {
            if (hasWinner && mWinClip != null)
            {
                mSfxSource.PlayOneShot(mWinClip);
            }
            else if (!hasWinner && mDrawClip != null)
            {
                mSfxSource.PlayOneShot(mDrawClip);
            }
        }
        else
        {
            Debug.LogWarning("Main.EndGame: mSfxSource is not assigned; can't play win/draw sound.");
        }

        yield return new WaitForSeconds(3f);

        mBoard.Reset();
        mTurnCount = 0;
        mWinner.SetActive(false);
    }
}