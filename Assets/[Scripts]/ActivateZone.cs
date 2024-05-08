using System;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateZone : MonoBehaviour
{
   [SerializeField] private GameObject gameObjectToActivate;
   private MovableObject _movableObject;
   private string currentControlScheme;
   public GameObject player;
   [SerializeField] private GameObject[] gameUI;
   [SerializeField] private bool isDeactivateWithCondition;
   [SerializeField] private int conditionId;
   public void DeactivateCanvas()
   {
      gameUI[0].SetActive(false);
      gameUI[1].SetActive(false);
      gameUI[2].SetActive(false);
   }
   
   public void SetActiveCanvas()
   {
      if (InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Gamepad")
      {
         gameUI[0].SetActive(false);
         gameUI[1].SetActive(true);
      }
      else if(InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Keyboard")
      {
         gameUI[0].SetActive(true);
         gameUI[1].SetActive(false);
      }
   }

   private void FixedUpdate()
   {
      if(!InputManager.GetInstance().HoldingInteract() && !_movableObject.isMoving && PlayerStates.GetInstance().GetCurrentPlayerState() == PLAYER_STATES.MOVINGBOXES)
      {
         Debug.Log("se desactiva");
         DeactivateBoxProcess();
      }
   }

   public void ActivateBoxProcess()
   {
      PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.MOVINGBOXES);
      _movableObject.GetDirection(player);
      gameUI[0].SetActive(false);
      gameUI[1].SetActive(false);
      gameUI[2].SetActive(true);
   }

   public void DeactivateBoxProcess()
   {
      gameUI[0].SetActive(false);
      gameUI[1].SetActive(true);
      gameUI[2].SetActive(false);
      PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
   }

   private void Start()
   {
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
         if (player == null)
         {
            player = other.gameObject;
         }
         SetActiveCanvas();
      }
       
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         DeactivateCanvas();
      }
   }

   
}
