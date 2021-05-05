using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TankShoot : MonoBehaviour
{
    public enum WeaponType {Cannon, Fervent, Rail, Launcher};

    public bool reload;
    public bool empty;
    public Tank tank;
    public TankCamera tankCamera;
    public Tank minDistPlayer;
    public List<Tank> seensPlayers = new List<Tank>();
    public List<Vector2> poses = new List<Vector2>();


    private void Update()
    {
        if (tank.controll)
        {
            var s = tank.weapons[tank.weapon];

            var finded = FindObjectsOfType<Tank>().ToList();
            finded.RemoveAll(x=>x.GetComponent<TankDead>() != null);
            var players = finded.ToArray();
            seensPlayers.Clear();
            poses.Clear();
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != tank)
                {
                    if (SeeObject(players[i].corpuses[players[i].corpus].coprus.gameObject, GetComponent<TankCamera>().shootCam, players[i].transform.name))
                    {
                        if (Vector3.Distance(players[i].transform.position, tankCamera.shootCam.transform.position) <= s.maxDist)
                        {
                            //players[i].NoRenderAll();

                            var scpos = tankCamera.shootCam.WorldToScreenPoint(players[i].transform.position, Camera.MonoOrStereoscopicEye.Mono);
                            if (scpos.x < Screen.width / 2 + s.camRange && scpos.x > Screen.width / 2 - s.camRange && scpos.y > Screen.height / 2 - s.camRange && scpos.y < Screen.height / 2 + s.camRange)
                            {
                                poses.Add(scpos);
                                seensPlayers.Add(players[i]);
                            }
                        }
                    }
                }
            }
            float mindist = 9999;
            int id = -1;
            for (int i = 0; i < poses.Count; i++)
            {
                var dst = Vector2.Distance(poses[i], new Vector2(Screen.width / 2, Screen.height / 2));
                if (dst <= mindist)
                {
                    id = i;
                    mindist = dst;
                }
            }
            if (id != -1)
            {
                //seensPlayers[id].RenderAll();
            }
            if (Input.GetKey(KeyCode.Space) && reload == false && empty == false)
            {
                float normaldamage = tank.weapons[tank.weapon].damage;
                tank.weapons[tank.weapon].damage = (int)(tank.weapons[tank.weapon].damage * (tank.damageBonus ? 2 : 1));



                if (s.type == WeaponType.Cannon)
                {
                    if (s.value >= s.valueMin)
                    {
                        GetComponent<Rigidbody>().AddTorque(-s.weapon.transform.right * s.upForce * Time.deltaTime, ForceMode.Impulse);
                        GetComponent<Rigidbody>().AddRelativeForce(-s.weapon.transform.forward * s.backForce * Time.deltaTime, ForceMode.Impulse);
                        s.value -= s.valueSub;
                        if (s.value <= 0)
                        {
                            empty = true;
                        }

                        if (id != -1)
                        {
                            if (seensPlayers[id].health > 0)
                            {
                                seensPlayers[id].GetComponent<Player>().photonView.RPC("TakeDamage", Photon.Pun.RpcTarget.OthersBuffered, (int)tank.weapons[tank.weapon].damage, (string)transform.name);
                                PhotonNetwork.Instantiate("Partic1", seensPlayers[id].weapons[seensPlayers[id].weapon].weapon.transform.position, Quaternion.identity);
                                seensPlayers[id].GetComponent<Player>().dmgs.Add("-" + (int)s.damage);
                            }
                        }
                        else
                        {
                            RaycastHit hit;
                            Ray ray = tankCamera.shootCam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

                            if (Physics.Raycast(ray, out hit))
                            {
                                if (hit.collider != null)
                                {
                                    if (hit.transform.GetComponent<Tank>() != null && hit.transform.GetComponent<TankDead>() == null)
                                    {
                                        hit.transform.GetComponent<Player>().photonView.RPC("TakeDamage", Photon.Pun.RpcTarget.OthersBuffered, (int)tank.weapons[tank.weapon].damage, (string)transform.name);
                                        seensPlayers[id].GetComponent<Player>().dmgs.Add("-" + (int)s.damage);
                                    }
                                    PhotonNetwork.Instantiate("Partic1", hit.point, Quaternion.identity);
                                }
                            }
                        }
                        reload = true;
                    }   
                }
                if (s.type == WeaponType.Fervent)
                {
                    if (s.value >= s.valueMin)
                    {
                        s.bullet.SetActive(true);
                        s.bullet.GetComponentInChildren<ParticleSystem>().Play();
                        s.value -= s.valueSub * Time.deltaTime;
                        reload = true;
                    }
                    else
                    {
                        s.bullet.GetComponentInChildren<ParticleSystem>().Stop();
                        s.bullet.SetActive(false);
                    }
                    if (id != -1)
                    {
                        if (seensPlayers[id].health > 0)
                        {
                            seensPlayers[id].GetComponent<Player>().photonView.RPC("TakeDamage", Photon.Pun.RpcTarget.OthersBuffered, (int)tank.weapons[tank.weapon].damage, (string)transform.name);
                            seensPlayers[id].GetComponent<Player>().dmgs.Add("-" + (int)s.damage);
                        }
                    }
                    if (s.value <= s.valueMin)
                    {
                        s.bullet.GetComponentInChildren<ParticleSystem>().Stop();
                        empty = true;
                    }
                }
                if (s.type == WeaponType.Rail)
                {
                    if (s.value >= s.valueMin)
                    {
                        GetComponent<Rigidbody>().AddTorque(-s.weapon.transform.right * s.upForce * Time.deltaTime, ForceMode.Impulse);
                        GetComponent<Rigidbody>().AddRelativeForce(-s.weapon.transform.forward * s.backForce * Time.deltaTime, ForceMode.Impulse);
                        s.value -= s.valueSub;
                        if (s.value <= 0)
                        {
                            empty = true;
                        }

                        if (id != -1)
                        {
                            if (seensPlayers[id].health > 0)
                            {
                                seensPlayers[id].GetComponent<Player>().photonView.RPC("TakeDamage", Photon.Pun.RpcTarget.OthersBuffered, (int)tank.weapons[tank.weapon].damage, (string)transform.name);
                                tank.weapons[tank.weapon].shootPoints[0].LookAt(seensPlayers[id].transform.position);
                                GameObject g = PhotonNetwork.Instantiate("RailBullet", tank.weapons[tank.weapon].shootPoints[0].transform.position, tank.weapons[tank.weapon].shootPoints[0].rotation);
                                seensPlayers[id].GetComponent<Player>().dmgs.Add("-" + (int)s.damage);
                                g.transform.LookAt(seensPlayers[id].transform);
                            }
                        }
                        else
                        {
                            RaycastHit hit;
                            Ray ray = tankCamera.shootCam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

                            if (Physics.Raycast(ray, out hit))
                            {
                                if (hit.collider != null)
                                {
                                    if (hit.transform.GetComponent<Tank>() != null && hit.transform.GetComponent<TankDead>() == null)
                                    {
                                        if (seensPlayers.Count > 0)
                                        {
                                            seensPlayers[id].GetComponent<Player>().dmgs.Add("-" + (int)s.damage);
                                        }
                                        else
                                        {
                                            hit.transform.GetComponent<Player>().dmgs.Add("-" + (int)s.damage);
                                        }
                                        hit.transform.GetComponent<Player>().photonView.RPC("TakeDamage", Photon.Pun.RpcTarget.OthersBuffered, (int)tank.weapons[tank.weapon].damage, (string)transform.name);
                                    }
                                }
                            }
                           PhotonNetwork.Instantiate("RailBullet", tank.weapons[tank.weapon].shootPoints[0].transform.position, tank.weapons[tank.weapon].shootPoints[0].rotation);
                            

                        }
                        reload = true;
                    }
                }
                if (s.type == WeaponType.Launcher)
                {
                    if (s.value >= s.valueMin)
                    {
                        GetComponent<Rigidbody>().AddTorque(-s.weapon.transform.right * s.upForce * Time.deltaTime, ForceMode.Impulse);
                        GetComponent<Rigidbody>().AddRelativeForce(-s.weapon.transform.forward * s.backForce * Time.deltaTime, ForceMode.Impulse);
                        s.value -= s.valueSub;
                        if (s.value <= 0)
                        {
                            empty = true;
                        }

                        if (id != -1)
                        {
                            if (seensPlayers[id].health > 0)
                            {
                                seensPlayers[id].GetComponent<Player>().photonView.RPC("TakeDamage", Photon.Pun.RpcTarget.OthersBuffered, (int)tank.weapons[tank.weapon].damage, (string)transform.name);
                                tank.weapons[tank.weapon].shootPoints[0].LookAt(seensPlayers[id].transform.position);
                                GameObject g = PhotonNetwork.Instantiate("Rocket", tank.weapons[tank.weapon].shootPoints[0].transform.position, tank.weapons[tank.weapon].shootPoints[0].rotation);
                                g.GetComponent<Rocket>().player = transform.name;
                                g.GetComponent<Rocket>().target = seensPlayers[id].transform.name;
                                g.GetComponent<Rocket>().damage = (int)tank.weapons[tank.weapon].damage;
                                GameObject g1 = PhotonNetwork.Instantiate("Rocket", tank.weapons[tank.weapon].shootPoints[1].transform.position, tank.weapons[tank.weapon].shootPoints[0].rotation);
                                g1.GetComponent<Rocket>().player = transform.name;
                                g1.GetComponent<Rocket>().target = seensPlayers[id].transform.name;
                                g1.GetComponent<Rocket>().damage = (int)tank.weapons[tank.weapon].damage;
                                g.transform.LookAt(seensPlayers[id].transform);
                            }
                        }
                        else
                        {
                            GameObject g = PhotonNetwork.Instantiate("Rocket", tank.weapons[tank.weapon].shootPoints[0].transform.position, tank.weapons[tank.weapon].shootPoints[0].rotation);
                            g.GetComponent<Rocket>().player = transform.name;
                            g.GetComponent<Rocket>().damage = (int)tank.weapons[tank.weapon].damage;
                            GameObject g1 = PhotonNetwork.Instantiate("Rocket", tank.weapons[tank.weapon].shootPoints[1].transform.position, tank.weapons[tank.weapon].shootPoints[1].rotation);
                            g1.GetComponent<Rocket>().player = transform.name;
                            g1.GetComponent<Rocket>().damage = (int)tank.weapons[tank.weapon].damage;

                        }
                        reload = true;
                    }
                }
                if (tank.weapons[tank.weapon].animManager != null)
                {
                    tank.GetComponent<Player>().photonView.RPC("ShootAnim", Photon.Pun.RpcTarget.AllBuffered, (string)transform.name);

                    if (tank.weapon == 0)
                    {
                        (tank.weapons[tank.weapon].animManager as CannonAnim).Shoot();
                        
                    }
                    if (tank.weapon == 2)
                    {
                        (tank.weapons[tank.weapon].animManager as RailAnim).Shoot();
                    }
                    if (tank.weapon == 3)
                    {
                        (tank.weapons[tank.weapon].animManager as LauncherAnim).Shoot();
                    }
                }

                tank.weapons[tank.weapon].damage = normaldamage;
            }
            else
            {
                if (s.type == WeaponType.Fervent)
                {
                    reload = true;
                    s.bullet.GetComponentInChildren<ParticleSystem>().Stop();
                }
            }
            if (s.value < 100)
            {
                s.value += s.valueAdd * Time.deltaTime * (tank.reloadBonus ? 5 : 1);
            }
            if (reload == true)
            {
                if (s.value >= 100)
                {
                    empty = false;
                }
                if (s.value >= s.valueMin)
                {
                    reload = false;
                }
            }

        }
        else
        {
            var s = tank.weapons[tank.weapon];
            if(s.type == WeaponType.Fervent)
            {
                s.bullet.active = true;
                if (tank.isShoot)
                {
                    s.bullet.GetComponentInChildren<ParticleSystem>().Play();
                }
                else
                {
                    s.bullet.GetComponentInChildren<ParticleSystem>().Stop();
                }
            }

        }
    }

    public bool SeeObject(GameObject Object, Camera cam, string name)
    {
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, Object.transform.position - cam.transform.position);
        if (Physics.Raycast(cam.transform.position, Object.transform.position - cam.transform.position, out hit))
        {
            if (hit.collider != null)
            {
                if (name != hit.transform.name)
                {  return false;
                }
                else {

                    return true;
                }
            }
            else
            {
                print("NULL");
                return false;
            }
        }
        else
        {
            print("FALSE");
            return false;
        }
    }

}
