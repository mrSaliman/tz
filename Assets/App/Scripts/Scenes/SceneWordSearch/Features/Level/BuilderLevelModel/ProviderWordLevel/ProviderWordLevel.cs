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
            var file = Resources.Load($"WordSearch/Levels/{levelIndex}") as TextAsset;
            var words = Rootobject.CreateFromJSON(file.ToString()).words;
            return new LevelInfo { words = words.ToList() };
        }
    }
}