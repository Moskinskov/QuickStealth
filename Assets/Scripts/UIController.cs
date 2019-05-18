using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject winUI;
        [SerializeField] private GameObject loseUI;
        [SerializeField] private GameObject menuUI;
        private bool gameOver;
        private bool menuOn;
        private int thisSceneID;
        private Player _player;


        private void Start()
        {
            thisSceneID = SceneManager.GetActiveScene().buildIndex;
            if (thisSceneID > 0)
            {
                winUI.SetActive(false);
                loseUI.SetActive(false);
                menuUI.SetActive(false);
                _player = FindObjectOfType<Player>();
                _player.OnGameOverEvent += ShowLosePanel;
                _player.OnWinEvent += ShowWinPanel;
                menuOn = false;
                Time.timeScale = 1;
            }
        }

        private void Update()
        {
            if (!gameOver)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ShowMenuPanel();
                }
            }
        }

        private void ShowThePanel(GameObject panel)
        {
            panel?.SetActive(true);
            gameOver = true;
            Time.timeScale = 0;
        }

        private void ShowWinPanel()
        {
            ShowThePanel(winUI);
        }

        private void ShowLosePanel()
        {
            ShowThePanel(loseUI);
        }
        private void ShowMenuPanel()
        {
            menuOn = !menuOn;
            menuUI?.SetActive(menuOn);
            if (menuOn)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            if (_player != null)
            {
                _player.OnGameOverEvent -= ShowLosePanel;
                _player.OnWinEvent -= ShowWinPanel;
            }
        }


        public void MenuBttn()
        {
            SceneManager.LoadScene(0);
        }
        public void AgainBttn()
        {
            SceneManager.LoadScene(thisSceneID);
        }
        public void ResumeBttn()
        {
            ShowMenuPanel();
        }
        public void NextLVLBttn()
        {
            SceneManager.LoadScene(thisSceneID++);
        }
        public void StartBttn()
        {
            SceneManager.LoadScene(1);
        }
        public void QuitBttn()
        {
            Application.Quit();
        }
    }
}