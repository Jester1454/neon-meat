using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DataPersistence
{
    public class FileDataHandler
    {
        private readonly string _dataDirPath = "";
        private readonly string _dataFileName = "";
        private readonly bool _useEncryption = false;
        private readonly string encryptionCodeWord = "word";
        private readonly string backupExtension = ".bak";

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption) 
        {
            this._dataDirPath = dataDirPath;
            this._dataFileName = dataFileName;
            this._useEncryption = useEncryption;
        }

        public bool TryLoad<T>(string profileId, out T result, bool allowRestoreFromBackup = true)
        {
            result = Load<T>(profileId, allowRestoreFromBackup);

            return result != null;
        }

        public T Load<T>(string profileId, bool allowRestoreFromBackup = true) 
        {
            if (profileId == null) 
            {
                return default(T);
            }

            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            var loadedData = default(T);
            if (!File.Exists(fullPath)) return loadedData;
            try 
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (_useEncryption) 
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<T>(dataToLoad);
            }
            catch (Exception e) 
            {
                if (allowRestoreFromBackup) 
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        loadedData = Load<T>(profileId, false);
                    }
                }
                else 
                {
                    Debug.LogError("Error occured when trying to load file at path: " 
                                   + fullPath  + " and backup did not work.\n" + e);
                }
            }
            return loadedData;
        }

        public void Save<T>(T data, string profileId) 
        {
            if (profileId == null) 
            {
                return;
            }

            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            string backupFilePath = fullPath + backupExtension;
            try 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(data, true);

                if (_useEncryption) 
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream)) 
                    {
                        writer.Write(dataToStore);
                    }
                }

                T verifiedGameData = Load<T>(profileId);
                if (verifiedGameData != null) 
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
                else 
                {
                    throw new Exception("Save file could not be verified and backup could not be created.");
                }

            }
            catch (Exception e) 
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        public void Delete(string profileId) 
        {
            if (profileId == null) 
            {
                return;
            }

            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            try 
            {
                if (File.Exists(fullPath)) 
                {
                    Directory.Delete(Path.GetDirectoryName(fullPath), true);
                }
                else 
                {
                    Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
                }
            }
            catch (Exception e) 
            {
                Debug.LogError("Failed to delete profile data for profileId: " 
                               + profileId + " at path: " + fullPath + "\n" + e);
            }
        }

        public Dictionary<string, T> LoadAllProfiles<T>() 
        {
            var profileDictionary = new Dictionary<string, T>();

            var dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            foreach (DirectoryInfo dirInfo in dirInfos) 
            {
                string profileId = dirInfo.Name;

                string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: "
                                     + profileId);
                    continue;
                }

                T profileData = Load<T>(profileId);
                if (profileData != null) 
                {
                    profileDictionary.Add(profileId, profileData);
                }
                else 
                {
                    Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
                }
            }

            return profileDictionary;
        }
    
        private string EncryptDecrypt(string data) 
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++) 
            {
                modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }
            return modifiedData;
        }

        private bool AttemptRollback(string fullPath) 
        {
            bool success = false;
            string backupFilePath = fullPath + backupExtension;
            try 
            {
                if (File.Exists(backupFilePath))
                {
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
                }
                else 
                {
                    throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
                }
            }
            catch (Exception e) 
            {
                Debug.LogError("Error occured when trying to roll back to backup file at: " 
                               + backupFilePath + "\n" + e);
            }

            return success;
        }
    }
}