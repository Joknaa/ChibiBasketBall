using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class InputController : MonoBehaviour {

        private bool aimPressed;
        private bool throwPressed;
        
        public Text outputText;
        
        public GameObject player;

        private Vector2 startTouchPosition;
        private Vector2 currentPosition;
        private Vector2 endTouchPosition;
        private Vector3 movementDirection = Vector3.zero;

        private bool stopTouch = false;

        public float speed = 10;

        public float swipeRange;
        public float tapRange;

        private bool enableSlowSwipe = false;
        private bool movedRight;
        private bool movedLeft;
        private bool movedUp;
        private bool movedDown;
        private Transform playerTransform;
        private PlayerController playerController;


        private void Start() {
            playerTransform = player.transform;
            playerController = player.GetComponent<PlayerController>();
        }

        void Update() {
            if (!enableSlowSwipe) {
                fastSwipe();
            }
        }

        public void fastSwipe() {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                startTouchPosition = Input.GetTouch(0).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
                currentPosition = Input.GetTouch(0).position;
                Vector2 Distance = currentPosition - startTouchPosition;
                
                    if (Distance.x < -swipeRange && !movedLeft) {
                        outputText.text = "Left";
                        movementDirection += new Vector3(-1, 0, 0);
                        startTouchPosition = Input.GetTouch(0).position;
                        stopTouch = true;
                        movedLeft = true;
                        movedRight = movedUp = movedDown = false;
                    }
                    else if (Distance.x > swipeRange && !movedRight) {
                        outputText.text = "Right";
                        movementDirection += new Vector3(1, 0, 0);
                        startTouchPosition = Input.GetTouch(0).position;
                        stopTouch = true;
                        movedRight = true;
                        movedLeft = movedUp = movedDown = false;
                    } 
                    else if (Distance.y > swipeRange && !movedUp) {
                        outputText.text = "Up";
                        movementDirection += new Vector3(0, 0, 1);
                        startTouchPosition = Input.GetTouch(0).position;
                        stopTouch = true;
                        movedUp = true;
                        movedRight = movedLeft = movedDown = false;
                    }
                    else if (Distance.y < -swipeRange && !movedDown) {
                        outputText.text = "Down";
                        movementDirection += new Vector3(0, 0, -1);
                        startTouchPosition = Input.GetTouch(0).position;
                        stopTouch = true;
                        movedDown = true;
                        movedRight = movedUp = movedLeft = false;
                    }

                    movementDirection = movementDirection.normalized;
                    //playerTransform.Translate(movementDirection.normalized * speed * Time.deltaTime);
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary) {
                //playerTransform.position += movementDirection * speed * Time.deltaTime;
            }
            
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
                movementDirection = Vector3.zero;
                stopTouch = false;
                endTouchPosition = Input.GetTouch(0).position;
                Vector2 Distance = endTouchPosition - startTouchPosition;

                if (Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange) {
                    outputText.text = "Tap";
                    // if i am not aiming and not throwing, then aim
                    // if i am aiming and not throwing, then throw
                    // if i am not aiming and throwing, then stop throwing
                    
                    if (PlayerState.Instance.IsAiming()) {
                        aimPressed = false;
                        throwPressed = true;
                    }
                    else if (PlayerState.Instance.IsThrowing()) {
                        throwPressed = false;
                    }
                    else {
                        aimPressed = true;
                    }
                }
            }
        }
        

        public bool IsAimPressed() => aimPressed;

        public bool IsThrowPressed() => throwPressed;

        public float GetHorizontalMove() => movementDirection.x;
        public float GetVerticalMove() => movementDirection.z;
        
    }
}