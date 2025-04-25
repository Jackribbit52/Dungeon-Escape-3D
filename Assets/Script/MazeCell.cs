using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject leftWall;
    [SerializeField]
    private GameObject rightWall;
    [SerializeField]
    private GameObject frontWall;
    [SerializeField]
    private GameObject backWall;
    [SerializeField]
    private GameObject unvisitedBlock;

    public bool IsVisited { get; private set; }

    public int GetOpenWallCount()
    {
        int openCount = 0;
        if (!leftWall.activeSelf) openCount++;
        if (!rightWall.activeSelf) openCount++;
        if (!frontWall.activeSelf) openCount++;
        if (!backWall.activeSelf) openCount++;
        return openCount;
    }


    public void Visit()
    {
        IsVisited = true;
        unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall() { leftWall.SetActive(false); }
    public void ClearRightWall() { rightWall.SetActive(false); }
    public void ClearFrontWall() { frontWall.SetActive(false); }
    public void ClearBackWall() { backWall.SetActive(false); }
}
