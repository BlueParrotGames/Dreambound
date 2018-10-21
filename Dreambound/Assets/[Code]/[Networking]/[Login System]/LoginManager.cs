using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Dreambound.Networking.Utility;
using Dreambound.Networking.Threading;

namespace Dreambound.Networking.LoginSystem
{
    public class LoginManager : ThreadObject
    {
        [SerializeField] private string _connectionIp;
        [SerializeField] private int _connectionPort;

        [Header("UI Properties")]
        [SerializeField] private InputField _emailText;
        [SerializeField] private InputField _passwordText;
        [SerializeField] private Text _feedbackText;

        private NetworkManager _networkManager;

        private void Awake()
        {
            //Events settings
            SetFunction(UpdateFeedBackText);
            NetworkEvents.Instance.OnLoginAttempt += Listener;
        }
        private void Start()
        {
            _networkManager = FindObjectOfType<NetworkManager>();
            _networkManager.ConnectUsingSettings(_connectionIp, _connectionPort);
        }

        public void Login()
        {
            _networkManager.Login("test", "test");
            //_networkHandler.Login(_emailText.text, _passwordText.text);
        }

        private void UpdateFeedBackText(object loginState, object gamePerk)
        {
            switch ((LoginState)loginState)
            {
                case LoginState.CannotConnectToDatabase:

                    _feedbackText.text = "Could not connect to user database!";
                    break;
                case LoginState.SuccelfullLogin:

                    if ((int)gamePerk <= 0) _feedbackText.text = "You do not own this game";
                    else _feedbackText.text = "Succesfully logged in!";
                    break;
                case LoginState.UserAlreadyLoggedIn:

                    _feedbackText.text = "Someone is already logged in with this account";
                    break;
                case LoginState.wrongUsernameOrPassword:

                    _feedbackText.text = "Password, Username or Email is incorrect";
                    break;
            }
        }
    }
}
