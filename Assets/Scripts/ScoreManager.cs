using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [Header("Players Settings")]
        [SerializeField] PlayerSettingsSO player1Settings;
        [SerializeField] PlayerSettingsSO player2Settings;
        [Header("Players life images")]
        [SerializeField] private Image[] imgPlayer1Life;
        [SerializeField] private Image[] imgPlayer2Life;
        [Header("Game Timer")]
        [SerializeField] private TextMeshProUGUI textTimer;
        public int time = 60;
        [Header("Speed Up")]
        [SerializeField] private int speedUpEverySeconds = 15;
        [SerializeField] private float speedIncrement = 0.5f;
        [Header("Audio")]
        [SerializeField] AudioClip audioClipWinSound;

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
            AudioController.Instance.StopBackgroundMusic();
            AudioController.Instance.PlaySoundEffect(audioClipWinSound);
            Time.timeScale = 0f;

            if(player1Life == 0)
                HUDManager.Instance.ShowPanelPlayerWon(player2Settings.PlayerName);
            else
                HUDManager.Instance.ShowPanelPlayerWon(player1Settings.PlayerName);
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

                if (time > 0 && time % speedUpEverySeconds == 0)
                    BallMovement.Instance.ballSpeed += speedIncrement;

                int minutes = (int)(time / 60);
                int seconds = (int)(time % 60);

                if (seconds < 10)
                    BallMovement.Instance.ballSpeed += 0.5f;

                textTimer.text = minutes.ToString() + ":" + seconds.ToString("00");
            }

            EndGame();
        }
    }
}