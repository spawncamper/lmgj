using Cinemachine;
using UnityEngine;

public class CMVCTargetAssign : MonoBehaviour
{
    GameObject player;

    private void Awake()
    {

    }

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

        if (vcam == null)
        {
            Debug.LogError("[CMVCTargetAssign] vcam is null");
        }

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("CMVCTargetAssign player object null");
        }

//        vcam.LookAt = player.transform;
        vcam.Follow = player.transform;

        print("vcam on player");
    }
}
