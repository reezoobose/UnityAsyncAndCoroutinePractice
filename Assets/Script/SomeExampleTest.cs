using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Script
{
    public class SomeExampleTest : MonoBehaviour
    {
        private async void Start()
        {
            var result = await  TestFour();
            Debug.Log($"Response from the server received : {result} ");
            await _exampleOfLambdaHelloAsync();
        }

        private async void TestOne()
        {
            var result = await SomeExample.GetARandomNumberASync(10);
            Debug.Log($"$result received {result}");
        }

        private void TestTwo()
        {
            HttpRequestWithUnity.GetTimeWithRoutine(this);
        }

        private static async Task<string> TestThree()
        {
            return await HttpRequestWithUnity.GetTimeAsync();
        }

        private static async Task<float> TestFour()
        {
            return await SomeExample.GetARandomNumberFromTwoRandomGeneratorSync();
        }


        private readonly Func<Task> _exampleOfLambdaHelloAsync = async () =>
        {
                await Task.Delay(TimeSpan.FromSeconds(10));
                Debug.Log("Hello world");
        };

    }
}