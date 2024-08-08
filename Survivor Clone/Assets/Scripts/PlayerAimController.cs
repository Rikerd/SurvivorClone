using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    // Current position of the gun
    private Transform gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FaceMouse();
    }

    // Takes mouse position and points gun towards that area
    private void FaceMouse()
    {
        // Position of the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        // Box in which the mouse is not tracked to prevent gun glitching out

        //if (gunPoint.position.x - 0.8f >= mousePos.x || gunPoint.position.x + 0.8f <= mousePos.x ||
        //    gunPoint.position.y - 0.8f >= mousePos.y || gunPoint.position.y + 0.8f <= mousePos.y)
        //{
        //    // Rotates gun to look at where the mouse is
        //    gun.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - gunPoint.position);

        //    // Flips gun and player in accordance to where it is pointing to
        //    if (gun.rotation.eulerAngles.z > 180)
        //    {
        //        gunPoint.localPosition = new Vector3(intialGunPointX, gunPoint.localPosition.y);
        //    }
        //    else if (gun.rotation.eulerAngles.z < 180)
        //    {
        //        gunPoint.localPosition = new Vector3(-intialGunPointX, gunPoint.localPosition.y);
        //    }

        //}
    }
}
