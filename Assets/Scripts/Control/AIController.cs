using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control {
    
    public class AIController : MonoBehaviour {

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float wayPointTolerance = 2f;
        [SerializeField] float wayPointDwellTime = 3f;
        [SerializeField] float speedFraction = 0.3f;

        GameObject player;
        Fighter fighter;
        Health health;
        Vector3 guardPosition;
        Mover mover;

        float timeSinceLastSeenPlayer = Mathf.Infinity;
        float timeSinceReachedWayPoint = Mathf.Infinity;
        int currentWayPointIndex = 0;
        
        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>(); 
            guardPosition = transform.position; 
        }

        private void Update()
        {

            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (!hasGivenUp())
            {
                SuscipionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            Updatetimers();
        }

        private void Updatetimers()
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            timeSinceReachedWayPoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            guardPosition = transform.position;

            if (patrolPath != null){
                    if (AtWayPoint())
                    {
                        currentWayPointIndex = CycleWayPoint();
                    }else{
                        nextPosition = GetCurrentWayPoint();
                    }
                    if(dwellTimeReached()){
                        mover.StartMoveAction(nextPosition, speedFraction);
                    }
                    
            }
        }

        private bool AtWayPoint(){
            float distanceToWayPoint = Vector3.Distance(transform.position,GetCurrentWayPoint());
            return distanceToWayPoint < wayPointTolerance;
        }

        private int CycleWayPoint()
        {
            timeSinceReachedWayPoint = 0;
            return patrolPath.GetNextWayPoint(currentWayPointIndex);
            //return patrolPath.GetNextWayPoint(currentWayPointIndex);
        }

        private Vector3 GetCurrentWayPoint(){
            return patrolPath.GetWayPoint(currentWayPointIndex);
        }

        private void SuscipionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSeenPlayer = 0;
            fighter.Attack(player.gameObject);
        }

        private bool hasGivenUp()
        {
            return timeSinceLastSeenPlayer > suspicionTime;
        }

        private bool dwellTimeReached()
        {
            return timeSinceReachedWayPoint > wayPointDwellTime;
        }

        private bool InAttackRangeOfPlayer()
        {
            float distaceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distaceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance / 2);
        }
    }
}