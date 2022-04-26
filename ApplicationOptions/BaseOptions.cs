using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ApplicationOptions
{
    /// <summary>
    /// Non-Generic BaseOptions that contains static properties
    /// </summary>
    public static class BaseOptions
    {
        /// <summary>
        /// List of all loaded options
        /// </summary>
        public static List<IBaseOptions> LoadedOptions { get; } = new List<IBaseOptions>();
    }

    public partial class BaseOptions<T> : IBaseOptions where T : BaseOptions<T>, new()
    {
        #region Protected Members/Properties
        protected static T instance;
        protected virtual string FileName => $"options.{typeof(T).Name}.json";
        protected virtual string BaseFolder => @"options\";
        protected bool ChangesMade { get; set; }
        #endregion

        #region Public/Virtual properties
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    instance.Load();
                }
                return instance;
            }
        }
                #endregion

        #region Protected/Virtual methods
        protected BaseOptions() { }
        #endregion

        #region Public/Virtual methods
        public virtual void Set(T options)
        {
            //Nothing
            foreach(var prop in options.GetType().GetProperties())
            {
                GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(options));
            }
        }

        public virtual string GetFileName()
        {
            return BaseFolder + FileName;
        }

        public virtual void Save()
        {
            Directory.CreateDirectory(BaseFolder);

            var serializedObject = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(GetFileName(), serializedObject);
        }

        public virtual bool Load()
        {
            try
            {
                var obj = File.ReadAllText(GetFileName());
                var deserializedObject = JsonConvert.DeserializeObject<T>(obj);

                //Use the provided options object, or the deserialized obj if options is null
                instance = deserializedObject ?? (T)this;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (BaseOptions.LoadedOptions.Count(x => x.GetType() == typeof(T)) == 0)
                {
                    BaseOptions.LoadedOptions.Add(instance);
                }
            }
        }
        #endregion
    }
}
