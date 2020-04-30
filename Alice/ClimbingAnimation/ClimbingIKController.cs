using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace ClimbingIKController
{
    [Serializable]
    public class StateMachine
    {
        public enum State { Wait, MoveEven, WaitAgain, MoveOdd, Exit, Enter }
        [SerializeField]
        private State currentState;

        public StateMachine()
        {
            currentState = State.Exit;
        }

        public void ToEnter()
        {
            currentState = State.Enter;
        }

        public void ToExit()
        {
            currentState = State.Exit;
        }

        public void ToMoveEven()
        {
            currentState = State.MoveEven;
        }

        public void ToMoveOdd()
        {
            currentState = State.MoveOdd;
        }

        public void ToWait()
        {
            currentState = State.Wait;
        }

        public void ToWaitAgain()
        {
            currentState = State.WaitAgain;
        }

        public State GetCurrentState()
        {
            return currentState;
        }
    }

    [Serializable]
    public class Limb
    {
        private Animator animator;
        [SerializeField]
        public AvatarIKGoal goal;
        [SerializeField]
        public Transform transform;
        [SerializeField]
        public Vector3 positionLock, oldPosition, currentPosition;
        [SerializeField]
        public bool locked;

        public Limb(Animator animator, AvatarIKGoal goal, Transform transform)
        {
            this.animator = animator;
            this.goal = goal;
            this.transform = transform;
            positionLock = transform.position;
            locked = false;
        }

        public void Lock()
        {
            locked = true;
            oldPosition = transform.position;
        }

        public void ClearLock()
        {
            locked = false;
        }

        public void UpdateCurrentPosition()
        {
            currentPosition = Vector3.Lerp(transform.position, positionLock, 1.5f * Time.deltaTime);
            MoveIKElement(positionLock, transform.rotation);
        }


        private void MoveIKElement(Vector3 position, Quaternion rotation)
        {
            animator.SetIKPositionWeight(goal, 1);
            animator.SetIKRotationWeight(goal, 1);
            animator.SetIKPosition(goal, position);
            animator.SetIKRotation(goal, rotation);
        }
    }

    public class ClimbingIKController : MonoBehaviour
    {
        [SerializeField]
        private Transform lHand, rHand, lFoot, rFoot;
        [SerializeField]
        private LayerMask climbingRayMask;

        private Animator animator;
        private float rayRange = 0.6f;
        private float rayMaxDistance = 0.6f;
        private int rayDivisions = 6;
        private Vector3 relativeUp = Vector3.up;
        private RaycastHit hit;
        [SerializeField]
        private float stepDistance = 1f;
        [SerializeField]
        private Vector3 lHandLock, rHandLock, lFootLock, rFootLock;
        [SerializeField]
        private Limb lHandLimb, rHandLimb, lFootLimb, rFootLimb;
        [SerializeField]
        private StateMachine stateMachine;
        private List<Limb> allLimbs = new List<Limb>();
        private List<Limb> oddLimbs = new List<Limb>();
        private List<Limb> evenLimbs = new List<Limb>();

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            stateMachine = new StateMachine();
            lHandLimb = new Limb(animator, AvatarIKGoal.LeftHand, lHand);
            lFootLimb = new Limb(animator, AvatarIKGoal.LeftFoot, lFoot);
            rHandLimb = new Limb(animator, AvatarIKGoal.RightHand, rHand);
            rFootLimb = new Limb(animator, AvatarIKGoal.RightFoot, rFoot);
            allLimbs.Add(lHandLimb);
            allLimbs.Add(lFootLimb);
            allLimbs.Add(rHandLimb);
            allLimbs.Add(rFootLimb);
            oddLimbs.Add(lHandLimb);
            oddLimbs.Add(rFootLimb);
            evenLimbs.Add(rHandLimb);
            evenLimbs.Add(lFootLimb);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (Input.GetKeyDown("k"))
            {
                stateMachine.ToEnter();
            }
            else if (Input.GetKeyDown("l"))
            {
                stateMachine.ToExit();
            }
            else if (Input.GetKeyDown("i"))
            {
                foreach (Limb l in evenLimbs)
                {
                    l.ClearLock();
                }
                stateMachine.ToMoveEven();
            }
            else if (Input.GetKeyDown("o"))
            {
                foreach (Limb l in oddLimbs)
                {
                    l.ClearLock();
                }
                stateMachine.ToMoveOdd();
            }
            // If Enter state, should find locks for all  limbs
            if (stateMachine.GetCurrentState().Equals(StateMachine.State.Enter))
            {
                FindTargetPosition(lHandLimb, -transform.forward);
                FindTargetPosition(lFootLimb, -transform.forward);
                FindTargetPosition(rFootLimb, -transform.forward);
                FindTargetPosition(rHandLimb, -transform.forward);

            }
            // If Exit state, should clear locks for all  limbs
            if (stateMachine.GetCurrentState().Equals(StateMachine.State.Exit))
            {
                lHandLimb.ClearLock();
                lFootLimb.ClearLock();
                rHandLimb.ClearLock();
                rFootLimb.ClearLock();
            }
            // If move odd
            if (stateMachine.GetCurrentState().Equals(StateMachine.State.MoveOdd))
            {
                foreach (Limb l in oddLimbs)
                {
                    FindTargetPosition(l, -transform.forward);
                }
            }
            // If move even
            if (stateMachine.GetCurrentState().Equals(StateMachine.State.MoveEven))
            {
                foreach (Limb l in evenLimbs)
                {
                    FindTargetPosition(l, -transform.forward);
                }
            }

            if (!stateMachine.GetCurrentState().Equals(StateMachine.State.Exit))
            {
                foreach (Limb l in allLimbs)
                {
                    l.UpdateCurrentPosition();
                }
            }

        }

        private void MoveIKElement(AvatarIKGoal goal, Vector3 position, Quaternion rotation)
        {
            animator.SetIKPositionWeight(goal, 1);
            animator.SetIKRotationWeight(goal, 1);
            animator.SetIKPosition(goal, position);
            animator.SetIKRotation(goal, rotation);
        }

        private void FindTargetPosition(Limb limb, Vector3 normal)
        {
            if (limb.locked)
            {
                return;
            }
            Vector3 v1 = GetVectorsFromNormal(normal)[0];
            Vector3 v2 = GetVectorsFromNormal(normal)[1];
            Vector3 basePoint = limb.transform.position + relativeUp * stepDistance;
            Vector3 pointToRaycast = basePoint;
            for (int i = 0; i < rayDivisions; i++)
            {
                for (int j = 0; j < rayDivisions; j++)
                {
                    pointToRaycast = basePoint + v1 * (rayRange * i / rayDivisions - rayRange * 0.5f) + v2 * (rayRange * j / rayDivisions - rayRange * 0.5f);
                    Debug.DrawRay(pointToRaycast, -normal, Color.green, Time.deltaTime);
                    // Debug.Log("i=" + i + ";j=" + j + ";delta1=" + (v1 * rayRange * i / rayDivisions) + ";delta2=" + (v2 * rayRange * j / rayDivisions));
                    bool b = Physics.Raycast(pointToRaycast, -normal, out hit, rayMaxDistance, climbingRayMask);
                    if (b)
                    {
                        limb.positionLock = hit.point;
                        limb.Lock();
                    }
                }
            }
        }

        private Vector3[] GetVectorsFromNormal(Vector3 normal)
        {
            return new Vector3[] { Vector3.up, Vector3.right };
        }

        private void SetLockForGoal(AvatarIKGoal goal, Vector3 position)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    lFootLock = position;
                    break;
                case AvatarIKGoal.LeftHand:
                    lHandLock = position;
                    break;
                case AvatarIKGoal.RightFoot:
                    rFootLock = position;
                    break;
                case AvatarIKGoal.RightHand:
                    rHandLock = position;
                    break;
                default:
                    Debug.LogError("Cant lock position.");
                    break;
            }
        }
    }
}