using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;
    private Cell[] mCells = new Cell[9];
    private bool mBuilt = false;

    public void Build(Main main)
    {
        if (mBuilt) return;
        mBuilt = true;

        for (int i = 0; i <= 8; i++)
        {
            GameObject newCell = Instantiate(mCellPrefab, transform);

            mCells[i] = newCell.GetComponent<Cell>();
            mCells[i].mMain = main;
        }
    }

    public void Reset()
    {
        for (int i = 0; i < mCells.Length; i++)
        {
            var cell = mCells[i];
            if (cell == null) continue;

            if (cell.mLabel != null) cell.mLabel.text = "";
            if (cell.mButton != null) cell.mButton.interactable = true;
        }
    }

    public bool CheckForWinner()
    {
        int i = 0;

        //Horizontal
        for (i = 0; i <= 6; i += 3)
        {
            if (!CheckValues(i, i + 1))
                continue;

            if (!CheckValues(i, i + 2))
                continue;

            return true;
        }

        //Vertical
        for (i = 0; i <= 2; i++)
        {
            if (!CheckValues(i, i + 3))
                continue;

            if (!CheckValues(i, i + 6))
                continue;

            return true;
        }

        // left Diagonal
        if (CheckValues(0, 4) && CheckValues(0, 8))
            return true;

        // right Diagonal
        if (CheckValues(2, 4) && CheckValues(2, 6))
            return true;

        return false;
    }

    private bool CheckValues(int firstindex, int secondindex)
    {
        string firstValue = mCells[firstindex]?.mLabel?.text ?? "";
        string secondValue = mCells[secondindex]?.mLabel?.text ?? "";

        if (firstValue == "" || secondValue == "")
            return false;

        return firstValue == secondValue;
    }
}