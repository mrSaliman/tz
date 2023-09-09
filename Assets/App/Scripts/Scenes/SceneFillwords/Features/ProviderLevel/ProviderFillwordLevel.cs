using System;
using System.Collections.Generic;
using System.IO;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using Unity.VisualScripting;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private const string PackPath = "Fillwords/pack_0";
        private const string WordsPath = "Fillwords/words_list";

        private static readonly TextAsset file1 = Resources.Load(PackPath) as TextAsset;
        private static readonly TextAsset file2 = Resources.Load(WordsPath) as TextAsset;

        private static readonly string[] input = file1.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        private static readonly string[] words = file2.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        public GridFillWords LoadModel(int index)
        {
            char[][] level;
            do
            {
                Debug.Log($"Loading level {index}");
                level = GetLevel(index);
                index++;
            }
            while (index <= input.Length && level == null);
            if (level == null)
            {
                throw new Exception("No valid level found");
            }

            var model = new GridFillWords(new(level.Length, level.Length));
            for (int i = 0; i < level.Length; i++)
                for (int j = 0; j < level[i].Length; j++) model.Set(i, j, new(level[i][j]));
            
            return model;
        }

        private char[][] GetLevel(int index)
        {
            index--;
            var level = input[index].Split(' ');

            var charCount = 0;
            var maxCharNumber = 0;
            var idxLevel = new List<List<int>>();

            for (int i = 0; i + 1 < level.Length; i += 2)
            {
                List<int> word = new();
                try
                {
                    int result = int.Parse(level[i]);
                    word.Add(result);

                    var chars = level[i + 1].Split(';');
                    if (chars.Length == words[word[0]].Length)
                    {
                        charCount += chars.Length;
                        foreach (string c in chars)
                        {
                            result = int.Parse(c);
                            word.Add(result);
                            if (result > maxCharNumber) maxCharNumber = result;
                        }
                    }
                    else
                    {
                        Debug.Log("Incorrect input.");
                        return null;
                    }
                }
                catch (FormatException)
                {
                    Debug.Log("Incorrect input.");
                    return null;
                }
                idxLevel.Add(word);
            }

            var mapSize = (int)Math.Floor(Math.Sqrt(charCount));
            // проверка на соответствие количества символов к квадратной сетке карты
            if (mapSize * mapSize != charCount)
            {
                Debug.Log("Uneven field.");
                return null;
            }

            // проверка максимального номера из ввода
            if (maxCharNumber + 1 > charCount)
            {
                Debug.Log("Incorrect Input.");
                return null;    
            }

            char[][] finalLevel = new char[mapSize][];
            for (int i = 0; i < mapSize; i++) finalLevel[i] = new char[mapSize];

            // заполнение результирующего массива и проверки: на заполнение каждой клетки и на возможность выполнения уровня
            foreach (var word in idxLevel)
            {
                finalLevel[word[1] / mapSize][word[1] % mapSize] = words[word[0]][0];
                for (int j = 2; j < word.Count; j++)
                {
                    var dist = Math.Abs(word[j] - word[j - 1]);
                    if ((dist == 1 || dist == mapSize) && finalLevel[word[j] / mapSize][word[j] % mapSize] == '\0')
                    {
                        finalLevel[word[j] / mapSize][word[j] % mapSize] = words[word[0]][j - 1];
                    }
                    else
                    {
                        Debug.Log("Impossible level.");
                        return null;
                    }
                }
            }

            return finalLevel;
        }
    }
}