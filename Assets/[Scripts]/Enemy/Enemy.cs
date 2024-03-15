using UnityEngine;

public class Enemy : MonoBehaviour,Ikillable
{
    private bool isSuscribed = true;
    private bool CanMove = true;

    #region ChasingVariables
     [SerializeField] private float satisfactionRadius = default;
     [SerializeField] private float timeToTarget = default;
     [SerializeField] private float proximateError = default;
    #endregion
    [SerializeField] private float maxSpeed = default; 
    [SerializeField] private Transform targetTransform = default;
    [SerializeField] private Rigidbody2D rb = default;
    private Flashlight _flashlightTarget;

    #region AttackingVariables
    private float prediction;
    [SerializeField] private Rigidbody2D targetRb; 
    [SerializeField] private Vector2 seekTarget; 
    [SerializeField] private float maxPrediction;
    #endregion
    
    private float framesHit = 0f;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float secondsToDie = 3;
    
    #region SubscriptionToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and makes differents behaviour
    {
        CanMove = _newGameState == GAME_STATE.EXPLORATION;
    }
        
    #endregion
        void Start()
        { 
            SubscribeToGameManagerGameState();
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        void FixedUpdate()
        {
            if (!CanMove) return;
            if (!GameManager.GetInstance().GetFlashing() && _flashlightTarget.currentSliderValue > 0)
            {
                Chasing();
            }
            else
            {
                GetSteering();
            }
          
        }
        
      private void OnDisable()
      {
          if (isSuscribed)
          {
              GameManager.GetInstance().OnGameStateChange -= OnGameStateChange;
              isSuscribed = false;
          }
      }

      private void OnEnable()
      {
          if (!isSuscribed)
          {
              GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
              OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
              
              isSuscribed = true;
          }
      }
      
      public void AssignTarget(GameObject _target)
      {
          targetTransform = _target.transform;
          targetRb = _target.GetComponent<Rigidbody2D>();
          _flashlightTarget = _target.GetComponent<Flashlight>();
      }
      
      public void Hit()
      {
          if (CanMove)
          {
              if (framesHit >= secondsToDie * 60)
              {
                  gameObject.SetActive(false);
                  if (EnemySpawner.getInstance() != null)
                  {
                      EnemySpawner.getInstance().enemiesActive.Remove(gameObject);
                      EnemySpawner.getInstance().CheckArray();
                  }
                  ChangeOpacity(1);
                  framesHit = 0;
              }
              else
              {
                  framesHit++;
                  rb.velocity /= 2;
                  float opacitySprite = framesHit * 100 / (secondsToDie * 60)/100;
                  ChangeOpacity(1.0f - opacitySprite); 
              }
          }
         
      }
      
      public virtual void GetSteering() //Gets the Player Direction and Makes a Prediction
      {
          Vector2 direction =  targetTransform.position - transform.position;
          float distance = direction.magnitude;
          float speed = rb.velocity.magnitude;
          if (speed <= distance / maxPrediction)
          {
              prediction = maxPrediction;
          }
          else
          {
              prediction = distance / speed;
          }
          seekTarget = targetTransform.position;
          var targetSpeed = targetRb.velocity;
          seekTarget += targetSpeed * prediction;
          KinematicSeek(seekTarget);
      }
      private void KinematicSeek(Vector2 _targetTransform) //Moves the enemy to the Vector 2 you assigned
      {
          //Seek   
          Vector2 result = _targetTransform - (Vector2) transform.position;
          /* Flee
           Vector3 result =  transform.position -  _target.position;
           */
          result.Normalize();
          result *= maxSpeed;
          // transform.rotation = Quaternion.LookRotation(result);
          rb.velocity = result;
      }

      private void Chasing() //Chasing or lurking method
      {
          Vector2 result = targetTransform.position - transform.position; //Calculates the distance between player
          if (result.magnitude > satisfactionRadius) //Check if the distance is bigger than radiusLimit
          {
              result /= timeToTarget; // decrease the speed in relation to the time is target
              if (result.magnitude > maxSpeed)
              {
                  result.Normalize();
                  result *= maxSpeed;
              } 
              rb.velocity = result;
          }
          else if (result.magnitude < satisfactionRadius - proximateError) 
          {
              result = transform.position - targetTransform.position;
              result *= maxSpeed;
              rb.velocity = result;
          }
          else //stop the movement
          {
              rb.velocity = Vector2.zero;
          }
      }
  
      private void OnDrawGizmos()
      {
          DrawGizmosLine(seekTarget);
      }
  
      private void DrawGizmosLine(Vector2 draw)
      {
          Gizmos.color = Color.cyan;
          Gizmos.DrawSphere(draw, maxPrediction);  
      }

      private void ChangeOpacity(float _newOpacity)
      {
          Color color = spriteRenderer.color;
          color.a = _newOpacity;
          spriteRenderer.color = color;
      }
}
