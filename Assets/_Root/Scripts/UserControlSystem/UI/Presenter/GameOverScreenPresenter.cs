using Abstractions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UserControlSystem.UI.Presenter
{
    public class GameOverScreenPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _winnerText;
        [SerializeField] private Button _exitButton;

        [Inject] private IGameStatus _gameStatus;

        [Inject]
        private void Init()
        {
            _gameStatus.Status.ObserveOnMainThread().Subscribe(number =>
            {
                gameObject.SetActive(true);
                _winnerText.text = number == 0 ? "Draw!" : $"Winner: Faction №{number}!";
            });

#if UNITY_EDITOR
            _exitButton.OnClickAsObservable().Subscribe(_ => UnityEditor.EditorApplication.isPlaying = false);
#endif
            _exitButton.OnClickAsObservable().Subscribe(_ => Application.Quit());
        }
    }
}