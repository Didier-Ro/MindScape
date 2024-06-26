using System;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateZone : MonoBehaviour
{
   public bool canActivate;
   [SerializeField] private GameObject gameObjectToActivate;
   private MovableObject _movableObject;
   private string currentControlScheme;
   public GameObject player;
   [SerializeField] private GameObject[] gameUI;
   [SerializeField] private bool isDeactivateWithCondition;
   [SerializeField] private int conditionId;
   private Animator animator;
   
  
   public void DeactivateCanvas()
   {
      if (gameUI[0] != null && gameUI[1] != null)
      {
         gameUI[0].SetActive(false);
         gameUI[1].SetActive(false);
      }
   }
   
   public void SetActiveCanvas()
   {
      if (InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Gamepad")
      {
         if (gameUI[0] != null && gameUI[1] != null)
         {
            gameUI[0].SetActive(true);
            gameUI[1].SetActive(false);
         }
      }
      else if(InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Keyboard")
      {
         if (gameUI[0] != null && gameUI[1] != null)
         {
            gameUI[0].SetActive(false);
            gameUI[1].SetActive(true);
         }
      }
   }

   private void FixedUpdate()
   {
      if(!InputManager.GetInstance().HoldingInteract() && !_movableObject.isMoving && PlayerStates.GetInstance().GetCurrentPlayerState() == PLAYER_STATES.MOVINGBOXES)
      {
            DeactivateBoxProcess();
      } 
      
   }

   public void ActivateBoxProcess()
   {
      PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.MOVINGBOXES);
      _movableObject.GetDirection(player);
      //gameUI[0].SetActive(false);
      //gameUI[1].SetActive(false);
      //gameUI[2].SetActive(true);
   }

   private void DeactivateBoxProcess()
   {
      //gameUI[0].SetActive(false);
      //gameUI[1].SetActive(true);
      //gameUI[2].SetActive(false);
      animator.SetBool("IsHoldingUD", false);
      animator.SetBool("IsHoldingRL", false);
      PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
   }

   private void Start()
   {
      animator = PlayerStates.GetInstance().buttonsAnimator;
      if (GameManager.GetInstance().IsConditionCompleted(conditionId) && isDeactivateWithCondition)
      {
         Destroy(gameObject);
      }
      else
      {
         _movableObject = gameObjectToActivate.GetComponent<MovableObject>();
      }

      for (int i = 0; i < PlayerStates.GetInstance().uiObjects.Length; i++)
      {
         gameUI[i] = PlayerStates.GetInstance().uiObjects[i];
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         SetActiveCanvas();
         canActivate = true;
         if (player == null)
         {
            player = other.gameObject;
         }
      }
       
   }

   private void OnEnable()
   {
      Collider2D collider2D = GetComponent<Collider2D>();
      collider2D.enabled = true;
   }

   private void OnDisable()
   {
      canActivate = false;
      DeactivateCanvas();
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         DeactivateCanvas();
         canActivate = false;
      }
   }

   public enum ObjectState
   {
      IDLE,
      MOVING,
   }
}
