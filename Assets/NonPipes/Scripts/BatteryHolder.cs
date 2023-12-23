using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHolder : MonoBehaviour
{
    public Signs[] signs = new Signs[0];
    private Battery battery = null;
    public Material litMaterial;
    public Transform socketLocation;
    public MeshRenderer mesh;
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
        if (other.gameObject.GetComponent<Battery>() != null)
        {
            battery = other.gameObject.GetComponent<Battery>();
            battery.gameObject.tag = "Untagged";
            battery.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            battery.gameObject.transform.position = socketLocation.position;
            battery.gameObject.transform.rotation = Quaternion.identity;

            mesh.material = litMaterial;
            battery.mesh2.SetMaterials(new List<Material> { battery.mesh2.material, litMaterial });
            battery.mesh1.SetMaterials(new List<Material> { battery.mesh1.material, litMaterial });
            TurnOnSigns();
        }
    }

    public void TurnOnSigns()
    {
        foreach (Signs s in signs)
        {
            s.PowerOn();
        }
    }
}
