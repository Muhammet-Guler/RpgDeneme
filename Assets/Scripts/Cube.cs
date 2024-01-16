using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cube : MonoBehaviourPunCallbacks
{

    public UnityEngine.UI.Text myText;

    public string[] yeniDizi = { "abc", "asd", "aaa", "bbb", "ccc", "ddd", "eee" };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int rnd = Random.Range(0, 7);
                ChangeText(yeniDizi[rnd]);
                photonView.RPC("SyncTextChange", RpcTarget.Others, yeniDizi[rnd]);
            }
        }
    }

    [PunRPC]
    private void SyncTextChange(string newText)
    {
        ChangeText(newText);
    }

    private void ChangeText(string newText)
    {
        myText.text = newText;
    }
}
