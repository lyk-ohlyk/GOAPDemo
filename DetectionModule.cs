using System.Linq;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using GOAP;

namespace Unity.FPS.AI
{
    public class DetectionModule : MonoBehaviour
    {
        [Tooltip("The point representing the source of target-detection raycasts for the enemy AI test.")]
        public Transform DetectionSourcePoint;

        [Tooltip("The max distance at which the enemy can see targets")]
        public float DetectionRange = 15f;

        [Tooltip("The max distance at which the enemy can attack its target")]
        public float AttackRange = 10f;

        [Tooltip("Time before an enemy abandons a known target that it can't see anymore")]
        public float KnownTargetTimeout = 4f;

        [Tooltip("Optional animator for OnShoot animations")]
        public Animator Animator;

        public UnityAction onDetectedTarget;
        public UnityAction onLostTarget;

        public GameObject KnownDetectedTarget { get; private set; }
        public bool IsTargetInAttackRange { get; private set; }
        public bool IsSeeingTarget { get; private set; }
        public bool HadKnownTarget { get; private set; }

        protected float TimeLastSeenTarget = Mathf.NegativeInfinity;

        ActorsManager m_ActorsManager;

        const string k_AnimAttackParameter = "Attack";
        const string k_AnimOnDamagedParameter = "OnDamaged";

        AIBlackboard blackboard;
        AIPlanner planner;

        protected virtual void Start()
        {
            m_ActorsManager = FindObjectOfType<ActorsManager>();
            DebugUtility.HandleErrorIfNullFindObject<ActorsManager, DetectionModule>(m_ActorsManager, this);
        }

        public virtual void HandleTargetDetection(Actor actor, Collider[] selfColliders)
        {
            if (planner == null)
            {
                planner = gameObject.GetComponentInParent<AIPlanner>();
            }
            if (blackboard == null)
            {
                blackboard = gameObject.GetComponentInParent<AIBlackboard>();
            }

            if (blackboard == null || planner == null)
            {
                Debug.Log("lyk debug: object(planner/blackboard) is null!!!" + gameObject.name);
            }

            // Handle known target detection timeout
            if (KnownDetectedTarget && !IsSeeingTarget && (Time.time - TimeLastSeenTarget) > KnownTargetTimeout)
            {
                KnownDetectedTarget = null;
            }

            // Find the closest visible hostile actor
            float sqrDetectionRange = DetectionRange * DetectionRange;
            IsSeeingTarget = false;
            float closestSqrDistance = Mathf.Infinity;
            foreach (Actor otherActor in m_ActorsManager.Actors)
            {
                if (otherActor.Affiliation != actor.Affiliation)
                {
                    float sqrDistance = (otherActor.transform.position - DetectionSourcePoint.position).sqrMagnitude;
                    if (sqrDistance < sqrDetectionRange && sqrDistance < closestSqrDistance)
                    {
                        // Check for obstructions
                        RaycastHit[] hits = Physics.RaycastAll(DetectionSourcePoint.position,
                            (otherActor.AimPoint.position - DetectionSourcePoint.position).normalized, DetectionRange,
                            -1, QueryTriggerInteraction.Ignore);
                        RaycastHit closestValidHit = new RaycastHit();
                        closestValidHit.distance = Mathf.Infinity;
                        bool foundValidHit = false;
                        foreach (var hit in hits)
                        {
                            if (!selfColliders.Contains(hit.collider) && hit.distance < closestValidHit.distance)
                            {
                                closestValidHit = hit;
                                foundValidHit = true;
                            }
                        }

                        if (foundValidHit)
                        {
                            Actor hitActor = closestValidHit.collider.GetComponentInParent<Actor>();
                            if (hitActor == otherActor)
                            {
                                IsSeeingTarget = true;
                                closestSqrDistance = sqrDistance;

                                TimeLastSeenTarget = Time.time;
                                KnownDetectedTarget = otherActor.AimPoint.gameObject;
                            }
                        }
                    }
                }
            }

            if (KnownDetectedTarget != null)
            {

                Debug.Log("Position:" + KnownDetectedTarget.transform.position);

                blackboard?.SetBlackboardValue(BlackboardKeys.BBTargetName.Str, KnownDetectedTarget.name);

                float dist = Vector3.Distance(transform.position, KnownDetectedTarget.transform.position);
                IsTargetInAttackRange = KnownDetectedTarget != null && dist <= AttackRange;
                blackboard?.SetBlackboardValue(BlackboardKeys.BBTargetDist.Str, dist);

                if (IsSeeingTarget)
                {
                    planner?.SetCurrentWorldState(StateDef.IS_IN_DANGER, dist < 10f);
                    planner?.SetCurrentWorldState(StateDef.IS_TARGET_IN_RANGE, dist < 20f);

                    float y1 = gameObject.transform.rotation.eulerAngles.y;
                    float y2 = KnownDetectedTarget.transform.rotation.eulerAngles.y;
                    if (Mathf.Abs(y1 - y2) > 90f)
                    {
                        planner?.SetCurrentWorldState(StateDef.IS_IN_DANGER, true);
                    }
                }
                else
                {
                    planner?.SetCurrentWorldState(StateDef.IS_IN_DANGER, false);
                    planner?.SetCurrentWorldState(StateDef.IS_TARGET_IN_RANGE, false);
                }

                planner?.SetCurrentWorldState(StateDef.TARGET_IN_SIGHT, IsSeeingTarget);
            }

            // Detection events
            if (!HadKnownTarget &&
                KnownDetectedTarget != null)
            {
                OnDetect();

                planner?.SetCurrentWorldState(StateDef.HAS_TARGET, true);
                Debug.Log("Target name: " + KnownDetectedTarget.name);
            }

            if (HadKnownTarget &&
                KnownDetectedTarget == null)
            {
                OnLostTarget();

                blackboard?.SetBlackboardValue(BlackboardKeys.BBTargetName.Str, "");
                planner?.SetCurrentWorldState(StateDef.HAS_TARGET, false);
            }

            // Remember if we already knew a target (for next frame)
            HadKnownTarget = KnownDetectedTarget != null;
        }

        public virtual void OnLostTarget() => onLostTarget?.Invoke();

        public virtual void OnDetect() => onDetectedTarget?.Invoke();

        public virtual void OnDamaged(GameObject damageSource)
        {
            TimeLastSeenTarget = Time.time;
            KnownDetectedTarget = damageSource;

            if (Animator)
            {
                Animator.SetTrigger(k_AnimOnDamagedParameter);
            }
        }

        public virtual void OnAttack()
        {
            if (Animator)
            {
                Animator.SetTrigger(k_AnimAttackParameter);
            }
        }
    }
}