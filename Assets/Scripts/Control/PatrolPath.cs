using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        float wayPointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWayPoint(i);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(GetWayPoint(i), wayPointGizmoRadius);
            
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            
            }
        }

        public Vector3 GetWayPoint(int item)
        {
            return transform.GetChild(item).position;
        }

        public int GetNextWayPoint(int index){
            if (index == transform.childCount - 1){
                return 0;
            } else {
                return index + 1;
            }
        }

    }
}

