using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public static class SomeExample
    {
        private const int Multiplier = 1000;
        private static async Task<float> GetRandomNumber(int withDelay, string id ="")
        {
            Debug.Log($"GetRandomNumber for id = {id}");
            var number = Random.Range(0f, 100f);
            await Task.Delay(withDelay * Multiplier);
            Debug.Log($"Task Complete result = {number} with id = {id}");
            return number;
        }

        public static async Task<float> GetARandomNumberFromTwoRandomGeneratorSync()
        {
             return await GetRandomNumber(3,"Left operand ") + await GetRandomNumber(4,"Right operand");
        }

        public static async Task<float> GetARandomNumberASync(int withDelay)
        {
            var taskOne = GetRandomNumber(withDelay, "takOne");
            var taskTwo = GetRandomNumber(withDelay, "taskTwo");
            var taskThree = GetRandomNumber(withDelay, "TaskThree");
            var parallelTak = await Task.WhenAny(taskOne, taskTwo, taskThree);
            return parallelTak.Result;
        }
    }
}
