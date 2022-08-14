using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat{
    public class Fighter : MonoBehaviour,IAction
    {
        [SerializeField] float weaponRange = 4.0f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float speedFraction = 1f;

        Health target = null;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.GetComponent<Health>().IsDead()) return;

                if (!GetIsInRange())
                {
                    GetComponent<Mover>().MoveTo(target.transform.position, speedFraction);
                }
                else
            {
                GetComponent<ActionScheduler>().StartAction(this);
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform.position);

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        public bool CanAttack(GameObject target)
        {
            return !target.GetComponent<Health>().IsDead();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }
    }
}