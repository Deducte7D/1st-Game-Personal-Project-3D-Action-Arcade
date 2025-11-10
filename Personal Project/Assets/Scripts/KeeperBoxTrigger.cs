using UnityEngine;

public class KeeperBoxTrigger : MonoBehaviour
{

    public KeeperController keeperScript;

    //bool isActiveGT = false;

    private void Update()
    {
        // update status bool of activeGT
        //bool isActiveGT = keeperScript.isActiveGT;
    }

    private void OnTriggerEnter(Collider other)
    {
        // react when contacted w/ ball
        if (other.CompareTag("Ball"))
        {
            keeperScript.InitiateCatched();
        }

        //// react when touching ground after dive (GT here means Ground Touch)
        //if(other.CompareTag("Ground") && isActiveGT == true)
        //{
        //    keeperScript.InitiateGroundLay();

        //}
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
