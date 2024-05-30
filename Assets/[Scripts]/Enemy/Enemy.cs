using System;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour, Ikillable
{
    private bool isSuscribed = true;
    private bool CanMove = true;
    private float recoverTime = 0;
    private bool isKnockBacking = false;
    Animator animator;

    #region ChasingVariables
    [SerializeField] private float satisfactionRadius = default;
    [SerializeField] private float timeToTarget = default;
    [SerializeField] private float proximateError = default;
    [SerializeField] private float rotationLurkingSpeed;
    [SerializeField] private bool isLurkingToTheRight;
    private int changeDirectionTimer;
    private int randomTimer;
    #endregion
    private float actualfleeSpeed;
    private float actualChasingSpeed;
    private float actualAttackingSpeed;
    [SerializeField] private float chasingSpeed = default;
    [SerializeField] private float fleeSpeed;
    [SerializeField] private float attackingSpeed;
    [SerializeField] private Transform targetTransform = default;
    [SerializeField] private Rigidbody2D rb = default;
    private Flashlight _flashlightTarget;

    #region AttackingVariables
    private float prediction;
    [SerializeField] private Rigidbody2D targetRb;
    [SerializeField] private Vector2 seekTarget;
    [SerializeField] private float maxPrediction;
    [SerializeField] private float knockBackForce;
    #endregion

    [SerializeField] private float recoverTimer = 1.0f;
    [SerializeField] private float healRate = 0.5f;
    [SerializeField] private float framesHit = 0f;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float secondsToDie = 3;

    private GameObject activeSmokeParticles;

    #region SubscriptionToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and makes differents behaviour
    {
        if (_newGameState == GAME_STATE.EXPLORATION)
        {
            CanMove = true;
            actualfleeSpeed = fleeSpeed;
            actualAttackingSpeed = attackingSpeed;
            actualChasingSpeed = chasingSpeed;
        }
        else if (_newGameState == GAME_STATE.TUTORIAL)
        {
            CanMove = true;
            rb.velocity /= 2;
            actualChasingSpeed /= 2f;
            actualAttackingSpeed /= 2f;
            actualfleeSpeed /= 2f;
        }
        else
        {
            CanMove = false;
            rb.velocity = Vector2.zero;
        }
    }

    #endregion
    void Start()
    {
        RandomTimer();
        recoverTime = recoverTimer;
        animator = GetComponent<Animator>();
        SubscribeToGameManagerGameState();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        actualfleeSpeed = fleeSpeed;
        actualAttackingSpeed = attackingSpeed;
        actualChasingSpeed = chasingSpeed;
    }

    void FixedUpdate()
    {
        if (!CanMove) return;

        HealObject();

        if (!GameManager.GetInstance().GetFlashing() && _flashlightTarget.currentSliderValue > 0)
        {
            animator.SetBool("Attack", false);
            Chasing();
        }
        else
        {
            animator.SetBool("Attack", true);
            GetSteering();
        }

        if (changeDirectionTimer < randomTimer)
        {
            changeDirectionTimer++;
        }
        else
        {
            changeDirectionTimer = 0;
            isLurkingToTheRight = !isLurkingToTheRight;
            RandomTimer();
        }

        if (rb.velocity.magnitude > 0.1f)
        {
            ActivateSmokeParticles();

        }
        if (rb.velocity.magnitude > 0.1f)
        {
            if (activeSmokeParticles != null)
            {
                activeSmokeParticles.transform.position = transform.position;
            }

            UpdateSmokeParticlesRotation();
        }
    }


    private void UpdateSmokeParticlesPosition()
    {
        if (activeSmokeParticles != null)
        {
            activeSmokeParticles.transform.position = transform.position;
        }
    }
    private void UpdateSmokeParticlesRotation()
    {
        Vector2 moveDirection = rb.velocity.normalized;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        if (activeSmokeParticles != null)
        {
            activeSmokeParticles.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }


    private void RandomTimer()
    {
        Random random = new Random();
        randomTimer = random.Next(30, 90);
    }

    private void KnockBackCheck()
    {
        if (rb.velocity.magnitude >= 0.1)
        {
            isKnockBacking = false;
        }
    }

    private void KnockBack(Vector2 _direction)
    {
        if (!isKnockBacking)
        {
            Debug.Log("hola");
            // isKnockBacking = true;
            rb.velocity = _direction.normalized * knockBackForce;
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

    private void HealObject() //Heals the enemy if is not taking damage 
    {
        if (framesHit <= 0)
            return;
        if (recoverTime <= 0)
        {
            framesHit -= healRate;
            float opacitySprite = framesHit * 100 / (secondsToDie * 60) / 100;
            ChangeOpacity(1.0f - opacitySprite);
        }
        else
        {
            recoverTime -= 1f / 60;
        }
    }

    public void Hit(Transform player)
    {

        if (CanMove)
        {
            recoverTime = recoverTimer;
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
                float opacitySprite = framesHit * 100 / (secondsToDie * 60) / 100;
                ChangeOpacity(1.0f - opacitySprite);
            }

        }
        ActivateSmokeParticles();
        Debug.Log("El enemigo ha sido golpeado");
        PlayDamageSound();

    }
    [SerializeField] private SoundLibrary soundLibrary;
    private void PlayDamageSound()
    {
        // Asegurarse de que el SoundLibrary est� asignado
        if (soundLibrary == null)
        {
            Debug.LogError("SoundLibrary is not assigned!");
            return;
        }

        // Obtener un sonido aleatorio de da�o de enemigo de la SoundLibrary
        AudioClip damageSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.GOLPE_ENEMIGO);

        // Reproducir el sonido si se encontr�
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
        else
        {
            Debug.LogWarning("Damage sound not found!");
        }
    }
    public void Die()
    {
        // Tu l�gica para manejar la muerte del enemigo...

        // Reproducir el sonido de muerte del enemigo
        PlayDeathSound();
    }
    private void PlayDeathSound()
    {
        // Asegurarse de que el SoundLibrary est� asignado
        if (soundLibrary == null)
        {
            Debug.LogError("SoundLibrary is not assigned!");
            return;
        }

        // Obtener un sonido aleatorio de muerte de enemigo de la SoundLibrary
        AudioClip deathSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.MUERTE_ENEMIGO);

        // Reproducir el sonido si se encontr�
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
        else
        {
            Debug.LogWarning("Death sound not found!");
        }
    }

    private void ActivateSmokeParticles()
    {
        // Desactivar part�culas activas si hay alguna
        if (activeSmokeParticles != null && activeSmokeParticles.activeInHierarchy)
        {
            // Si ya hay una part�cula activa, no necesitamos instanciar otra
            return;
        }

        // Obtener una instancia de la part�cula de humo enemigo variant desde el PoolManager
        activeSmokeParticles = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.HumoEnemigoVariant, transform.position, transform.rotation.eulerAngles);

        // Asegurarse de que la part�cula est� activada y posicionada correctamente
        if (activeSmokeParticles != null)
        {
            activeSmokeParticles.transform.position = transform.position;

            // Reproducir el ParticleSystem si es necesario
            var particleSystem = activeSmokeParticles.GetComponent<ParticleSystem>();
            if (particleSystem != null && !particleSystem.isPlaying)
            {
                particleSystem.Play();
            }

            // Actualizar la rotaci�n inicial
            UpdateSmokeParticlesRotation();
        }
    }

    private void DeactivateSmokeParticles()
    {
        // Desactivar la part�cula de humo si est� activa
        if (activeSmokeParticles != null && activeSmokeParticles.activeInHierarchy)
        {
            activeSmokeParticles.SetActive(false);
        }
    }


    public virtual void GetSteering() //Gets the Player Direction and Makes a Prediction
    {
        Vector2 direction = targetTransform.position - transform.position;
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
        Vector2 result = _targetTransform - (Vector2)transform.position;
        /* Flee
         Vector3 result =  transform.position -  _target.position;
         */
        result.Normalize();
        result *= actualAttackingSpeed;
        // transform.rotation = Quaternion.LookRotation(result);
        rb.velocity = result;
    }

    private void Chasing() //Chasing or lurking method
    {
        Vector2 result = targetTransform.position - transform.position; //Calculates the distance between player
        if (result.magnitude > satisfactionRadius) //Check if the distance is bigger than radiusLimit
        {
            result /= timeToTarget; // decrease the speed in relation to the time is target
            if (result.magnitude > actualChasingSpeed)
            {
                result.Normalize();
                result *= actualChasingSpeed;
            }
            rb.velocity = result;
        }
        else if (result.magnitude < satisfactionRadius - proximateError)
        {
            // get out of the circle
            result = transform.position - targetTransform.position;
            result *= actualfleeSpeed;
            rb.velocity = result;
        }
        else //lurking in circles
        {
            Vector2 center = targetTransform.position;
            Vector2 direction = ((Vector2)transform.position - center).normalized;
            Vector2 desiredPosition = center + (direction * satisfactionRadius);
            Vector2 velocity = (desiredPosition - (Vector2)transform.position).normalized * actualChasingSpeed;
            if (!isLurkingToTheRight)
            {
                rb.velocity = new Vector2(-direction.y, direction.x) * velocity.magnitude;
            }
            else
            {
                rb.velocity = new Vector2(direction.y, -direction.x) * velocity.magnitude;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /* if (other.gameObject.CompareTag("Player"))
         {
            KnockBack(other.transform.position - transform.position);
            Debug.Log(other.transform.position - transform.position);
         }*/
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

    private void ChangeOpacity(float _newOpacity) //change the opacity depends on the value
    {
        Color color = spriteRenderer.color;
        color.a = _newOpacity;
        spriteRenderer.color = color;
    }

    public void UnHit(Transform player)
    {

    }
}