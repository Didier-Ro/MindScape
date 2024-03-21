using UnityEngine;
using UnityEngine.UI;


public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
       Uniform, 
       Width,
       Height,
       FixedRows,
       FixedColumms
    }

    public FitType fitType;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public bool fitX;
    public bool fitY;
    
    public override void CalculateLayoutInputVertical()
    {
        
       
    }
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform )
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumms )
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }

        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parenHeight = rectTransform.rect.height;

        float cellWidth = parentWidth / (float)columns - ((spacing.x / (float)columns)*2) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = parenHeight / (float)rows - ((spacing.y / (float)rows)*2) - (padding.top / (float)rows) - (padding.bottom / (float) rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int index = 0; index < rectChildren.Count; index++)
        {
            rowCount = index / columns;
            columnCount = index % columns;

            var item = rectChildren[index];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
            
            SetChildAlongAxis(item, 0 ,xPos, cellSize.x);
            SetChildAlongAxis(item, 1 ,yPos, cellSize.y);
            

        }
    }

    public override void SetLayoutHorizontal()
    {
      
    }

    public override void SetLayoutVertical()
    {
        
    }
}
