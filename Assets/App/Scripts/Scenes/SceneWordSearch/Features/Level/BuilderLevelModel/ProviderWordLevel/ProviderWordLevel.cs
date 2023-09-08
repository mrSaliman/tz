using System;
using System.IO;
using System.Linq;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using Assets.App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            TextAsset file = Resources.Load("WordSearch/Levels/" + levelIndex) as TextAsset;
            string jsonData = file.ToString();
            Rootobject words = Rootobject.CreateFromJSON(jsonData);
            LevelInfo level = new()
            {
                words = words.words.ToList<string>()
            };
            return level;
        }
    }
}