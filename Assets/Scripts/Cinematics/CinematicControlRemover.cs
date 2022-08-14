using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics {
    
    public class CinematicControlRemover : MonoBehaviour {

        GameObject player;
        
        private void Awake() {
            player = GameObject.FindWithTag("Player");

            GetComponent<PlayableDirector>().played += onEnabledControl;
            GetComponent<PlayableDirector>().stopped += onDisableControl;
        } 

        void onEnabledControl(PlayableDirector pd){
            print("Enabling ...");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void onDisableControl(PlayableDirector pd){
                print("Disabling ...");
                player.GetComponent<PlayerController>().enabled = true;
        }
    }

}