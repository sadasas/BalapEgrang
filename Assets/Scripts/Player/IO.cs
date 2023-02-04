using System;
using System.IO;
using UnityEngine;

namespace Player
{
    public class IO
    {
        readonly string m_path = Application.persistentDataPath + "/DataPlayer.json";
        readonly object m_persistanDataLock = new();
        FileStream m_stream;

        public void SaveData(PlayerData data)
        {
            try
            {
                lock (m_persistanDataLock)
                {
                    using (m_stream = new(m_path, FileMode.Create))
                    {
                        using var sWriter = new StreamWriter(m_stream);
                        string json = JsonUtility.ToJson(data);
                        sWriter.Write(json);
                    }

                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public PlayerData LoadData()
        {
            PlayerData data = default;
            try
            {
                if (File.Exists(m_path))
                {
                    lock (m_persistanDataLock)
                    {
                        using (m_stream = new(m_path, FileMode.Open))
                        {
                            using var rReader = new StreamReader(m_stream);
                            string json = rReader.ReadToEnd();
                            data = JsonUtility.FromJson<PlayerData>(json);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return data;
        }
    }
}

