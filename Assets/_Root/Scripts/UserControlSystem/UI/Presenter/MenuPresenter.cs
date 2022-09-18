using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UserControlSystem.UI.Presenter
{
    public class MenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _backButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false));
#if UNITY_EDITOR
            _exitButton.OnClickAsObservable().Subscribe(_ => UnityEditor.EditorApplication.isPlaying = false);
#endif
            _exitButton.OnClickAsObservable().Subscribe(_ => Application.Quit());
        }
    }
}