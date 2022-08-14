using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using RPG.SceneManagement;

namespace RGP.Core {
    public class Portal : MonoBehaviour
    {
        enum Destination {
            A,B,C,D,E,F,G,H
        }
        [SerializeField] int sceneIndex = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] Destination destination;
        [SerializeField] float fadeInTime = 0.3f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] float fadeWaitTime = 0.4f;

        private void OnTriggerEnter(Collider other) {
            if (other.tag =="Player"){
                StartCoroutine(Transistion());
            }
        }

        private IEnumerator Transistion(){
            if (sceneIndex < 0)
            {
                Debug.LogError("Scene to load nto set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(fadeOutTime);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneIndex);

            savingWrapper.Load();

            yield return new WaitForSeconds(fadeWaitTime);

            Portal otherPortal = GetOtherPortal();
            updatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);

            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal(){

            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;

                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }

        private void updatePlayer(Portal portal){
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<NavMeshAgent>().enabled =false;
                player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.transform.position);
                player.transform.rotation = portal.spawnPoint.transform.rotation;
                player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}


