using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Windows.Speech;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class Cube : MonoBehaviourPunCallbacks, IPunObservable
{

    public UnityEngine.UI.Text myText;

    public string[] yeniDizi = { "abc", "asd", "aaa", "bbb", "ccc", "ddd", "eee" };
    public int health = 100;
    public UnityEngine.UI.Text healthText;
    public int playerID;
    public GameObject spell;
    public GameObject spell2;
    public GameObject shield;
    private Room room;
    public float hiz = 10.0f;
    public GameObject spellInstantieA;
    public GameObject spellInstantieB;
    public GameObject shieldInstantie;
    private float destroyTimer = 0f;
    private float destroyDelay = 2f;
    private float destroyTimer2 = 0f;
    private float destroyDelay2 = 2f;
    public List<int> teamA=new List<int>();
    public List<int> teamB = new List<int>();
    public UnityEngine.UI.Text TeamAHealthText;
    public UnityEngine.UI.Text TeamBHealthText;
    public int teamAHealthTotal=0;
    public int teamBHealthTotal=0;
    public string teamType;

    // Start is called before the first frame update
    void Start()

    {
        TeamAHealthText = GameObject.Find("TeamA").GetComponent<UnityEngine.UI.Text>();
        TeamBHealthText = GameObject.Find("TeamB").GetComponent<UnityEngine.UI.Text>();
        room = GameObject.Find("Photon").GetComponent<Room>();
            if (PhotonNetwork.PlayerList.Length==1)
            {
                photonView.RPC("AddToTeamAListRPC", RpcTarget.AllBuffered, health);
                teamType = "A";
            }
            if (PhotonNetwork.PlayerList.Length == 2)
            {
                photonView.RPC("AddToTeamAListRPC", RpcTarget.AllBuffered, health);
                teamType = "A";
            }
            else if(PhotonNetwork.PlayerList.Length == 3)
            {
            if (PhotonNetwork.PlayerList[2]==PhotonNetwork.LocalPlayer)
                {
                photonView.RPC("AddToTeamBListRPC", RpcTarget.AllBuffered, health);
                teamType = "B";
                }
            }
    }
    void Update()
    {

        TeamAHealthText = GameObject.Find("TeamA").GetComponent<UnityEngine.UI.Text>();
        TeamBHealthText = GameObject.Find("TeamB").GetComponent<UnityEngine.UI.Text>();

        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int rnd = Random.Range(0, 7);
                ChangeText(yeniDizi[rnd]);
                photonView.RPC("SyncTextChange", RpcTarget.Others, yeniDizi[rnd]);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.position = new Vector3(transform.position.x - (float)(0.5), transform.position.y, transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position = new Vector3(transform.position.x + (float)(0.5), transform.position.y, transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (float)(0.5));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (float)(0.5));
            }
            if (teamType == "A")
            {

                if (Input.GetKeyDown(KeyCode.Mouse0) && spellInstantieA == null)
                {
                    spellInstantieA = PhotonNetwork.Instantiate(spell.name, room.Cube.transform.position, Quaternion.identity);
                    spellInstantieA.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                    destroyTimer = 0f;
                }

                if (spellInstantieA != null)
                {
                    spellInstantieA.transform.Translate(Vector3.forward * hiz * Time.deltaTime);
                    destroyTimer += Time.deltaTime;
                    if (destroyTimer >= destroyDelay)
                    {
                        PhotonNetwork.Destroy(spellInstantieA);
                        spellInstantieA = null;
                    }
                }
            }
            if (teamType == "B")
            {

                if (Input.GetKeyDown(KeyCode.Mouse0) && spellInstantieB == null)
                {
                    spellInstantieB = PhotonNetwork.Instantiate(spell2.name, room.Cube.transform.position, Quaternion.identity);
                    spellInstantieB.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                    destroyTimer = 0f;
                }

                if (spellInstantieB != null)
                {
                    spellInstantieB.transform.Translate(Vector3.forward * hiz * Time.deltaTime);
                    destroyTimer += Time.deltaTime;
                    if (destroyTimer >= destroyDelay)
                    {
                        PhotonNetwork.Destroy(spellInstantieB);
                        spellInstantieB = null;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                shieldInstantie = PhotonNetwork.Instantiate(shield.name, room.Cube.transform.position, Quaternion.identity);
                shieldInstantie.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                destroyTimer2 = 0f;

            }
            if (shieldInstantie != null)
            {

                destroyTimer2 += Time.deltaTime;
                if (destroyTimer2 >= destroyDelay2)
                {
                    PhotonNetwork.Destroy(shieldInstantie);
                    shieldInstantie = null;
                }
            }
        }
    }
    [PunRPC]
    void AddToTeamAListRPC(int health)
    {
        teamA.Add(health);
        teamAHealthTotal += health;
        //TeamAHealthText.text = teamAHealthTotal.ToString();
    }
    [PunRPC]
    void AddToTeamBListRPC(int health)
    {
        teamB.Add(health);
        teamBHealthTotal = health;
        //TeamBHealthText.text = teamBHealthTotal.ToString();
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "spell2" && teamType == "A")
        {
            PhotonNetwork.Destroy(spellInstantieA);
            spellInstantieA = null;
            if (photonView.IsMine)
            {
                photonView.RPC("TakeDamage", RpcTarget.All, 10);
            }
        }
        if (other.gameObject.tag == "spell"&& teamType == "B")
        {
            PhotonNetwork.Destroy(spellInstantieB);
            spellInstantieB = null;
            if (photonView.IsMine)
            {
                photonView.RPC("TakeDamage2", RpcTarget.All, 10);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
        }
        teamAHealthTotal -= damage;
        if (teamAHealthTotal <= 0)
        {
           teamAHealthTotal = 0;
        }
        photonView.RPC("totalA", RpcTarget.AllBuffered, teamAHealthTotal);
        TeamAHealthText.text = teamAHealthTotal.ToString();
    }
    [PunRPC]
    public void TakeDamage2(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
        }
        teamBHealthTotal -= damage;
        if (teamBHealthTotal <= 0)
        {
            teamBHealthTotal = 0;
        }
        photonView.RPC("totalB", RpcTarget.AllBuffered, teamBHealthTotal);
        TeamBHealthText.text = teamBHealthTotal.ToString();
    }
    [PunRPC]
    void totalA(int teamATotal)
    {
        teamAHealthTotal = teamATotal;
        TeamAHealthText.text = teamAHealthTotal.ToString();
    }
    [PunRPC]
    void totalB(int teamBTotal)
    {
        teamBHealthTotal = teamBTotal;
        TeamBHealthText.text = teamBHealthTotal.ToString();
    }
    private void FixedUpdate()
    {
        healthText.text = health.ToString();
        //Debug.Log("teamA"+teamA.Count); 
        if (TeamAHealthText.text!="TeamAHealtMaximum")
        {
            teamAHealthTotal = int.Parse(TeamAHealthText.text);
            if (int.Parse(TeamAHealthText.text)==0)
            {
                room.gameover.SetActive(true);
            }

        }
        if (TeamBHealthText.text != "TeamBHealtMaximum")
        {
            teamBHealthTotal = int.Parse(TeamBHealthText.text); 
            if (int.Parse(TeamBHealthText.text) == 0)
            {
                room.gameover.SetActive(true);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(teamType);
            stream.SendNext(teamAHealthTotal);
            stream.SendNext(teamBHealthTotal);
            //Debug.Log("gitii");
        }
        else if (stream.IsReading)
        {
            health = (int)stream.ReceiveNext();
            teamType= (string)stream.ReceiveNext();
            teamAHealthTotal = (int)stream.ReceiveNext();
            teamBHealthTotal = (int)stream.ReceiveNext();
            //Debug.Log("geldi");

        }
    }
}