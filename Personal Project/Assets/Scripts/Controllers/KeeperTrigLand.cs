using UnityEngine;

public class KeeperTrigLand : MonoBehaviour
{
    public KeeperController keeperScript;

    public bool isActiveGT;
    

    private void Update()
    {
        // update status bool of activeGT
        isActiveGT = keeperScript.isActiveGT;
    }

    private void OnTriggerEnter(Collider other)
    {
        // react when touching ground after dive (GT here means Ground Touch)
        if (other.CompareTag("Ground") && isActiveGT == true)
        {
            keeperScript.InitiateGroundLay();
            //Debug.Log("Debug Reached");
        }
    }
}
