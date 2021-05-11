using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : MonoBehaviour
{
    public GameObject camera;
    public Camera shootCam;
    public float speed, angleSpeed, rotSpeed;
    public Tank tank;
    public Vector3 offcet;
    public float yLook;
    public Transform maxYPoint;


    public bool wall, oldWall;
    private void Start()
    {
        Application.targetFrameRate = 60;
        shootCam.targetDisplay = 1;
    }
    private void Update()
    {
        camera.transform.LookAt(transform.position + new Vector3(0, yLook, 0));
        if (!tank.controll)
        {
            shootCam.GetComponent<PlayerCameraHandle>().player = this.gameObject;
            shootCam.transform.gameObject.SetActive(false);
            camera.SetActive(false);
        }
        else
        {
            camera.transform.parent = null;
        }

        shootCam.transform.parent = tank.weapons[tank.weapon].weapon.transform;
    }

    void FixedUpdate(){
        Update_Other();
    }
    private void Update_Other()
    {
        shootCam.transform.position = tank.corpuses[tank.corpus].weaponPoint.position + tank.corpuses[tank.corpus].weaponPointOffcet;
        //handle.transform.parent = tank.weapons[tank.weapon].weapon.transform;

        Vector3 dir = (tank.weapons[tank.weapon].weapon.transform.position + tank.weapons[tank.weapon].weapon.transform.TransformVector(offcet)) - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, Vector3.Distance(tank.weapons[tank.weapon].weapon.transform.position + tank.weapons[tank.weapon].weapon.transform.TransformVector(offcet), transform.position)))
        {
            if (hit.collider != null)
            { 
                wall = true;
                print("true");
            }
            else
            {
                wall = false;
                print("false");
            }
        }
        else
        {
            wall = false;
        }
        
        if (oldWall != wall)
        {
            if (wall == true)
            {

            }
        }

        if (wall)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, hit.point, speed * 2);
        }
        else
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, tank.weapons[tank.weapon].weapon.transform.position + tank.weapons[tank.weapon].weapon.transform.TransformVector(offcet), speed);

        }



        oldWall = wall;

        //handle.transform.localEulerAngles = new Vector3(Mathf.Clamp(handle.transform.localEulerAngles.x + Input.GetAxisRaw("UpDown") * Time.deltaTime * angleSpeed, 90f, 180f), handle.transform.localEulerAngles.y, handle.transform.localEulerAngles.z);
        //handle.transform.localEulerAngles = new Vector3((handle.transform.localEulerAngles.x, maxCam, minCam), handle.transform.localEulerAngles.y, handle.transform.localEulerAngles.z);
    }

}
