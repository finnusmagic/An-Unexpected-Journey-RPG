using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraController : MonoBehaviour
    {

        public float PlayerCameraDistance;
        GameObject player;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }


        void LateUpdate()
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + PlayerCameraDistance, player.transform.position.z - PlayerCameraDistance +1);
        }
    }
}
