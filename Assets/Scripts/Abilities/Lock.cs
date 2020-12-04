using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Ability
{
    Dictionary<Transform, Vector3> oldPositions = new Dictionary<Transform, Vector3>();
    bool warped = false;
    Vector3 oldStandingPlace = Vector3.zero;
    Transform oldStandingObject;

    [SerializeField]
    Material platformBase;
    [SerializeField]
    Material platformLock;
    [SerializeField]
    Material damageFieldBase;
    [SerializeField]
    Material damageFieldLock;
    [SerializeField]
    float scale = 1;
    [SerializeField]
    GameObject path;
    [SerializeField]
    PlayerController player;
    [SerializeField]
    Transform warper;

    public override void Use(Transform origin, Transform target)
    {
        PlayerController player = origin.GetComponent<PlayerController>();
        if (player == null) return;
        Debug.Log("Beep Boop Bop - Bastion");

        if (!warped)
        {
            warped = true;
            path.SetActive(true);
            player.scrollCam = true;
            oldPositions.Clear();

            foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (obj.tag == "ignore" || obj.GetComponent<Camera>()) continue;
                
                oldPositions.Add(obj.transform, obj.transform.position);
                obj.transform.position = Vector3.ProjectOnPlane(obj.transform.position, Camera.main.transform.forward);// - Vector3.Project(player.transform.position, Camera.main.transform.forward);
                warper.rotation = Camera.main.transform.rotation;
                obj.transform.parent = warper;
                obj.layer = 9;
                if (obj.GetComponent<DamageVolume>()) obj.GetComponent<Renderer>().sharedMaterial = damageFieldLock;
                else obj.GetComponent<Renderer>().sharedMaterial = platformLock;
            }
            
            warper.transform.localScale = new Vector3(1, 1, scale);
            player.down = -Camera.main.transform.up;
        }
        else
        {
            warped = false;
            path.SetActive(false);
            player.scrollCam = false;

            warper.transform.localScale = Vector3.one;

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(new Ray(player.transform.position - player.down * .5f, player.down), out hit))
            {
                player.controller.enabled = false;
                player.transform.position += oldPositions[hit.transform] - hit.transform.position;
                player.controller.enabled = true;
            }

            foreach (KeyValuePair<Transform, Vector3> obj in oldPositions)
            {
                obj.Key.parent = null;
                obj.Key.position = obj.Value;
                obj.Key.gameObject.layer = 0;
                if (obj.Key.GetComponent<DamageVolume>()) obj.Key.GetComponent<Renderer>().sharedMaterial = damageFieldBase;
                else obj.Key.GetComponent<Renderer>().sharedMaterial = platformBase;
            }
            player.down = Vector3.down;
        }
        
    }
}
