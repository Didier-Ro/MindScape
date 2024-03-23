using System;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.SimpleLocalization.Scripts
{
    public class Boxes : MonoBehaviour, Istepable
    {
        /* private bool isMoving = false;
        private Vector3 startPos = default;
        int finalFramesToReachPoint = default;
        const float cubeBorder = 0.05f;
        public float distanceToMove = 1;
        RaycastHit2D rayhit;
        float startposition = default;
        float finalDistanceToMove = default;
        public float timeToReachPointInSeconds = 1;
        int framesToReachPoint = default;
        float speedPerFrame = default;
        float finalSpeedPerFrame = default;
        int frameCounter = 0;
        public Transform[] firepoints;
        direction push;


        private void FixedUpdate()
        {
            MoveBox();
        }

        private void MoveBox()
        {
            if (!isMoving) return;
            frameCounter++;
            switch (push)
            {
                case direction.left:
                    if (frameCounter >= finalFramesToReachPoint)
                    {
                        transform.Translate(new Vector3(-speedPerFrame, 0, 0));
                        isMoving = false;
                        transform.position = new Vector3(transform.position.x - finalDistanceToMove, transform.position.y, 0);
                    }
                    break;
                case direction.right:
                    if (frameCounter >= finalFramesToReachPoint)
                    {
                        transform.Translate(new Vector3(speedPerFrame, 0, 0));
                        isMoving = false;
                        transform.position = new Vector3(transform.position.x + finalDistanceToMove, transform.position.y, 0);
                    }
                    break;
                case direction.up:
                    if (frameCounter >= finalFramesToReachPoint)
                    {
                        transform.Translate(new Vector3(0, speedPerFrame, 0));
                        isMoving = false;
                        transform.position = new Vector3(transform.position.x, transform.position.y + finalDistanceToMove, 0);
                    }
                    break;
                case direction.down:
                    if (frameCounter >= finalFramesToReachPoint)
                    {
                        transform.Translate(new Vector3(0, -speedPerFrame, 0));
                        isMoving = false;
                        transform.position = new Vector3(transform.position.x, transform.position.y - finalDistanceToMove, 0);
                    }
                    break;
            }

            if (frameCounter >= finalFramesToReachPoint)
            {
                isMoving = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, startposition + finalDistanceToMove);
            }
        }

        public void Activate(Vector2 _playerDirection)
        {
            isMoving = true;
            framesToReachPoint = (int)timeToReachPointInSeconds * 60;
            speedPerFrame = distanceToMove / (timeToReachPointInSeconds * 60);
            Vector2 distance = _playerDirection - (Vector2)transform.position;
            Vector2 dir = Vector2.zero;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                startPos = transform.position;
                if (distance.x > 0)
                {
                    push = direction.left;
                    Debug.DrawRay(transform.position + (Vector3.left * cubeBorder), Vector3.left * distanceToMove, Color.red, 1f);
                    rayhit = Physics2D.Raycast(transform.position + (Vector3.left * cubeBorder), Vector3.left, distanceToMove);
                    if (rayhit.collider != null)
                    {
                        Debug.Log("Raycast Hit" + rayhit.point);
                        finalDistanceToMove = rayhit.point.x - (startposition + cubeBorder);
                        finalFramesToReachPoint = (int)(finalDistanceToMove / speedPerFrame);

                    }
                    else
                    {
                        finalDistanceToMove = distanceToMove;
                        finalFramesToReachPoint = framesToReachPoint;

                    }
                    frameCounter = 0;
                }
                else
                {
                    push = direction.right;
                    Debug.Log("aa");
                    Debug.DrawRay(transform.position + (Vector3.right * cubeBorder), Vector3.right * distanceToMove, Color.red, 1f);
                    rayhit = Physics2D.Raycast(transform.position + (Vector3.left * cubeBorder), Vector3.right, distanceToMove);
                    if (rayhit.collider != null)
                    {
                        Debug.Log("Raycast Hit" + rayhit.point);
                        finalDistanceToMove = rayhit.point.x - (startposition + cubeBorder);
                        finalFramesToReachPoint = (int)(finalDistanceToMove / speedPerFrame);
                    }
                    else
                    {
                        finalDistanceToMove = distanceToMove;
                        finalFramesToReachPoint = framesToReachPoint;
                    }
                    frameCounter = 0;
                }
            }
            else
            {
                startPos = transform.position;
                if (distance.y > 0)
                {
                    Debug.Log("aaa");
                    push = direction.down;
                    Debug.DrawRay(transform.position + (Vector3.down * cubeBorder), Vector3.down * distanceToMove, Color.red, 1f);
                    rayhit = Physics2D.Raycast(transform.position + (Vector3.left * cubeBorder), Vector3.down, distanceToMove);
                    if (rayhit.collider != null)
                    {
                        Debug.Log("Raycast Hit" + rayhit.point);
                        finalDistanceToMove = rayhit.point.y - (startposition + cubeBorder);
                        finalFramesToReachPoint = (int)(finalDistanceToMove / speedPerFrame);
                    }
                    else
                    {
                        finalDistanceToMove = distanceToMove;
                        finalFramesToReachPoint = framesToReachPoint;
                    }
                    frameCounter = 0;
                }
                else
                {
                    Debug.Log("aaaa");
                    push = direction.up;
                    Debug.DrawRay(transform.position + (Vector3.up * cubeBorder), Vector3.up * distanceToMove, Color.red, 1f);
                    rayhit = Physics2D.Raycast(transform.position + (Vector3.left * cubeBorder), Vector3.up, distanceToMove);
                    if (rayhit.collider != null)
                    {
                        Debug.Log("Raycast Hit" + rayhit.point);
                        finalDistanceToMove = rayhit.point.y - (startposition + cubeBorder);
                        finalFramesToReachPoint = (int)(finalDistanceToMove / speedPerFrame);
                    }
                    else
                    {
                        finalDistanceToMove = distanceToMove;
                        finalFramesToReachPoint = framesToReachPoint;
                    }
                    frameCounter = 0;
                }
            }
        }
        public enum direction
        {
            up, down, left, right,
        }

        */
        [SerializeField] private Dialog dialog;
        [SerializeField] private GameObject interactionCanvas = default;
        [SerializeField] private GameObject keyboardUI = default;
        [SerializeField] private GameObject gamepadUI = default;
        public String keyTittle;
        public List<String> localizationKey = new List<string>();
        [SerializeField] private LetterManager letterManager;
        private string currentControlScheme;

        private void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                SetActiveCanvas();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                DeactivateCanvas();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                SetActiveCanvas();
            }
        }

        private void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
            Debug.Log(letterManager.GetLetterList().Count);
        }

        public void Activate()
        {
            StartCoroutine(DialogManager.GetInstance().ShowDialog(dialog));
            GameManager.GetInstance().ChangeGameState(GAME_STATE.READING);
        }

        public void Deactivate()
        {
            letterManager.AddLetter(AddLetterToScriptableObject());
            GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
            Destroy(gameObject);
        }

        private LetterStructure AddLetterToScriptableObject()
        {
            LetterStructure letterStructure = new LetterStructure();
            letterStructure.letterTittle = keyTittle;
            letterStructure.letterBody = new List<string>();
            foreach (var body in localizationKey)
            {
                letterStructure.letterBody.Add(body);
            }
            return letterStructure;
        }

        private void Localize()
        {
            dialog.Lines.Clear();
            foreach (var key in localizationKey)
            {
                dialog.Lines.Add(LocalizationManager.Localize(key));
            }
        }

        public void SetActiveCanvas()
        {
            if (InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Gamepad")
            {
                interactionCanvas.SetActive(true);
                keyboardUI.SetActive(false);
                gamepadUI.SetActive(true);
            }
            else if (InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Keyboard")
            {
                interactionCanvas.SetActive(true);
                gamepadUI.SetActive(false);
                keyboardUI.SetActive(true);
            }
        }

        public void DeactivateCanvas()
        {
            interactionCanvas.SetActive(false);
        }
    }
}
