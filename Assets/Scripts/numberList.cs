using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class numberList : MonoBehaviourPunCallbacks
{

    public List<int> NumberList = new List<int>();
    public UnityEngine.UI.Text listText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void UpdateList(int newNumber)
    {
        NumberList.Add(newNumber);
        UpdateUI();
    }

    void UpdateUI()
    {
        string listString = "List: ";
        foreach (int number in NumberList)
        {
            listString += number + " ";
        }
        listText.text = listString;
    }

    public void OnListButtonClick()
    {
        int randomNumber = Random.Range(1, 100);
        photonView.RPC("UpdateList", RpcTarget.AllBuffered, randomNumber);
    }
}
