using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectLayoutGroup : MonoBehaviour
{
    [System.Serializable]
    public enum Corner
    {
        UpperLeft = 0,
        UpperRight = 1,
        LowerLeft = 2,
        LowerRight = 3
    }

    [System.Serializable]
    public enum Axis
    {
        Horizontal = 0,
        Vertical = 1
    }

    [System.Serializable]
    public enum Constraint
    {
        Flexible = 0,
        FixedColumnCount = 1,
        FixedRowCount = 2
    }

    [System.Serializable]
    public enum ChildAlignment
    {
        UpperLeft = 0,
        UpperCenter = 1,
        UpperRight = 2,
        MiddleLeft = 3,
        MiddleCenter = 4,
        MiddleRight = 5,
        LowerLeft = 6,
        LowerCenter = 7,
        LowerRight = 8
    }

    [Header("Grid Layout Settings")]
    [SerializeField] protected Corner startCorner = Corner.UpperLeft;
    [SerializeField] protected Axis startAxis = Axis.Horizontal;
    [SerializeField] protected Vector3 cellSize = Vector2.one; // Size on AX plane (A=X, X=Z in 3D)
    [SerializeField] protected Vector2 spacing = Vector2.zero; // Spacing on AX plane
    [SerializeField] protected Constraint constraint = Constraint.FixedColumnCount;
    [SerializeField] protected int constraintCount = 3;
    [SerializeField] protected ChildAlignment childAlignment = ChildAlignment.MiddleCenter; // Default to center alignment

    [Header("3D Specific Settings")]
    [SerializeField] protected float yPosition = 0f; // Fixed Y height for all grid items
    [SerializeField] protected bool autoUpdateLayout = true;
    [SerializeField] protected bool includeInactiveChildren = false;

    // Grid state
    protected List<Transform> managedChildren = new List<Transform>();
    protected bool layoutNeedsUpdate = true;

    // Public properties matching UI GridLayoutGroup interface
    public Corner StartCorner
    {
        get => startCorner;
        set { startCorner = value; SetLayoutDirty(); }
    }

    public Axis StartAxis
    {
        get => startAxis;
        set { startAxis = value; SetLayoutDirty(); }
    }

    public Vector3 CellSize
    {
        get => cellSize;
        set { cellSize = value; SetLayoutDirty(); }
    }

    public Vector2 Spacing
    {
        get => spacing;
        set { spacing = value; SetLayoutDirty(); }
    }

    public Constraint ConstraintType
    {
        get => constraint;
        set { constraint = value; SetLayoutDirty(); }
    }

    public int ConstraintCount
    {
        get => constraintCount;
        set { constraintCount = Mathf.Max(1, value); SetLayoutDirty(); }
    }

    public ChildAlignment Alignment
    {
        get => childAlignment;
        set { childAlignment = value; SetLayoutDirty(); }
    }

    public float YPosition
    {
        get => yPosition;
        set { yPosition = value; SetLayoutDirty(); }
    }

    protected void Start()
    {
        SetLayoutDirty();
    }

    protected void Update()
    {
        if (autoUpdateLayout && layoutNeedsUpdate)
        {
            UpdateLayout();
        }
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        constraintCount = Mathf.Max(1, constraintCount);
        SetLayoutDirty();
    }
#endif
    /// <summary>
    /// Mark the layout as needing an update
    /// </summary>
    public void SetLayoutDirty()
    {
        layoutNeedsUpdate = true;
    }

    /// <summary>
    /// Force immediate layout update
    /// </summary>
    [ContextMenu("Update Layout")]
    public void UpdateLayout()
    {
        CollectManagedChildren();
        CalculateAndApplyLayout();
        layoutNeedsUpdate = false;
    }

    /// <summary>
    /// Collect all children that should be managed by this layout group
    /// </summary>
    private void CollectManagedChildren()
    {
        managedChildren.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Skip inactive children if not included
            if (!includeInactiveChildren && !child.gameObject.activeInHierarchy)
                continue;

            managedChildren.Add(child);
        }
    }

    /// <summary>
    /// Calculate grid positions and apply them to managed children
    /// Uses AX coordinate system where Y is height (not XY)
    /// Applies child alignment offset to center or align the grid as needed
    /// </summary>
    private void CalculateAndApplyLayout()
    {
        if (managedChildren.Count == 0)
            return;

        SetChildSize();
        int columnCount, rowCount;
        CalculateGridDimensions(out columnCount, out rowCount);

        // Calculate alignment offset to center or position the grid
        Vector3 alignmentOffset = CalculateAlignmentOffset(columnCount, rowCount);

        for (int i = 0; i < managedChildren.Count; i++)
        {
            int gridCol, gridRow;
            GetGridCoordinatesForIndex(i, columnCount, rowCount, out gridCol, out gridRow);

            Vector3 worldPosition = CalculateWorldPosition(gridCol, gridRow);
            // Apply alignment offset to position the entire grid according to childAlignment
            managedChildren[i].localPosition = worldPosition + alignmentOffset;
        }
    }

    protected void SetChildSize()
    {
        for (int i = 0; i < managedChildren.Count; i++)
        {
            managedChildren[i].localScale = CellSize;
        }
    }
    /// <summary>
    /// Calculate grid dimensions based on constraint settings
    /// Mimics UI GridLayoutGroup behavior
    /// </summary>
    protected void CalculateGridDimensions(out int columnCount, out int rowCount)
    {
        int childCount = managedChildren.Count;

        switch (constraint)
        {
            case Constraint.FixedColumnCount:
                columnCount = constraintCount;
                rowCount = Mathf.CeilToInt((float)childCount / columnCount);
                break;

            case Constraint.FixedRowCount:
                rowCount = constraintCount;
                columnCount = Mathf.CeilToInt((float)childCount / rowCount);
                break;

            case Constraint.Flexible:
            default:
                // For flexible, create roughly square grid
                columnCount = Mathf.CeilToInt(Mathf.Sqrt(childCount));
                rowCount = Mathf.CeilToInt((float)childCount / columnCount);
                break;
        }
    }

    /// <summary>
    /// Calculate the alignment offset to position the entire grid according to childAlignment
    /// This centers or aligns the grid relative to the transform position
    /// </summary>
    protected Vector3 CalculateAlignmentOffset(int columnCount, int rowCount)
    {
        // Calculate total grid size
        float totalWidth = (columnCount - 1) * (cellSize.x + spacing.x);
        float totalDepth = (rowCount - 1) * (cellSize.y + spacing.y);

        Vector3 offset = Vector3.zero;

        // Calculate horizontal (A-axis / X-axis) alignment
        switch (childAlignment)
        {
            case ChildAlignment.UpperLeft:
            case ChildAlignment.MiddleLeft:
            case ChildAlignment.LowerLeft:
                // Left aligned - no horizontal offset needed
                offset.x = 0f;
                break;

            case ChildAlignment.UpperCenter:
            case ChildAlignment.MiddleCenter:
            case ChildAlignment.LowerCenter:
                // Center aligned horizontally
                offset.x = -totalWidth * 0.5f;
                break;

            case ChildAlignment.UpperRight:
            case ChildAlignment.MiddleRight:
            case ChildAlignment.LowerRight:
                // Right aligned
                offset.x = -totalWidth;
                break;
        }

        // Calculate vertical (X-axis / Z-axis) alignment  
        switch (childAlignment)
        {
            case ChildAlignment.UpperLeft:
            case ChildAlignment.UpperCenter:
            case ChildAlignment.UpperRight:
                // Upper aligned - no vertical offset needed (assuming upper = forward)
                offset.z = 0f;
                break;

            case ChildAlignment.MiddleLeft:
            case ChildAlignment.MiddleCenter:
            case ChildAlignment.MiddleRight:
                // Middle aligned vertically
                offset.z = -totalDepth * 0.5f;
                break;

            case ChildAlignment.LowerLeft:
            case ChildAlignment.LowerCenter:
            case ChildAlignment.LowerRight:
                // Lower aligned
                offset.z = -totalDepth;
                break;
        }

        return offset;
    }

    /// <summary>
    /// Get grid coordinates for a child at given index
    /// </summary>
    private void GetGridCoordinatesForIndex(int index, int columnCount, int rowCount, out int col, out int row)
    {
        if (startAxis == Axis.Horizontal)
        {
            // Fill horizontally first
            col = index % columnCount;
            row = index / columnCount;
        }
        else
        {
            // Fill vertically first
            row = index % rowCount;
            col = index / rowCount;
        }
    }

    /// <summary>
    /// Calculate world position for grid coordinates using AX plane (Y is height)
    /// A-axis = X-axis in world space
    /// X-axis = Z-axis in world space  
    /// Y-axis = Y-axis in world space (height)
    /// </summary>
    protected virtual Vector3 CalculateWorldPosition(int gridCol, int gridRow)
    {
        // Calculate position on AX plane
        // A-axis (columns) maps to world X-axis
        // X-axis (rows) maps to world Z-axis
        float worldX = gridCol * (cellSize.x + spacing.x);
        float worldZ = gridRow * (cellSize.y + spacing.y); // cellSize.y represents X-axis spacing in AX system

        // Apply start corner adjustments for AX coordinate system
        Vector3 position = new Vector3(worldX, yPosition, worldZ);

        switch (startCorner)
        {
            case Corner.LowerLeft:
                // Standard: A+ right, X+ forward (into screen)
                position = new Vector3(worldX, yPosition, worldZ);
                break;
            case Corner.LowerRight:
                // Flip A-axis: A+ left, X+ forward
                position = new Vector3(-worldX, yPosition, worldZ);
                break;
            case Corner.UpperLeft:
                // Flip X-axis: A+ right, X+ backward
                position = new Vector3(worldX, yPosition, -worldZ);
                break;
            case Corner.UpperRight:
                // Flip both: A+ left, X+ backward
                position = new Vector3(-worldX, yPosition, -worldZ);
                break;
        }

        return position;
    }

    /// <summary>
    /// Get grid coordinates from a world position (useful for hit testing)
    /// </summary>
    public virtual bool GetGridCoordinatesFromWorldPos(Vector3 worldPosition, out int col, out int row)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);

        // Convert back from world space to grid coordinates
        float gridX, gridZ;

        switch (startCorner)
        {
            case Corner.LowerLeft:
                gridX = localPos.x;
                gridZ = localPos.z;
                break;
            case Corner.LowerRight:
                gridX = -localPos.x;
                gridZ = localPos.z;
                break;
            case Corner.UpperLeft:
                gridX = localPos.x;
                gridZ = -localPos.z;
                break;
            case Corner.UpperRight:
                gridX = -localPos.x;
                gridZ = -localPos.z;
                break;
            default:
                col = 0;
                row = 0;
                return false;
        }

        col = Mathf.RoundToInt(gridX / (cellSize.x + spacing.x));
        row = Mathf.RoundToInt(gridZ / (cellSize.y + spacing.y));

        return col >= 0 && row >= 0;
    }

    /// <summary>
    /// Get world position for specific grid coordinates
    /// Now includes alignment offset for accurate positioning
    /// </summary>
    public virtual Vector3 GetWorldPositionForGrid(int col, int row)
    {
        // We need to calculate alignment offset based on current grid dimensions
        int columnCount, rowCount;
        if (managedChildren.Count > 0)
        {
            CalculateGridDimensions(out columnCount, out rowCount);
        }
        else
        {
            // Use constraint count as fallback
            columnCount = constraintCount;
            rowCount = constraintCount;
        }

        Vector3 alignmentOffset = CalculateAlignmentOffset(columnCount, rowCount);
        Vector3 localPos = CalculateWorldPosition(col, row) + alignmentOffset;
        return transform.TransformPoint(localPos);
    }

    /// <summary>
    /// Manually add a child at specific grid position
    /// Now includes alignment offset for proper positioning
    /// </summary>
    public void SetChildAtGridPosition(Transform child, int col, int row)
    {
        child.SetParent(transform);

        // Calculate alignment offset for proper positioning
        int columnCount, rowCount;
        if (managedChildren.Count > 0)
        {
            CalculateGridDimensions(out columnCount, out rowCount);
        }
        else
        {
            columnCount = constraintCount;
            rowCount = constraintCount;
        }

        Vector3 alignmentOffset = CalculateAlignmentOffset(columnCount, rowCount);
        child.localPosition = CalculateWorldPosition(col, row) + alignmentOffset;
    }

    /// <summary>
    /// Get the total bounds of the grid layout
    /// </summary>
    public virtual Bounds GetLayoutBounds()
    {
        if (managedChildren.Count == 0)
            return new Bounds(transform.position, Vector3.zero);

        int columnCount, rowCount;
        CalculateGridDimensions(out columnCount, out rowCount);

        // Calculate total size in world space
        float totalWidth = columnCount * cellSize.x + (columnCount - 1) * spacing.x;
        float totalDepth = rowCount * cellSize.y + (rowCount - 1) * spacing.y;

        Vector3 size = new Vector3(totalWidth, 0.1f, totalDepth); // Small Y size for visualization
        return new Bounds(transform.position, size);
    }

    /// <summary>
    /// Debug visualization in Scene view
    /// Shows the aligned grid layout
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;

        int columnCount, rowCount;
        if (managedChildren.Count > 0)
        {
            CalculateGridDimensions(out columnCount, out rowCount);
        }
        else
        {
            // Show preview based on constraint
            switch (constraint)
            {
                case Constraint.FixedColumnCount:
                    columnCount = constraintCount;
                    rowCount = 3; // Preview 3 rows
                    break;
                case Constraint.FixedRowCount:
                    rowCount = constraintCount;
                    columnCount = 3; // Preview 3 columns
                    break;
                default:
                    columnCount = 3;
                    rowCount = 3;
                    break;
            }
        }

        // Calculate alignment offset for proper gizmo positioning
        Vector3 alignmentOffset = CalculateAlignmentOffset(columnCount, rowCount);

        // Draw grid cells for visualization
        for (int col = 0; col < columnCount; col++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Vector3 center = CalculateWorldPosition(col, row) + alignmentOffset;
                Vector3 size = new Vector3(cellSize.x, 0.1f, cellSize.y);
                Gizmos.DrawWireCube(center, size);
            }
        }

        // Draw center indicator for alignment reference
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, 0.2f);

        // Draw axis indicators
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, Vector3.right * 2f); // A-axis (X)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward * 2f); // X-axis (Z)
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, Vector3.up * 1f); // Y-axis (Height)
    }
}