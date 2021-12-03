using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Script
{
    public static class HttpRequestWithUnity
    {
        
        public static void GetTimeWithRoutine(MonoBehaviour reference)
        {
            var routine = UnityWebRequestWithCoroutine("http://worldtimeapi.org/api/timezone/Asia/Kolkata");
            var coroutine = reference.StartCoroutine(routine);
        }

        public static async Task<string> GetTimeAsync( )
        {
            return await UnityWebRequestWithAsync("http://worldtimeapi.org/api/timezone/Asia/Kolkata");
        }
        private static IEnumerator UnityWebRequestWithCoroutine(string uri)
        {
            using var request = new UnityWebRequest(uri,UnityWebRequest.kHttpVerbGET);
            var downloadHandlerBuffer = new DownloadHandlerBuffer();
            request.downloadHandler = downloadHandlerBuffer;
            yield return request.SendWebRequest();
            Debug.Log(request.result != UnityWebRequest.Result.Success
                ? request.error
                : request.downloadHandler.text);
        }

        //This method does not work .
        private static async Task<string> UnityWebRequestWithAsync(string uri)
        {
            
            using var request = new UnityWebRequest(uri,UnityWebRequest.kHttpVerbGET);
            var downloadHandlerBuffer = new DownloadHandlerBuffer();
            request.downloadHandler = downloadHandlerBuffer;
            request.SendWebRequest();
            await Task.Yield();
            string result;
            if(request.result != UnityWebRequest.Result.Success)
            { 
                //Debug.Log(request.error);
                result = request.error;
            }
            else
            {
                //Debug.Log(request.downloadHandler.text);
                result = request.downloadHandler.text;
            }
            return result;
        }
    }
}