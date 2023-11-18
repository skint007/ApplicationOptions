using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ApplicationOptions
{
    /// <summary>
    /// Non-Generic BaseOptionSettings that contains static properties
    /// </summary>
    public static class BaseOptionSettings
    {
        /// <summary>
        /// List of all loaded options
        /// </summary>
        public static List<IBaseOptions> LoadedOptions { get; } = new();

        /// <summary>
        /// If true, the options will be reloaded when the file changes
        /// </summary>
        public static bool WatchForOptionChanges { get; set; }

        /// <summary>
        /// If true, the json will be indented when saved
        /// </summary>
        public static bool IndentJson { get; set; } = true;
    }

    public partial class BaseOptions<T> : IDisposable, IBaseOptions where T : BaseOptions<T>, new()
    {
        /// <summary>
        /// The instance of the options
        /// </summary>
        protected static T? instance;
        /// <summary>
        /// The name of the file to save the options as
        /// </summary>
        protected virtual string FileName => $"options.{typeof(T).Name}.json";
        /// <summary>
        /// The folder to save the options in
        /// </summary>
        protected virtual string BaseFolder => "";
        /// <summary>
        /// The full path to the options file
        /// </summary>
        protected virtual string FilePath => Path.Combine(BaseFolder, FileName);

        private FileSystemWatcher _watcher;
        private bool _watcherDisposed;
        private static readonly object _instanceLock = new();
        private DateTime? _lastWrite;

        /// <summary>
        /// Invoked any time the config is loaded
        /// </summary>
        public event OptionEventHandler? Loaded;

        /// <summary>
        /// Invoked any time the config is saved
        /// </summary>
        public event OptionEventHandler? Saved;
        public delegate void OptionEventHandler();

        /// <summary>
        /// Instance of the options
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_instanceLock)
                    {
                        instance = new T();
                        instance.InitializeWatcher();
                        instance.Load();
                    }
                }
                return instance;
            }
        }

        protected BaseOptions() { }

        /// <summary>
        /// Sets the values of the options from the given options
        /// </summary>
        /// <remarks>
        /// This prevents accidentally disconnecting any event handlers, which happens when you replace the instance.
        /// </remarks>
        /// <param name="options"></param>
        public virtual void Set(T options)
        {
            //Nothing
            var flags = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance;
            foreach (var prop in options.GetType().GetProperties(flags))
            {
                if (!prop.CanWrite) continue;
                GetType().GetProperty(prop.Name)!.SetValue(this, prop.GetValue(options));
            }
        }

        /// <summary>
        /// Saves the options to the file
        /// </summary>
        public virtual void Save()
        {
            lock (_instanceLock)
            {
                SetWatcherStatus(false);
                if (!string.IsNullOrEmpty(BaseFolder))
                {
                    Directory.CreateDirectory(BaseFolder);
                }

                var serializedObject = JsonConvert.SerializeObject(Instance, BaseOptionSettings.IndentJson ? Formatting.Indented : Formatting.None);
                File.WriteAllText(FilePath, serializedObject);
                SetWatcherStatus(BaseOptionSettings.WatchForOptionChanges);
                Saved?.Invoke();
            }
        }

        /// <summary>
        /// Loads or reloads the options from the file.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if successful or no changes to load; 
        /// <see langword="false"/> if unable to load or deserialize the file.
        /// </returns>
        public virtual bool Load()
        {
            try
            {
                lock (_instanceLock)
                {
                    // Load option file
                    var file = new FileInfo(FilePath);
                    if (file.LastWriteTime == _lastWrite) return true;
                    var fileContent = File.ReadAllText(FilePath);
                    if (string.IsNullOrEmpty(fileContent)) return false;

                    // Parse json string
                    var deserializedObject = JsonConvert.DeserializeObject<T>(fileContent);
                    if (deserializedObject is null) return false;

                    // Replace our properties with the new ones
                    Set(deserializedObject);
                    _lastWrite = file.LastWriteTime;
                    Loaded?.Invoke();
                }
                return true;
            }
            catch
            {
                // This is most likely a non existing config file
                return false;
            }
            finally
            {
                if (!BaseOptionSettings.LoadedOptions.Any(x => x.GetType() == typeof(T)))
                {
                    BaseOptionSettings.LoadedOptions.Add(Instance);
                }
            }
        }

        /// <summary>
        /// Initializes the file watcher
        /// </summary>
        private void InitializeWatcher()
        {
            var file = new FileInfo(FilePath);
            file.Directory!.Create();
            _watcher = new FileSystemWatcher(file.Directory.FullName, FileName)
            {
                EnableRaisingEvents = BaseOptionSettings.WatchForOptionChanges,
                NotifyFilter = NotifyFilters.LastWrite
            };
            _watcher.Changed += async (s, e) =>
            {
                await Task.Delay(500);
                //Reload the config
                Load();
            };
        }

        /// <summary>
        /// Sets the status of the file watcher
        /// </summary>
        /// <param name="value"></param>
        private void SetWatcherStatus(bool value)
        {
            if (_watcherDisposed) return;
            _watcher.EnableRaisingEvents = value;
        }

        /// <summary>
        /// Disposes the file watcher
        /// </summary>
        private void DisposeWatcher()
        {
            if (_watcherDisposed) return;
            _watcherDisposed = true;
            _watcher.Dispose();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            DisposeWatcher();
        }
    }
}
