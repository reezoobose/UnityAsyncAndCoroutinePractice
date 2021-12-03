using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class FillUpManager : MonoBehaviour
    {
        public Image loaderOne;
        public Image loaderThree;
        public Image loaderTwo;
        public bool sync ;
        public double timeToLoad = 100f;
        public bool useTask;

        private async void Start()
        {
            if (!useTask)
            {
                if (sync) MakeLoadingCompleteOverAsync(timeToLoad);
                else MakeLoadingCompleteOverSync(timeToLoad);
            }

            /* var task = MakeLoadingCompleteOverAsync(timeToLoad,loaderOne); */
            /* await MakeLoadingCompleteOverInSerially(timeToLoad); */

            await MakeLoadingCompleteOverInParallel(timeToLoad);
           
            

        }

        #region Task Based
        
        
        private async Task MakeLoadingCompleteOverInSerially(double time)
        {
            await MakeLoadingCompleteOverAsync(time,loaderThree);
            await MakeLoadingCompleteOverAsync(time,loaderOne);
            await MakeLoadingCompleteOverAsync(time,loaderTwo);
            Debug.Log("All loadings are done ");
        }

        private async Task MakeLoadingCompleteOverInParallel(double time)
        {
            var taskOne = MakeLoadingCompleteOverAsync(time, loaderThree);
            var taskTwo = MakeLoadingCompleteOverAsync(time, loaderOne);
            var taskThree = MakeLoadingCompleteOverAsync(time, loaderTwo);
            await Task.WhenAll(taskOne, taskTwo, taskThree);
            Debug.Log("All loadings are done ");
        }


        private static async Task MakeLoadingCompleteOverAsync(double time, Image loader)
        {
            var startTime = Time.realtimeSinceStartup;
            var endTime = DateTime.Now.AddSeconds(time);
            while (DateTime.Now < endTime)
            {
                var timePassed = endTime - DateTime.Now;
                var timePassedInSeconds = time - timePassed.Seconds;
                var percentage = timePassedInSeconds/time;
                loader.fillAmount = (float)percentage;
                //will force your method to be asynchronous, and return control at that point.
                //The rest of the code will execute at a later time (at which point, it still may run synchronously) on the current context.
                await Task.Yield();
            }
            Debug.Log($"ExtraTime Task:{Time.realtimeSinceStartup - startTime }");
           
        }

        #endregion

        #region Coroutine
        
        private void MakeLoadingCompleteOverAsync(double time)
        {
            var routineOne = MakeLoadingCompleteOverRoutine(time,loaderOne,null);
            var threadOne = StartCoroutine(routineOne);
            var routineTwo = MakeLoadingCompleteOverRoutine(time,loaderThree, null);
            var threadTwo = StartCoroutine(routineTwo);
            var routineThree = MakeLoadingCompleteOverRoutine(time,loaderTwo,null);
            var threadThree = StartCoroutine(routineThree);
            
        }
        private void MakeLoadingCompleteOverSync(double time)
        {
           
           
            var routineOne = MakeLoadingCompleteOverRoutine(time, loaderOne, () =>
            {
                 var routineTwo = MakeLoadingCompleteOverRoutine(time,loaderThree, () =>
                 {
                      var routineThree = MakeLoadingCompleteOverRoutine(time,loaderTwo,null);
                      StartCoroutine(routineThree);
                 });

                 StartCoroutine(routineTwo);
            });
            StartCoroutine(routineOne);

            
        }

        private static IEnumerator MakeLoadingCompleteOverRoutine(double time , Image loader , Action done)
        {
            var startTime = Time.realtimeSinceStartup;
            var endTime = DateTime.Now.AddSeconds(time);
            while (DateTime.Now < endTime)
            {
               var timePassed = endTime - DateTime.Now;
               var timePassedInSeconds = time - timePassed.Seconds;
               var percentage = timePassedInSeconds/time;
               loader.fillAmount = (float)percentage;
               yield return null;
            }
            Debug.Log($"ExtraTime Routine :{Time.realtimeSinceStartup - startTime } by loader = {loader.name}");
            done?.Invoke();   
        }
        #endregion
    }
}
