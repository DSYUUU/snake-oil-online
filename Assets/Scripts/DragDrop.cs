using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    private bool isDragging = false;
    private bool isOverDrop = false;
    private bool isDraggable = true;
    private Vector2 startPos;
    private GameObject canvas;
    private GameObject dropZone;
    private GameObject startPar;
    private int maxDropSize = 2;
    private int currDropSize;
   
    public PlayerManager PlayerManager;

    void Start()
    {   
        canvas = GameObject.Find("Canvas");
        dropZone = GameObject.Find("DropZone");
        
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        

        if(!hasAuthority)
        {
            isDraggable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging) 
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(canvas.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDrop = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDrop = false;
        dropZone = null;
    }

    public void StartDrag() 
    {
        if(!isDraggable)
        {
            return;
        }

        startPar = transform.parent.gameObject;
        startPos = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        if(!isDraggable)
        {
            return;
        }

        isDragging = false;

        if (isOverDrop && (PlayerManager.currDropSize < maxDropSize)) 
        {
            PlayerManager.currHandSize -= 1;
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;
            PlayerManager.PlayCard(gameObject);
            PlayerManager.currDropSize += 1;
        }
        else 
        {
            transform.position = startPos;
            transform.SetParent(startPar.transform, false);
        }
    }
}
