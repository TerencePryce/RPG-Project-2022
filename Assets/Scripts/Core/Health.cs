using RPG.Saving;
using UnityEngine;
 
namespace RPG.Core{
    
    public class Health : MonoBehaviour, ISaveable {
        
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        public bool IsDead(){
            return isDead;
        }

        private void Update() {
            if (healthPoints == 0){
                if (!isDead)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
        }

        public void TakeDamage(float damage){
            healthPoints = Mathf.Max(healthPoints - damage, 0);
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}