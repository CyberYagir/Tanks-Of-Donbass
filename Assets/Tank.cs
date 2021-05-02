using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public int weapon, corpus;
    public float health;
    public float team;
    public int k, d;
    public List<WeaponData> weapons;
    public List<CorpusData> corpuses;


    public Rigidbody m_Rigidbody;

    public bool controll = true;

    public LayerMask layer;

    public List<bool> yes;

    public bool isShoot;

    float anglemx = 0;

    [Header("Bonuses")]

    public bool damageBonus;
    public bool healthBonus;
    public bool speedBonus;
    public bool reloadBonus;

    public IEnumerator dmgBonus;
    public IEnumerator healBonus;
    public IEnumerator spBonus;
    public IEnumerator relBonus;

    IEnumerator damageBonusWait()
    {
        damageBonus = true;
        yield return new WaitForSeconds(15);
        damageBonus = false;
        dmgBonus = null;
    }
    IEnumerator healthBonusWait()
    {
        healthBonus = true;
        yield return new WaitForSeconds(2);
        healthBonus = false;
        healBonus = null;
    }
    IEnumerator speedBonusWait()
    {
        speedBonus = true;
        yield return new WaitForSeconds(20);
        speedBonus = false;
        spBonus = null;
    }
    IEnumerator realoadBonusWait()
    {
        reloadBonus = true;
        yield return new WaitForSeconds(5);
        reloadBonus = false;
        relBonus = null;
    }

    public void AddDamageBonus()
    {
        if (dmgBonus != null)
        {
            StopCoroutine(dmgBonus);
        }
        dmgBonus = damageBonusWait();
        StartCoroutine(dmgBonus);
    }
    public void AddHealthBonus()
    {
        if (healBonus != null)
        {
            StopCoroutine(healBonus);
        }
        healBonus = healthBonusWait();
        StartCoroutine(healBonus);
    }
    public void AddSpeedBonus()
    {
        if (spBonus != null)
        {
            StopCoroutine(spBonus);
        }
        spBonus = speedBonusWait();
        StartCoroutine(spBonus);
    }
    public void AddRelBonus()
    {
        if (relBonus != null)
        {
            StopCoroutine(relBonus);
        }
        relBonus = realoadBonusWait();
        StartCoroutine(relBonus);
    }

    private void Start()
    {
        Set();
        if (controll)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].weapon.layer = 8;
            }
            for (int i = 0; i < corpuses.Count; i++)
            {
                corpuses[i].coprus.layer = 8;
                SetLayerRecursively(corpuses[i].coprus, 8);
            }
        }
        else
        {
            Set(true); //Оно всё работает
        }
        
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj.transform.tag != "Decor")
        {
            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
    private void LateUpdate()
    {
        Set(true);
        if (controll)
        {
            isShoot = Input.GetKey(KeyCode.Space);
            weapons[weapon].weapon.transform.Rotate(Vector3.forward * Input.GetAxisRaw("Turret") * weapons[weapon].rotSpeed * Time.deltaTime);
            corpuses[corpus].leftG.material.mainTextureOffset -= new Vector2(0, (corpuses[corpus].speed + m_Rigidbody.angularVelocity.y) * Time.deltaTime) * ((Input.GetAxis("Horizontal") > 0 ? 1 : 0) + Input.GetAxis("Vertical"));
            corpuses[corpus].rightG.material.mainTextureOffset -= new Vector2(0, (corpuses[corpus].speed + m_Rigidbody.angularVelocity.y) * Time.deltaTime) * ((Input.GetAxis("Horizontal") < 0 ? 1 : 0) + Input.GetAxis("Vertical"));

            yes = new List<bool>();

            float anglemx = 0;
            for (int i = 0; i < corpuses[corpus].pointsGrounds.Count; i++)
            {
                RaycastHit raycast;
                Debug.DrawRay(corpuses[corpus].pointsGrounds[i].position, -transform.up);
                if (Physics.Raycast(corpuses[corpus].pointsGrounds[i].position, -transform.up, out raycast, 1f, layer))
                {
                    if (raycast.collider != null)
                    {
                        if (Vector3.Angle(raycast.normal, Vector3.down) > anglemx)
                        {
                            anglemx = Vector3.Angle(raycast.normal, Vector3.down);
                        }
                        yes.Add(true);
                    }
                    else
                    {
                        yes.Add(false);
                    }
                }
            }

            if (yes.Contains(true))
            {
                m_Rigidbody.AddRelativeForce(Vector3.forward * corpuses[corpus].speed * Time.deltaTime * Input.GetAxisRaw("Vertical") * (speedBonus ? 1.5f : 1) * (((180 - anglemx)/100)+1));
                //Move();
                Turn();
            }
        }
    }

    public void RenderAll()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].weapon.GetComponent<Renderer>().material.color = Color.red;
            weapons[i].weapon.layer = 9;
        }
        for (int i = 0; i < corpuses.Count; i++)
        {
            weapons[i].weapon.GetComponent<Renderer>().material.color = Color.red;
            corpuses[i].coprus.layer = 9;
            SetLayerRecursively(corpuses[i].coprus, 9);
        }
    }
    public void NoRenderAll()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].weapon.GetComponent<Renderer>().material.color = Color.white;
            weapons[i].weapon.layer = 0;
        }
        for (int i = 0; i < corpuses.Count; i++)
        {
            weapons[i].weapon.GetComponent<Renderer>().material.color = Color.white;
            corpuses[i].coprus.layer = 0;
            SetLayerRecursively(corpuses[i].coprus, 0);
        }
    }

    private void Move()
    {
        Vector3 movement = transform.forward * Input.GetAxis("Vertical") * corpuses[corpus].speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        
    }
    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = Input.GetAxis("Horizontal") * corpuses[corpus].rotSpeed * Time.deltaTime;

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            turn = Input.GetAxis("Horizontal") * corpuses[corpus].rotSpeed * Time.deltaTime; 
        }

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    public void Set()
    {
        //GetComponent<TankCamera>().handle.transform.parent = transform;
        //GetComponent<TankCamera>().handle.transform.localEulerAngles = Vector3.zero;
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == weapon)
            {
                weapons[i].weapon.SetActive(true);
            }
            else
            {
                weapons[i].weapon.SetActive(false);
            }
        }
        for (int i = 0; i < corpuses.Count; i++)
        {
            if (i == corpus)
            {
                corpuses[i].coprus.SetActive(true);
                weapons[weapon].weapon.transform.position = corpuses[i].weaponPoint.position;
            }
            else
            {

                corpuses[i].coprus.SetActive(false);
            }
        }
        health = corpuses[corpus].maxHealth;
    }
    public void Set(bool noHp)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == weapon)
            {
                weapons[i].weapon.SetActive(true);
            }
            else
            {
                weapons[i].weapon.SetActive(false);
            }
        }
        for (int i = 0; i < corpuses.Count; i++)
        {
            if (i == corpus)
            {
                corpuses[i].coprus.SetActive(true);
                weapons[weapon].weapon.transform.position = corpuses[i].weaponPoint.position;
            }
            else
            {
                corpuses[i].coprus.SetActive(false);
            }
        }
    }

}
[System.Serializable]
public class CorpusData
{
    public string name;
    public GameObject coprus;
    public Transform weaponPoint;
    public Vector3 weaponPointOffcet;
    public List<Transform> pointsGrounds;
    public float speed;
    public float rotSpeed;
    public float maxHealth;
    public Renderer leftG, rightG;
    

}
[System.Serializable]
public class WeaponData
{
    public string name;
    public TankShoot.WeaponType type;
    public GameObject weapon;
    public float rotSpeed;
    public float damage;
    public float value;
    public int camRange;
    public float valueSub;
    public float valueAdd;
    public float valueMin;
    public float upForce;
    public float backForce;
    public float maxDist;
    public List<Transform> shootPoints;
    public GameObject bullet;
    public MonoBehaviour animManager;

}