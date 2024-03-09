using UnityEngine;

public class Enemy : MonoBehaviour,Ikillable
{
    private bool isSuscribed = true;
    private bool CanMove = true;

    #region ChasingVariables
     [SerializeField] private float satisfactionRadius;
     [SerializeField] private float timeToTarget = 0.25f;
     [SerializeField] private float proximateError = 0.5f;
    #endregion
    [SerializeField] private float _maxSpeed; 
    [SerializeField] private Transform _target;
    [SerializeField] private Rigidbody2D _rb; 

    #region AttackingVariables
    [SerializeField] private Rigidbody2D _targetRb; 
    [SerializeField] private Vector2 _seekTarget; 
    private float prediction;
    [SerializeField] private float maxPrediction;
    #endregion
    
   
    private SpriteRenderer _spriteRenderer;
    private float _secondsToDie = 3;
    private float _framesHit = 0f;
    
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
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
      
      public void AssignTarget(GameObject target)
      {
          _target = target.transform;
          _targetRb = target.GetComponent<Rigidbody2D>();
      }
  
      private void KinematicSeek(Vector2 targetTransform) //Moves the enemy to the Vector 2 you assigned
      {
          //Seek   
          Vector2 result = targetTransform - (Vector2) transform.position;
          /* Flee
           Vector3 result =  transform.position -  _target.position;
           */
          result.Normalize();
          result *= _maxSpeed;
         // transform.rotation = Quaternion.LookRotation(result);
          _rb.velocity = result;
      }
  
      public virtual void GetSteering() //Gets the Player Direction and Makes a Prediction
      {
          Vector2 direction =  _target.position - transform.position;
          float distance = direction.magnitude;
          float speed = _rb.velocity.magnitude;
          if (speed <= distance / maxPrediction)
          {
              prediction = maxPrediction;
          }
          else
          {
              prediction = distance / speed;
          }
          _seekTarget = _target.position;
          var targetSpeed = _targetRb.velocity;
          _seekTarget += targetSpeed * prediction;
          KinematicSeek(_seekTarget);
      }
      void FixedUpdate()
      {
          if (!CanMove) return;
          if (!GameManager.GetInstance().ReturnFlashing())
          {
              Chasing();
          }
          else
          {
              GetSteering();
          }
          
      }

      private void Chasing() //Chasing or lurking method
      {
          Vector2 result = _target.position - transform.position; //Calculates the distance between player
          if (result.magnitude > satisfactionRadius) //Check if the distance is bigger than radiusLimit
          {
              result /= timeToTarget; // decrease the speed in relation to the time is target
              if (result.magnitude > _maxSpeed)
              {
                  result.Normalize();
                  result *= _maxSpeed;
              }
              _rb.velocity = result;
          }
          else if (result.magnitude < satisfactionRadius - proximateError) 
          {
              result = transform.position - _target.position;
              result *= _maxSpeed;
              _rb.velocity = result;
          }
          else //stop the movement
          {
              _rb.velocity = Vector2.zero;
          }
      }
  
      private void OnDrawGizmos()
      {
          Vector2 point = GameManager.GetInstance().ReturnFlashing() ? _seekTarget :  _target.position;
          DrawGizmosLine(point);
      }
  
      private void DrawGizmosLine(Vector2 draw)
      {
          Gizmos.color = Color.cyan;
          float radiusGizmos = GameManager.GetInstance().ReturnFlashing() ? 0.3f : satisfactionRadius;
          Gizmos.DrawSphere(draw, radiusGizmos);  
      }

      private void ChangeOpacity(float newOpacity)
      {
          Color color = _spriteRenderer.color;
          color.a = newOpacity;
          _spriteRenderer.color = color;
      }

      public void Hit()
      {
          if (CanMove)
          {
              if (_framesHit >= _secondsToDie * 60)
              {
                  gameObject.SetActive(false);
                  ChangeOpacity(1);
                  _framesHit = 0;
              }
              else
              {
                  _framesHit++;
                  _rb.velocity /=  1.1f;
                  float opacitySprite = _framesHit * 100 / (_secondsToDie * 60)/100;
                  ChangeOpacity(1.0f - opacitySprite); 
              }
          }
         
      }
}
