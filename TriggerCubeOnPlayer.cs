using UnityEngine;

public class TriggerCubeOnPlayer : MonoBehaviour
{
    [Header("Reference to the Cube")]
    public CubeInteraction cubeToTrigger;

    [Header("Reference to the Player")]
    public Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            if (cubeToTrigger != null)
            {
                cubeToTrigger.TriggerDisappearance();
            }
        }
    }
}
