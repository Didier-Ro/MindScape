using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] private float _maxSpeed;
      [SerializeField] private Transform _target;
      
      [SerializeField] private Rigidbody2D _targetRb;
  
      [SerializeField] private Vector2 _seekTarget;
      
      [SerializeField] private Rigidbody2D _rb;
      private float prediction;
      [SerializeField] private float maxPrediction;
  
  
      public void AssignTarget(GameObject target)
      {
          _target = target.transform;
          _targetRb = target.GetComponent<Rigidbody2D>();
      }
  
      private void KinematicSeek(Vector2 targetTransform)
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
  
      public virtual void GetSteering()
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
      void Start()
      { 
          _rb = GetComponent<Rigidbody2D>();
      }
      void FixedUpdate()
      {
          GetSteering();
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
  
  
  }
