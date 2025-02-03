using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;


public class TargetReticle : MonoBehaviour
{
    // public Camera cam;
    public GameObject target;
    public float sensitivity;
    public bool locked;

    // public Vector3 camForward;
    // public RaycastHit planeCheck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnLook(InputAction.CallbackContext context) {
        if (locked) {
            return;
        }
        var lookAmount = context.ReadValue<Vector2>();
        transform.position -= new Vector3(lookAmount.x * sensitivity, 0, lookAmount.y * sensitivity) * Time.deltaTime;
    }

    public void OnClick(InputAction.CallbackContext context) {
        if (locked) {
            locked = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    // this code works with the reticle to raycast onto the ground.
    // I'm going to instead have the look 2d vector move the target on the x z plane
    // void Start()
    // {
    //     cam = Camera.main;
    //     camForward = cam.transform.forward;
    //     target.transform.position = cam.transform.position;
    //     target.transform.forward = camForward;
    // }

    // // Update is called once per frame
    // void FixedUpdate()
    // {
    //     Ray ray = new Ray(target.transform.position, target.transform.forward);
    //     Physics.Raycast(ray, out planeCheck, 100000f);
    //     if (planeCheck.distance == 0) {
    //         Debug.Log("not touching anything");
    //     }
    //     else {
    //         transform.position = planeCheck.point;
    //         transform.up = planeCheck.normal;
    //     }

    //     Vector3 veccy = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
    //     Debug.Log("veccy: "+veccy);
    //     Vector3 sexyveccy = cam.ScreenToWorldPoint(veccy);
    //     Debug.Log("veccy is sexy: "+sexyveccy);
    //     target.transform.position = sexyveccy;

    //     Debug.DrawRay(target.transform.position, target.transform.forward * 100000f, Color.red);
    //     Debug.DrawRay(cam.transform.position, cam.transform.forward * 10000f, Color.blue);
    // }
}
