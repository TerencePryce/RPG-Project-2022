using UnityEngine;
using RPG.Saving;
using System.Collections;

namespace RPG.SceneManagement {

    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "Save";
        Fader fader;
        [SerializeField] float fadeInTime = 0.5f;

        private void Awake() {
            Fader fader = FindObjectOfType<Fader>();
        }

        IEnumerator Start()
        {
            FindObjectOfType<Fader>().FadeOutImmedidately();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return new WaitForSeconds(1f);
            yield return FindObjectOfType<Fader>().FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}

