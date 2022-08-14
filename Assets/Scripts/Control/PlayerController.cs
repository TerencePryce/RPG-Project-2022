using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float speedFraction = 1f;
        Fighter fighter;
        Health health;

        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit [] hits =  Physics.RaycastAll(RayMouseHit());
                foreach (RaycastHit hit in hits)
                {
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                    if (target == null) continue;

                    if (!fighter.CanAttack(target.gameObject)) continue;

                    if (Input.GetMouseButton(0))
                    {
                        fighter.Attack(target.gameObject);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(RayMouseHit(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, speedFraction);
                }
                return true;
            }
            return false;
        }

        private static Ray RayMouseHit()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}