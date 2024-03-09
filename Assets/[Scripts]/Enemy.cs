using UnityEngine;

public class Enemy : MonoBehaviour,Ikillable
{
    private bool isSuscribed = true;
    private bool CanMove = true;
    [SerializeField] private float _maxSpeed; 
    [SerializeField] private Transform _target; 
    [SerializeField] private Rigidbody2D _targetRb; 
    [SerializeField] private Vector2 _seekTarget; 
    [SerializeField] private Rigidbody2D _rb; 
    private float prediction;
    private SpriteRenderer _spriteRenderer;
    private float _secondsToDie = 3;
    private float _framesHit = 0f;
    [SerializeField] private float maxPrediction;
    
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
       /*   if (!CanMove) return;
          GetSteering();*/
      }
  
      private void OnDrawGizmos() 
      {
          DrawGizmosLine(_seekTarget);
      }
  
      private void DrawGizmosLine(Vector2 draw)
      {
          Gizmos.color = Color.cyan;
          Gizmos.DrawSphere(draw, 0.3f);
      }

      private void ChangeOpacity(float newOpacity)
      {
          Color color = _spriteRenderer.color;
          color.a = newOpacity;
          _spriteRenderer.color = color;
      }

      public void Hit()
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
              float opacitySprite = _framesHit * 100 / (_secondsToDie * 60)/100;
              ChangeOpacity(1.0f - opacitySprite); 
          }
      }
}
