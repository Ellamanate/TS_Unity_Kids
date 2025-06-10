using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using Utils;

namespace Game
{
    public class MessageService : IDisposable
    {
        private readonly MessagesConfig _messagesConfig;
        private readonly TextMeshProUGUI _textField;
        
        private CancellationTokenSource _tokenSource;

        public MessageService(MessagesConfig messagesConfig, TextMeshProUGUI textField)
        {
            _messagesConfig = messagesConfig;
            _textField = textField;
            _tokenSource = new CancellationTokenSource();
            
            _textField.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _tokenSource.CancelAndDispose();
        }
        
        public void ShowMessage(string localizationKey)
        {
            // string message = LocalizationTable.GetTerm(localizationKey);
            
            _textField.text = localizationKey;
            
            _tokenSource = _tokenSource.Refresh();
            var token = _tokenSource.Token;
            
            _ = PlayPopUp(token);
        }

        private async UniTaskVoid PlayPopUp(CancellationToken cancellationToken)
        {
            _textField.gameObject.SetActive(true);
            
            await UniTask.Delay(
                TimeSpan.FromSeconds(_messagesConfig.MessageDuration),
                cancellationToken: cancellationToken);
            
            _textField.gameObject.SetActive(false);
        }
    }
}