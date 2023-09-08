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
        private static readonly TextAsset file1 = Resources.Load("Fillwords/pack_0") as TextAsset;
        private static readonly TextAsset file2 = Resources.Load("Fillwords/words_list") as TextAsset;

        private static readonly string[] input = file1.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        private static readonly string[] words = file2.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        public GridFillWords LoadModel(int index)
        {
            char[][] level;
            do
            {
                Debug.Log("Loading level " + index);
                level = GetLevel(index);
                index++;
            }
            while (index <= input.Length && level == null);
            if (level == null)
            {
                throw new();
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

            var charNumber = 0;
            var maxCharNumber = 0;
            List<List<int>> idxLevel = new();
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
                        charNumber += chars.Length;
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

            var mapSize = (int)Math.Floor(Math.Sqrt(charNumber));
            if (mapSize * mapSize != charNumber)
            {
                Debug.Log("Uneven field.");
                return null;
            }

            if (maxCharNumber + 1 > charNumber)
            {
                Debug.Log("Incorrect Input.");
                return null;    
            }

            char[][] finalLevel = new char[mapSize][];
            for (int i = 0; i < mapSize; i++) finalLevel[i] = new char[mapSize];

            for (int i = 0; i < idxLevel.Count; i++)
            {
                finalLevel[idxLevel[i][1] / mapSize][idxLevel[i][1] % mapSize] = words[idxLevel[i][0]][0];
                for (int j = 2; j < idxLevel[i].Count; j++)
                {
                    var dist = Math.Abs(idxLevel[i][j] - idxLevel[i][j - 1]);
                    if ((dist == 1 || dist == mapSize) && finalLevel[idxLevel[i][j] / mapSize][idxLevel[i][j] % mapSize] == '\0')
                    {
                        finalLevel[idxLevel[i][j] / mapSize][idxLevel[i][j] % mapSize] = words[idxLevel[i][0]][j - 1];
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