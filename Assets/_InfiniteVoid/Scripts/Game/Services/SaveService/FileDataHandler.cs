using System;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Services
{
    public class FileDataHandler<T>
    {
        private string _dataDirPath = "";
        private string _dataFileName = "";
        private bool _useEncryption = false;

        private const string _encryptionCodeWord = "word";
        private const string _backupExtension = ".bak";

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _useEncryption = useEncryption;
        }

        public bool CheckIfSaveFileExist()
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);

            return File.Exists(fullPath);
        }

        public void Delete()
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);

            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                else
                {
                    Debug.Log("Tried to delete data, but data was not found at path: " + fullPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to delete data at path: " + fullPath + "\n" + e);
            }
        }

        public async UniTaskVoid SaveAsync(T saveData, Action onEnd = null)
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);
            string backupFilePath = fullPath + _backupExtension;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(saveData, true);

                if (_useEncryption)
                {
                    dataToStore = await EncryptDecryptAsync(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }

                T verifiedSaveData = await LoadAsync();

                if (verifiedSaveData != null)
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

            onEnd?.Invoke();
        }

        public async UniTask<T> LoadAsync(bool allowRestoreFromBackup = true)
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);

            T loadedData = default(T);

            if (File.Exists(fullPath))
            {
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
                        dataToLoad = await EncryptDecryptAsync(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    if (allowRestoreFromBackup)
                    {
                        Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);

                        if (AttemptRollback(fullPath))
                        {
                            loadedData = await LoadAsync(false);
                        }
                    }
                    else
                    {
                        Debug.LogError("Error occured when trying to load data at path: " + fullPath + " and backup dod not work.\n" + e);
                    }
                }
            }

            return loadedData;
        }

        private async UniTask<string> EncryptDecryptAsync(string data)
        {
            var sb = new StringBuilder(data.Length);
            int blockSize = 1024;

            for (int i = 0; i < data.Length; i++)
            {
                char modifiedChar = (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
                sb.Append(modifiedChar);

                if ((i + 1) % blockSize == 0 || i == data.Length - 1)
                {
                    await UniTask.Yield();
                }
            }

            return sb.ToString();
        }

        private bool AttemptRollback(string fullPath)
        {
            bool success = false;

            string backupFilePath = fullPath + _backupExtension;

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
                Debug.LogError("Error occured when attempting to roll back to backup file at: " + backupFilePath + "\n" + e);
            }

            return success;
        }
    }
}
