using Cinemachine;
using UnityEngine;

public class CMVCTargetAssign : MonoBehaviour
{
    GameObject player;

    void OnEnable()
    {
        GameManager.PlayerSpawnedEvent += VCamLookAtFollow;
    }

    void OnDisable()
    {
        GameManager.PlayerSpawnedEvent -= VCamLookAtFollow;
    }

    void VCamLookAtFollow()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("CMVCTargetAssign player object null");
        }

//        vcam.LookAt = player.transform;
        vcam.Follow = player.transform;
    }
}
