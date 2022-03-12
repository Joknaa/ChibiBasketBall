using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;
    public float MoveSpeed = 5;
    [Header("Throw")] public float ThrowDuration = 0.6f;
    public float ArcHeight = 5;

    // variables
    private Rigidbody ballRigidBody;
    private float flightTime;
    private InputController InputController;

    private void Start() {
        ballRigidBody = Ball.GetComponent<Rigidbody>();
        InputController = GetComponent<InputController>();
    }

    private void Update() {
        if (!PlayerState.Instance.HasBall()) return;

        if (Input.GetKeyUp(KeyCode.Space)) {
        //if (InputController.IsThrowPressed()) {
            PlayerState.Instance.SetState_Throwing();
            flightTime = 0;
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        //if (InputController.IsAimPressed())
            PlayerState.Instance.SetState_Aiming();
        else
            PlayerState.Instance.SetState_Dribbling();
    }

    private void FixedUpdate() {
        Move();

        switch (PlayerState.Instance.GetCurrentState()) {
            case PlayerStates.Dribbling:
                Dribble();
                break;
            case PlayerStates.Aiming:
                Aim();
                break;
            case PlayerStates.Throwing:
                Throw();
                break;
        }
    }

    private void Move() {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //Vector3 direction = new Vector3(InputController.GetHorizontalMove(), 0, InputController.GetVerticalMove());
        transform.position += direction * MoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);
    }
    private void Dribble() {
        Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 0.5f);
        Arms.localEulerAngles = Vector3.right * 0;
    }
    private void Aim() {
        Ball.position = PosOverHead.position;
        Arms.localEulerAngles = Vector3.right * 180;
        transform.LookAt(Target.parent.position);
    }
    private void Throw() {
        Arms.localEulerAngles = Vector3.right * 0;

        flightTime += Time.deltaTime;
        float t01 = flightTime / ThrowDuration;

        Vector3 pos = Vector3.Lerp(PosOverHead.position, Target.position, t01);
        Vector3 arc = Vector3.up * ArcHeight * Mathf.Sin(t01 * Mathf.PI);
        Ball.position = pos + arc;

        if (t01 < 1) return;

        PlayerState.Instance.SetState_NoBall();
        ballRigidBody.isKinematic = false;
    }


    private void OnTriggerEnter(Collider other) {
        if (PlayerState.Instance.HasBall()) return;
        PlayerState.Instance.SetState_Dribbling();
        ballRigidBody.isKinematic = true;
    }
}