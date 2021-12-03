using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class ButtonPanel : MonoBehaviour
    {
        public Button[] buttons;
        private async  void Start()
        {
            var buttonClickTask = string.Empty;
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                buttonClickTask = await GetTheButtonClicked(cancellationTokenSource);
            }
            catch (Exception exception)
            {
                Debug.Log($"Exception {exception.Message}");
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }

            Debug.Log($"Button clicked {buttonClickTask} ");
        }

        private async Task<string> GetTheButtonClicked(CancellationTokenSource tokenSource ) 
        {
           
            var listOfTask = buttons.Select(button => ButtonClicked(button, tokenSource)).ToList();
            var result = await Task.WhenAny(listOfTask);
            tokenSource.Cancel();
            return result.Result.name;
        }

        private static async Task<Button> ButtonClicked(Button attachedButton , CancellationTokenSource tokenSource )
        {
            var anyButtonClicked = false;
            if (!ReferenceEquals(attachedButton, null))
            {
                attachedButton.onClick.AddListener(() =>
                {
                    anyButtonClicked = true;
                });
            }
            

            while (!anyButtonClicked)
            {
                if (tokenSource.IsCancellationRequested)
                {
                    Debug.Log($" Button click permission {attachedButton.name} has been revoked .");
                    tokenSource.Token.ThrowIfCancellationRequested();
                }
                await Task.Yield();
            }
            return attachedButton;
        }
    }
}
