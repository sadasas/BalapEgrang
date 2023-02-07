using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Utility;

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

                        var temp = data;
                        List<string> sc = new();
                        foreach (var item in data.CharacterCollections)
                        {
                            sc.Add(item.Name);

                        }
                        temp.CharacterNames = sc.ToArray();
                        temp.CharacterCollections = null;
                        string json = JsonUtility.ToJson(temp);
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

                            if (data.CharacterNames != null)
                            {
                                data.CharacterCollections = new();
                                foreach (var item in data.CharacterNames)
                                {
                                    var cr = Helper.GetPlayerType(item);

                                    data.CharacterCollections.Add(cr);

                                }

                                data.CharacterNames = null;

                            }
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

