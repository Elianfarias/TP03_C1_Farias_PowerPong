using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [Header("Players life images")]
        [SerializeField] private Image[] imgPlayer1Life;
        [SerializeField] private Image[] imgPlayer2Life;

        [Header("Game Timer")]
        [SerializeField] private TextMeshProUGUI textTimer;
        public int time = 60;

        private int player1Life = 2, player2Life = 2;

        private void Awake()
        {
            Instance = this;
            StartTimer();
        }

        public void RemoveLifePlayer1()
        {
            imgPlayer1Life[player1Life].gameObject.SetActive(false);

            if (player1Life == 0)
            {
                EndGame();
                return;
            }

            player1Life--;
        }

        public void RemoveLifePlayer2()
        {
            imgPlayer2Life[player2Life].gameObject.SetActive(false);

            if (player2Life == 0)
            {
                EndGame();
                return;
            }

            player2Life--;
        }

        private void EndGame()
        {
            if (player1Life < 0)
                Debug.Log("Player 2 Wins!");
            else if (player2Life < 0)
                Debug.Log("Player 1 Wins!");

            ResetGame();
        }

        private void ResetGame()
        {
            player1Life = 2;
            player2Life = 2;
            foreach (var img in imgPlayer1Life)
                img.gameObject.SetActive(true);
            foreach (var img in imgPlayer2Life)
                img.gameObject.SetActive(true);
        }

        private void StartTimer()
        {
            StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            while (time > 0)
            {
                yield return new WaitForSeconds(1f);
                time--;
                int minutes = (int)(time / 60);
                int seconds = (int)(time % 60);
                textTimer.text = minutes.ToString() + ":" + seconds.ToString();
            }

            EndGame();
        }
    }
}