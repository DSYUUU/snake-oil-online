using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;
    public int currHandSize;
    public int currDropSize;

    private string path = "Assets/Resources/cards.txt";

    private int maxHandSize = 6;

    private List<string> cardNames = new List<string>();

    // Start is called before the first frame update
    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        DropZone = GameObject.Find("DropZone");
        currHandSize = 0;
    }

    [Server]
    public override void OnStartServer()
    {
        StreamReader sr = new StreamReader(path);
        string line;
        while((line = sr.ReadLine()) != null)
        {
            cardNames.Add(line);
        }
        sr.Close();
    }

    [Command]
    public void CmdDealCards(int hand)
    {
        for(int i = hand; i < maxHandSize; i++) 
        {
            GameObject card = Instantiate(Card1, new Vector2(0, 0), Quaternion.identity);
            string cardName = cardNames[Random.Range(0, cardNames.Count)];
            cardNames.Remove(cardName);
            //card.GetComponent<CardProperties>().cardText = cardName;
            //card.transform.GetChild(0).gameObject.GetComponent<Text>().text = cardName;
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
            RpcShowCardText(card, cardName);
            hand += 1;
        }
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
    }

    [ClientRpc]
    void RpcShowCardText(GameObject card, string cardName)
    {
        card.GetComponent<CardProperties>().cardText = cardName;
        card.transform.GetChild(0).gameObject.GetComponent<Text>().text = cardName;
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if(hasAuthority) 
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                //card.transform.SetParent(EnemyArea.transform, false);
            }
        }
        else if (type == "Played")
        {
            card.transform.SetParent(DropZone.transform, false);
        }
    }
}
