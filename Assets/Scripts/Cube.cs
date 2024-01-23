using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Windows.Speech;

public class Cube : MonoBehaviourPunCallbacks,IPunObservable
{

    public UnityEngine.UI.Text myText;

    public string[] yeniDizi = { "abc", "asd", "aaa", "bbb", "ccc", "ddd", "eee" };
    public int health = 100;
    public UnityEngine.UI.Text healthText;
    public int playerID;
    public GameObject spell;
    public GameObject shield;
    private Room room;
    public float hiz = 10.0f;
    public GameObject spellInstantie;
    public GameObject shieldInstantie;
    private float destroyTimer = 0f;
    private float destroyDelay = 2f;
    private float destroyTimer2 = 0f;
    private float destroyDelay2 = 2f;

    // Start is called before the first frame update
    void Start()
        
    {
        room = GameObject.Find("Photon").GetComponent<Room>();
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
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+ (float)(0.5));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (float)(0.5));
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && spellInstantie == null)
            {
                // Yeni bir spellInstantie oluþtur ve konumunu ayarla
                spellInstantie = PhotonNetwork.Instantiate(spell.name, room.Cube.transform.position, Quaternion.identity);
                spellInstantie.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                destroyTimer = 0f;
            }

            // Eðer spellInstantie varsa, ilerlet ve 2 saniye sonra yok etme
            if (spellInstantie != null)
            {
                spellInstantie.transform.Translate(Vector3.forward * hiz * Time.deltaTime); 
                destroyTimer += Time.deltaTime;

                // Eðer belirlenen süre geçtiyse, yok et
                if (destroyTimer >= destroyDelay)
                {
                    PhotonNetwork.Destroy(spellInstantie);
                    spellInstantie = null;
                }
                //Destroy(spellInstantie, 2f);
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
        if (other.gameObject.tag == "spell")
        {
            PhotonNetwork.Destroy(spellInstantie);
            spellInstantie = null;
            if (photonView.IsMine)
            {
                photonView.RPC("TakeDamage", RpcTarget.All, 10);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
         health-= damage;
        if (health<=0)
        {
            health = 0;
        }
    }
    private void FixedUpdate()
    {
        healthText.text = health.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else if (stream.IsReading)
        {
            health = (int)stream.ReceiveNext();
        }
    }
}
