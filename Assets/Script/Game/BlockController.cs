using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private Block[] blocks;

    public delegate void OnBlockClicked(int row, int col);
    public OnBlockClicked OnBlockClickedDelegate;

    public void InitBlocks()
    {
        for(int i = 0; i < blocks.Length; i++)
        {
            blocks[i].InitMarker(i, onBlockClicked:blockIndex =>
            {
                var row = blockIndex / Constants.BlockColumCount;
                var col = blockIndex % Constants.BlockColumCount;
                OnBlockClickedDelegate?.Invoke(row, col);
            });
        }
    }

    public void PlaceMaker(Block.MarkerType markerType, int row, int col)
    {
        var blockIndex = row * Constants.BlockColumCount + col;
        blocks[blockIndex].SetMarker(markerType);
    }

    public void SetBlockColor()
    {

    }
}
