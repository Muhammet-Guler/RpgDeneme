using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shield : MonoBehaviourPunCallbacks
{
    public Cube cube;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="spell")
        {
            PhotonNetwork.Destroy(other.gameObject);
            cube.spellInstantieA = null;
            
        }
        if (other.tag == "spell2")
        {
            PhotonNetwork.Destroy(other.gameObject);
            cube.spellInstantieB = null;

        }
    }
}
