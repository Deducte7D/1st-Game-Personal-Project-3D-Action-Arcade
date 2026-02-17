using UnityEngine;

public class KeeperBoxTrigger : MonoBehaviour
{

    public KeeperController keeperScript;
    public SpawnManagerV2 spawnManagerV2;

    //bool isActiveGT = false;

    private void Update()
    {
        // update status bool of activeGT
        //bool isActiveGT = keeperScript.isActiveGT;

        if (spawnManagerV2 == null)
        {
            spawnManagerV2 = FindFirstObjectByType<SpawnManagerV2>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // react when contacted w/ ball
        if (other.CompareTag("Ball"))
        {
            keeperScript.InitiateCatched();
            Debug.Log("Keeper got some moves");
            spawnManagerV2.NopeGoal();
        }

        //// react when touching ground after dive (GT here means Ground Touch)
        //if(other.CompareTag("Ground") && isActiveGT == true)
        //{
        //    keeperScript.InitiateGroundLay();

        //}
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
