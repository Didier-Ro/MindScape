using UnityEngine;

public class BoxFalling : MonoBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private BoxCollider2D boxColliderParent;
    [SerializeField] private BoxCollider2D boxColliderChild;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 finalPoint;
    [SerializeField] private bool canMove = false;

    [SerializeField] bool isFalling;

    private float size;
    private float totalSize;

    void Start()
    {
        size = 1 - 0;
        totalSize = size / (60 * 1);
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            Falling();
        }

        if (canMove)
        {
            MoveBox();
        }
    }

    public void BoxInZone()
    {
        isFalling = true;
    }

    void Falling()
    {
        boxColliderParent.enabled = false;
        boxColliderChild.enabled = false;
        box.transform.localScale -= new Vector3(totalSize, totalSize,0);

        if (box.transform.localScale.y <= 0 || box.transform.localScale.x <= 0)
        {
            isFalling = false;
            box.transform.localScale = new Vector3(0,0,0);
            RespawnBox();
        }
    }

    void RespawnBox()
    {
        transform.position = finalPoint;
        box.transform.position = spawnPoint;
        box.transform.localScale = new Vector3(1, 1, 1);
        canMove = true;
    }

    void MoveBox()
    {
        float distance = spawnPoint.y - finalPoint.y;
        float destiny = distance / (60 * 1f);

        box.transform.position -= new Vector3(0, destiny, 0);

        if (box.transform.position.y <= finalPoint.y)
        {
            box.transform.position = finalPoint;
            canMove = false;
            boxColliderParent.enabled = true;
            boxColliderChild.enabled = true;
        }
    }
}