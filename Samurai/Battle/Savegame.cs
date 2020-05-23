using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Numian
{
    public static class Savegame
    {
        public static string path = Path.Combine(Application.persistentDataPath, "save");
        
        public static void SaveDictionary(WordDictionary d)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, d);

            stream.Close();
        }

        public static WordDictionary LoadDictionary()
        {
            if(File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                WordDictionary dictionary = formatter.Deserialize(stream) as WordDictionary;

                stream.Close();

                return dictionary;
            }
            else
            {
                Debug.LogWarning("Save not found in: " + path);
                return new WordDictionary();
            }
        }

    }
}