using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    public float MoveSpeed = 5;
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    // variables
    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0;

    // Update is called once per frame
    void Update() {
        Move();

        if (IsBallInHands) {
            if (Input.GetKey(KeyCode.Space)) {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;

                // look towards the target
                transform.LookAt(Target.parent.position);
            } else {
                // dribbling
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 0.5f);
                Arms.localEulerAngles = Vector3.right * 0;
            }

            // throw ball
            if (Input.GetKeyUp(KeyCode.Space)) {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        // ball in the air
        if (IsBallFlying) {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            // move to target
            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // move in arc
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            // moment when ball arrives at the target
            if (t01 >= 1) {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void Move() {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * MoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);
    }

    private void OnTriggerEnter(Collider other) {
        if (!IsBallInHands && !IsBallFlying) {
            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}