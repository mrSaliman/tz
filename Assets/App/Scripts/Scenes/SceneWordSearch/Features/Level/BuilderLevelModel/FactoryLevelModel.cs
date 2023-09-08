using System;
using System.Collections.Generic;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            List<char> result = new();
            Dictionary<char, int> maxCharNumber = new(), tempCharNumber;
            foreach (var word in words)
            {
                tempCharNumber = new();
                foreach (var letter in word)
                {
                    if (!tempCharNumber.ContainsKey(letter)) tempCharNumber[letter] = 0;
                    tempCharNumber[letter]++;
                }
                foreach (var letter in tempCharNumber)
                {
                    if (!maxCharNumber.ContainsKey(letter.Key)) maxCharNumber[letter.Key] = 0;
                    maxCharNumber[letter.Key] = Math.Max(maxCharNumber[letter.Key], letter.Value);
                }
            }
            foreach (var letter in maxCharNumber)
            {
                for (int i = 0; i < letter.Value; i++) result.Add(letter.Key);
            }
            return result;
        }
    }
}