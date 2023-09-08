using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    [Serializable]
    public class Rootobject
    {
        public string[] words;

        public static Rootobject CreateFromJSON(string jsonData)
        {
            return JsonUtility.FromJson<Rootobject>(jsonData);
        }
    }

}
