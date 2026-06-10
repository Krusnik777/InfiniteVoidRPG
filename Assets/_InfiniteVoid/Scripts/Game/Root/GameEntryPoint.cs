using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using R3;
using DI;
using InfiniteVoidRPG.Utils;
using InfiniteVoidRPG.Game.EntryPoints;
using InfiniteVoidRPG.Game.Services;

namespace InfiniteVoidRPG.Game.Root
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        private Coroutines _coroutines;
        private UIRootView _uiRoot;

        private readonly DIContainer _rootContainer = new();
        private DIContainer _cachedSceneContainer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            //Application.targetFrameRate = 60;
            //QualitySettings.vSyncCount = 1;
            //Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _instance = new GameEntryPoint();
            _instance.RunGame();
        }

        private GameEntryPoint()
        {
            Localization.LocalizationSystem.CreateInstance();

            _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);

            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            _uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRoot.gameObject);
            _rootContainer.RegisterInstance(_uiRoot);

            SetupAudioService();
            SetupInputServices();
            SetupStoryEventsServices();
            SetupDataProviders();
            SetupApplicationControlService();
        }

        private async void RunGame()
        {
            var applicationSettingsProvider = _rootContainer.Resolve<ApplicationSettingsProvider>();

            // UI Show "Loading Application Data..."

            var applicationSettingsData = await applicationSettingsProvider.LoadData();

            if (applicationSettingsData == null)
            {
                // UI Show "No Created Application Data Finded...";
                // UI Show "Creating Application Data...";

                applicationSettingsData = await applicationSettingsProvider.CreateData();

                // UI Show "Application Data Created..."; ?

                // Play Settings Setup - need await
            }

            // Apply Settings
            _rootContainer.Resolve<ApplicationControlService>().Initialize();

            #if UNITY_EDITOR

            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.GAMEPLAY)
            {
                //var enterParams = new GameplayEnterParams(0);
                _coroutines.StartCoroutine(LoadAndStartGameplay(/*enterParams*/));

                return;
            }

            if (sceneName == Scenes.HUB)
            {
                _coroutines.StartCoroutine(LoadAndStartHub());

                return;
            }

            if (sceneName != Scenes.BOOTSTRAP)
            {
                return;
            }

            #endif

            _coroutines.StartCoroutine(LoadAndStartHub());
            //_coroutines.StartCoroutine(LoadAndStartGameplay());
        }

        private IEnumerator LoadAndStartGameplay(/*GameplayEnterParams enterParams*/)
        {
            _uiRoot.ShowLoadingScreen();
            _cachedSceneContainer?.Dispose();

            yield return LoadScene(Scenes.BOOTSTRAP);
            yield return LoadScene(Scenes.GAMEPLAY);

            yield return new WaitForSeconds(1);

            var sceneEntryPoint = Object.FindFirstObjectByType<EntryPoint>();
            var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(sceneContainer).Subscribe(exitTag =>
            {
                #if PLATFORM_STANDALONE_WIN && !UNITY_EDITOR
                if (exitTag == "FINISH")
                {
                    Application.Quit();
                    return;
                }
                #endif

                if (exitTag == Scenes.HUB) _coroutines.StartCoroutine(LoadAndStartHub());
            });

            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartHub(/*HubEnterParams enterParams = null*/)
        {
            _uiRoot.ShowLoadingScreen();
            _cachedSceneContainer?.Dispose();

            yield return LoadScene(Scenes.BOOTSTRAP);
            yield return LoadScene(Scenes.HUB);

            yield return new WaitForSeconds(1);

            var sceneEntryPoint = Object.FindFirstObjectByType<EntryPoint>();
            var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(sceneContainer).Subscribe(exitTag =>
            {
                #if PLATFORM_STANDALONE_WIN && !UNITY_EDITOR
                if (exitTag == "FINISH")
                {
                    Application.Quit();
                    return;
                }
                #endif

                if (exitTag == Scenes.GAMEPLAY) _coroutines.StartCoroutine(LoadAndStartGameplay());
                //else Application.Quit();
            });

            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }

        #region Services Setup Methods

        private void SetupAudioService()
        {
            var mixer = Resources.Load<AudioMixer>("AudioMixer");
            var sfxGroup = mixer.FindMatchingGroups("SFX")[0];
            var bgmGroup = mixer.FindMatchingGroups("BGM")[0];

            var audioSystemContainer = new GameObject("[AUDIO]").AddComponent<AudioListener>();

            var soundsContainer = new GameObject("[SOUNDS]").AddComponent<AudioSource>();
            soundsContainer.outputAudioMixerGroup = sfxGroup;
            soundsContainer.transform.SetParent(audioSystemContainer.transform);
            //var loopSoundsContainer = new GameObject("[SOUNDS_LOOP]").AddComponent<AudioSource>();
            //loopSoundsContainer.outputAudioMixerGroup = sfxGroup;
            //loopSoundsContainer.transform.SetParent(audioSystemContainer.transform);

            AudioSource bgmContainer = new GameObject("[BACKGROUND_MUSIC]").AddComponent<AudioSource>();
            bgmContainer.outputAudioMixerGroup = bgmGroup;
            bgmContainer.transform.SetParent(audioSystemContainer.transform);

            Object.DontDestroyOnLoad(audioSystemContainer);

            // AudioService init
        }

        private void SetupInputServices()
        {
            var inputDeviceDetectService = new InputDeviceDetectService();
            _rootContainer.RegisterInstance(inputDeviceDetectService);

            var gameInputService = new GameInputService();
            _rootContainer.RegisterInstance(gameInputService);
        }

        private void SetupStoryEventsServices()
        {
            var storyEventsProvider = new StoryEventsProvider();
            _rootContainer.RegisterInstance(storyEventsProvider);

            var storyEventsController = new StoryEventsController(_rootContainer.Resolve<GameInputService>());
            _rootContainer.RegisterInstance(storyEventsController);
        }

        private void SetupDataProviders()
        {
            var applicationSettingsProvider = new ApplicationSettingsProvider();
            _rootContainer.RegisterInstance(applicationSettingsProvider);

            var gameDataProvider = new GameDataProvider();
            _rootContainer.RegisterInstance(gameDataProvider);
        }

        private void SetupApplicationControlService()
        {
            var audioMixer = Resources.Load<AudioMixer>("AudioMixer");
            var applicationSettingsProvider = _rootContainer.Resolve<ApplicationSettingsProvider>();

            var graphicsController = new Settings.GraphicsController(applicationSettingsProvider.DefaultApplicationSettings);
            var audioMixerController = new Settings.AudioMixerController(audioMixer, applicationSettingsProvider.DefaultApplicationSettings);
            var applicationControlService = new ApplicationControlService(applicationSettingsProvider, graphicsController, audioMixerController);
            _rootContainer.RegisterInstance(applicationControlService);
        }

        #endregion
    }
}
