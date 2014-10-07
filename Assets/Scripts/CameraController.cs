using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public GameObject player;

    void Update() {
        Vector3 newCamPos = player.transform.position;
        newCamPos.z = camera.transform.position.z;
        camera.transform.position = newCamPos;
    }
}
