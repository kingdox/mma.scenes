#region Access
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion
namespace MMA.Scenes
{
    public static partial class Key
    {
        public static int AddScene = 0;
        public static int RemoveScene = 1;
        public static int OnSceneRemoved = 2;
        public static int OnSceneAdded = 3;
    }

    public sealed partial class Scenes_Module : Module
    {
        #region References
        //[Header("Applications")]
        //[SerializeField] public ApplicationBase interface_Scenes;
        #endregion
        #region Reactions ( On___ )
        // Contenedor de toda las reacciones del Scenes
        protected override void OnSubscription(bool condition)
        {
            //
            if (condition)
            {
                SceneManager.sceneLoaded += OnSceneAdded;
                SceneManager.sceneUnloaded += OnSceneRemoved;
            }
            else
            {
                SceneManager.sceneLoaded -= OnSceneAdded;
                SceneManager.sceneUnloaded -= OnSceneRemoved;
            }

            Middleware<string>.Subscribe_IEnumerator(condition, Key.AddScene,      Request_AddAsyncScene);
            Middleware<string[]>.Subscribe_IEnumerator(condition, Key.AddScene,      Request_AddAsyncScene);
            Middleware<string>.Subscribe_IEnumerator(condition, Key.RemoveScene,   Request_RemovesAsyncScene);
            Middleware<string[]>.Subscribe_IEnumerator(condition, Key.RemoveScene,   Request_RemovesAsyncScene);
        }
        private void OnSceneAdded(Scene scene, LoadSceneMode mode)
        {
            try
            {
                Middleware<Scene>.Invoke_Publish(Key.OnSceneAdded, scene);
            }
            catch
            {
                //$"ERROR { nameof(OnSceneAdded)}: {scene}, {mode}".Print("red");
            }
        }
        private void OnSceneRemoved(Scene scene)
        {
            try
            {
                Middleware<Scene>.Invoke_Publish(Key.OnSceneRemoved, scene);
            }
            catch
            {
                //$"ERROR {nameof(OnSceneRemoved)}: {scene.name}".Print("red");
            }
        }
        #endregion
        #region Methods
        // Contenedor de toda la logica del Scenes
        #endregion
        #region Request ( Coroutines )
        // Contenedor de toda la Esperas del Scenes
        private IEnumerator Request_AddAsyncScene(string name)
        {
            yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        }

        private IEnumerator Request_AddAsyncScene(string[] name)
        {
            for (int i = 0; i < name.Length; i++) yield return SceneManager.LoadSceneAsync(name[i], LoadSceneMode.Additive);
        }

        private IEnumerator Request_RemovesAsyncScene(string name)
        {
            yield return SceneManager.UnloadSceneAsync(name);
        }

        private IEnumerator Request_RemovesAsyncScene(string[] name)
        {
            for (int i = 0; i < name.Length; i++) yield return SceneManager.UnloadSceneAsync(name[i]);
        }
        #endregion
        #region Task ( async )
        // Contenedor de toda la Esperas del Scenes
        #endregion
    }
}
