using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity;
using UnityEngine.SceneManagement;


namespace BasicMenuOptions
{

    public class MenuManager : MonoBehaviour
    {

        [Header("Loading UI"), SerializeField]
        private GameObject UI;
        public GameObject Menu;
        public PlayerController player;

        [SerializeField]
        private Slider loadingSlider;

        [SerializeField]
        private Button btnSetup;

        [SerializeField]
        private Button btnQuit;

        private void Start()
        {
            Menu.SetActive(false);
        }

        private void OnEnable()
        {
            btnSetup.onClick.AddListener(Setting);
            btnQuit.onClick.AddListener(Quit);
        }

        private void OnDisable()
        {
            btnSetup.onClick.RemoveListener(Setting);
            btnQuit.onClick.RemoveListener(Quit);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SettingExit();
            }
        }
        private void Quit()
        {
            GameManagerScript.Instance.ButtonQuitGame();
        }

        public void Setting()   
        {
            Menu.SetActive(true);
            Time.timeScale = 0;
            GameManagerScript.Instance.player.canAttack = false;
        }

        public void SettingExit()
        {
            Menu.SetActive(false);
            Time.timeScale = 1;
            GameManagerScript.Instance.player.canAttack = true;
        }

        public void OpenSceneWithoutLoadingUI(string levelname)
        {
            SceneManager.LoadScene(levelname);
        }

        public void DeletePlayerPreps()
        {
            PlayerPrefs.DeleteAll();
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadScene_Coroutine(sceneName));
        }

        public IEnumerator LoadScene_Coroutine(string sceneName)
        {
            loadingSlider.value = 0.1f;

            UI.SetActive(true);

            Time.timeScale = 1;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            asyncOperation.allowSceneActivation = false;

            float progress = 0;

            while (!asyncOperation.isDone)
            {
                progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
                loadingSlider.value = progress;

                if (progress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}

