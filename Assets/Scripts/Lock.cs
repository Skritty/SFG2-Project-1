using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Ability
{
    Dictionary<Transform, Vector3> oldPositions = new Dictionary<Transform, Vector3>();
    bool warped = false;
    Transform oldStandingPlace = null;

    [SerializeField]
    Material platform1;
    [SerializeField]
    Material platform2;
    [SerializeField]
    PlayerController player;

    public override void Use(Transform origin, Transform target)
    {
        PlayerController player = origin.GetComponent<PlayerController>();
        if (player == null) return;
        Debug.Log("Beep Boop Bop - Bastion");

        if (!warped)
        {
            warped = true;
            oldPositions.Clear();
            //player.transform.rotation = Quaternion.LookRotation(player.transform.forward, Camera.main.transform.up);
            //player.down = -Camera.main.transform.up;
            player.scrollCam = true;
            ScreenMesh(player.transform.position);
        }
        else
        {
            warped = false;
            RaycastHit hit = new RaycastHit();
            if (Physics.SphereCast(new Ray(player.transform.position + player.transform.up * .5f, -player.transform.up), .2f, out hit))
            {
                player.transform.position = oldPositions[hit.transform] + (hit.point - hit.transform.position);
                Camera.main.transform.position = player.transform.position + player.cameraOffset;
                Debug.Log("Hit");
            }
            //player.transform.rotation = Quaternion.LookRotation(player.transform.forward, Vector3.up);
            //player.down = -Vector3.up;
            player.scrollCam = false;
            foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (obj.tag != "ignore" && !obj.GetComponent<PlayerController>() && oldPositions.ContainsKey(obj.transform) && Vector3.Project(obj.transform.position - Camera.main.transform.position, Camera.main.transform.forward).normalized == Camera.main.transform.forward)
                {
                    obj.transform.position = oldPositions[obj.transform];
                    if (obj.GetComponent<Renderer>())
                    {
                        obj.GetComponent<Renderer>().material = platform1;
                    }
                }
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (player == null || !warped) return;
        
        RaycastHit hit = new RaycastHit();
        if (Physics.SphereCast(new Ray(player.transform.position + player.transform.up*.5f, -player.transform.up), .2f, out hit))
        {
            if (hit.collider.GetComponent<Renderer>())
            {
                hit.collider.GetComponent<Renderer>().material = platform2;
                if (oldStandingPlace != null && oldStandingPlace != hit.collider.transform)
                {
                    oldStandingPlace.GetComponent<Renderer>().material = platform1;
                }
                oldStandingPlace = hit.collider.transform;
            }
        }
    }

    private void ScreenMesh(Vector3 center)
    {
        foreach(GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if(obj.tag != "ignore" && !obj.GetComponent<Camera>() && Vector3.Project(obj.transform.position - Camera.main.transform.position, Camera.main.transform.forward).normalized == Camera.main.transform.forward)
            {
                oldPositions.Add(obj.transform, obj.transform.position);
                obj.transform.position = obj.transform.position - Vector3.Project(obj.transform.position, Camera.main.transform.forward) + center;
            }
        }
    }
}
