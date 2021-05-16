using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DungeonCrawlerGame.Classes
{
    /// <summary>
    /// Any property decorated with this attribute will be subject to inclusion in the json file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SaveAttribute : Attribute
    {
    }

    /// <summary>
    /// Provides persistent storage functionality.
    /// </summary>
    public class JsonStorage
    {
        /// <summary>
        /// Load the JSON file into the target object.
        /// </summary>
        public virtual void Load<T>(string filename, T obj)
            where T : class
        {
            using (TextReader reader = new StreamReader(filename))
            {
                var json = reader.ReadToEnd();
                var jsonObj = JObject.Parse(json);
                var deserialized = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
                {
                    ContractResolver = new JsonStorageResolver()
                });

                foreach (var property in typeof(T).GetProperties())
                {
                    if (jsonObj.Properties().ToList().Any(a => a.Name == property.Name))
                        property.SetValue(obj, property.GetValue(deserialized, null), null);
                }
            }
        }

        /// <summary>
        /// Save the class to a JSON file.
        /// </summary>
        public virtual void Save<T>(string filename, T obj)
            where T : class
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new JsonStorageResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });

            using (TextWriter writer = new StreamWriter(filename))
            {
                writer.Write(json);
            }
        }

        /// <summary>
        /// Loads the JSON file and returns the property's value
        /// </summary>
        public virtual T GetSavedPropertyValue<T>(string filename, string propertyName)
        {
            using (TextReader reader = new StreamReader(filename))
            {
                var json = reader.ReadToEnd();
                var jsonObj = JObject.Parse(json);
                var v = jsonObj.Property(propertyName).Value.ToObject<T>();
                return v;
            }
        }

        /// <summary>
        /// Check if the property is contained within the save file.
        /// </summary>
        public virtual bool IsPropertySaved(string filename, string propertyName)
        {
            using (TextReader reader = new StreamReader(filename))
            {
                var json = reader.ReadToEnd();
                var jsonObj = JObject.Parse(json);
                return jsonObj.Property(propertyName) != null;
            }
        }

        /// <summary>
        /// Json contract resolver which loads only properties decorated with <see cref="SaveAttribute"/>.
        /// </summary>
        protected class JsonStorageResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);

                var property = member.DeclaringType.GetRuntimeProperty(member.Name);
                if (property == null || property.GetCustomAttribute<SaveAttribute>() == null)
                    prop.Ignored = true;

                if (property.GetSetMethod(true) != null)
                    prop.Writable = true;

                return prop;
            }
        }
    }

    /// <summary>
    /// Local storage provider which saves files in a specific directory.
    /// </summary>
    public class LocalStorage : JsonStorage
    {
        public string SaveDirectory { get; private set; }

        public LocalStorage(string folderName)
        {
            SaveDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), folderName);

            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);
        }

        public bool StorageExists(string filename) => File.Exists(Path.Combine(SaveDirectory, filename));
        public override void Load<T>(string filename, T obj) => base.Load(Path.Combine(SaveDirectory, filename), obj);
        public override void Save<T>(string filename, T obj) => base.Save(Path.Combine(SaveDirectory, filename), obj);
        public override T GetSavedPropertyValue<T>(string filename, string propertyName) => base.GetSavedPropertyValue<T>(Path.Combine(SaveDirectory, filename), propertyName);
        public override bool IsPropertySaved(string filename, string propertyName) => base.IsPropertySaved(Path.Combine(SaveDirectory, filename), propertyName);
    }
}
