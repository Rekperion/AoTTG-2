using Assets.Scripts;
using UnityEngine;

public class RacingCheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.layer == 8)
        {
            gameObject = gameObject.transform.root.gameObject;
            if (gameObject.GetPhotonView() != null && gameObject.GetPhotonView().isMine && gameObject.GetComponent<Hero>() != null)
            {
                FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#00ff00>Checkpoint set.</color>");
                gameObject.GetComponent<Hero>().fillGas();
                FengGameManagerMKII.instance.racingSpawnPoint = base.gameObject.transform.position;
                FengGameManagerMKII.instance.racingSpawnPointSet = true;
            }
        }
    }
}

