using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public bool CinematicCamera = false;
   [SerializeField] private bool follow = false;
   [SerializeField] private float Rotate_inDegrees = 60;
   [SerializeField] private Transform ChildCamera;
    public Transform Target;
   //[SerializeField] private Transform SourceCamera;
   [SerializeField] private Transform CinematicPoint;
   [SerializeField] private Vector3 offset;
   [SerializeField] private Vector3 NormalOffsetFromtransform;
   [SerializeField] private float RotateSpeed = 2;
   [SerializeField] private float RotateLimit = 60;
   [SerializeField] private Vector3 WinCamOffset;
   [SerializeField] private Transform FocusPoint;
   //[SerializeField] private float CameraRotationFromFOcusPoint = 60;
   [SerializeField] private Transform FocusPointBack;
   private Vector3 cameraposAfterCalculation = Vector3.zero;
   Vector3 CamPos=Vector3.zero;
   public static CameraFollow Instance{ get; private set; }
   public delegate void CameraFocusGhost();

   public static event CameraFocusGhost OnCameraFocusGhost;

   private void Awake()
   {
      if (Instance != null && Instance != this) 
      { 
         Destroy(this); 
      } 
      else 
      { 
         Instance = this; 
      }
   }

 
   private void ReceivePlayer(Transform player)
   {
      Target = player;
   }

   private void WinCam()
   {
      follow = false;
      NormalOffsetFromtransform = WinCamOffset;
      transform.DOMove(Target.position - WinCamOffset, 1);
   }

   private void Start()
   {
      cameraposAfterCalculation = offset;
      transform.eulerAngles=Vector3.zero;
      /*
      DOVirtual.DelayedCall(2, () =>
      {
         FocusOnVictim(FocusPoint,FocusPointBack);
      });
      */
      
   }

   private void Update()
   {

      if (follow)
      {
         
         CamPos = Target.position - NormalOffsetFromtransform;
         CamPos.x = 00;
         transform.position = CamPos;
      }

      if (CinematicCamera)
      {
         if (ChildCamera.eulerAngles.y >= RotateLimit)
         {
            CinematicCamera = false;
            return;
         }

         ChildCamera.RotateAround(CinematicPoint.position, Vector3.up, Rotate_inDegrees * Time.deltaTime * RotateSpeed);
      }

   }

   public void PlayCinematicMode(bool Status)
   {
      if (Status)
      {
         ChildCamera.localRotation=Quaternion.identity;
         ChildCamera.localPosition=Vector3.zero;
         CinematicCamera = true;
      }
      else if (!Status)
      {
         RotateCameraToOriginalPos();
      }

   }

   void RotateCameraToOriginalPos()
   {
      ChildCamera.DOLocalMove(Vector3.zero, 1f);
      ChildCamera.DOLocalRotate(Vector3.zero, 1f);
   }

   public void FocusOnVictim(Transform FocusPoint,Transform FocusPointBack)
   {
      ChildCamera.localRotation = FocusPoint.localRotation;
      ChildCamera.DOLocalMove(FocusPoint.localPosition, 1f);
      if (FocusPointBack==null)
      {
         return;  
      }
      DOVirtual.DelayedCall( 3,() =>
      {
            //ChildCamera.DOLocalRotate
            //(transform.eulerAngles + new Vector3(0, CameraRotationFromFOcusPoint, 0), 0.15f);
            OnCameraFocusGhost?.Invoke();
            ChildCamera.DOLocalRotate(FocusPointBack.localEulerAngles, 0.25f);
            ChildCamera.DOLocalMove(FocusPointBack.localPosition, 0.2f);
      });
   }

   void FollowEnemyBoss(Transform FollowTarget)
   {
      if (FollowTarget.tag=="Player")
      {
         follow = false;
         Target = FollowTarget;
         WinCam();
      }
      Target = FollowTarget;
   }

   public void CameraShake()
   {
      ChildCamera.DOShakePosition(1, 2, 3, 4).SetEase(Ease.Linear);
   }
}
