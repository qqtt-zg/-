using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 日志加密工具类，用于对日志中的敏感信息进行加密和解密
    /// </summary>
    public class LogEncryptor
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private const int _keySize = 256;
        private const int _blockSize = 128;
        private const string _defaultEncryptionKey = "87b94e5f-0a3d-41c2-b7bc-f18d6c4a9d82";

        /// <summary>
        /// 默认的加密范围标记
        /// </summary>
        public const string EncryptTagStart = "[ENCRYPTED]";
        public const string EncryptTagEnd = "[/ENCRYPTED]";

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogEncryptor() : this(_defaultEncryptionKey) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptionKey">加密密钥</param>
        public LogEncryptor(string encryptionKey)
        {
            if (string.IsNullOrEmpty(encryptionKey))
                throw new ArgumentNullException(nameof(encryptionKey));

            using (var sha256 = SHA256.Create())
            {
                _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));
            }

            // 使用密钥的前16字节作为IV
            _iv = new byte[_blockSize / 8];
            Array.Copy(_key, 0, _iv, 0, _iv.Length);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">要加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.KeySize = _keySize;
                    aesAlg.BlockSize = _blockSize;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.Key = _key;
                    aesAlg.IV = _iv;

                    // 创建加密器
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // 创建内存流
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        // 创建加密流
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            // 创建写入流
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                // 写入要加密的数据
                                swEncrypt.Write(plainText);
                            }

                            // 获取加密后的数据
                            byte[] encrypted = msEncrypt.ToArray();
                            
                            // 转换为Base64字符串
                            return Convert.ToBase64String(encrypted);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 加密失败时，返回原始文本（这是一个权衡，确保日志可用性）
                Console.WriteLine("加密失败: " + ex.Message);
                return plainText;
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="cipherText">要解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.KeySize = _keySize;
                    aesAlg.BlockSize = _blockSize;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.Key = _key;
                    aesAlg.IV = _iv;

                    // 创建解密器
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // 将Base64字符串转换为字节数组
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);

                    // 创建内存流
                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    {
                        // 创建解密流
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            // 创建读取流
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // 读取解密后的数据
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 解密失败时，返回原始密文
                Console.WriteLine("解密失败: " + ex.Message);
                return cipherText;
            }
        }

        /// <summary>
        /// 加密日志消息中的敏感信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="encryptSensitiveInfo">是否加密敏感信息</param>
        /// <returns>处理后的日志消息</returns>
        public string ProcessSensitiveInfo(string message, bool encryptSensitiveInfo = true)
        {
            if (!encryptSensitiveInfo || string.IsNullOrEmpty(message))
                return message;

            try
            {
                // 这里可以添加需要加密的敏感信息模式识别和处理
                // 例如：密码、身份证号、手机号等
                
                // 示例：加密密码字段
                message = ProcessPattern(message, "password=[^\"]*", match =>
                {
                    if (match != null && match.Contains("="))
                    {
                        string prefix = match.Substring(0, match.IndexOf('=') + 1);
                        string password = match.Substring(match.IndexOf('=') + 1);
                        return prefix + EncryptTagStart + Encrypt(password) + EncryptTagEnd;
                    }
                    return match;
                });

                // 示例：加密身份证号
                message = ProcessPattern(message, @"\d{17}[\dXx]", match =>
                {
                    return EncryptTagStart + Encrypt(match) + EncryptTagEnd;
                });

                // 示例：加密手机号
                message = ProcessPattern(message, @"1[3-9]\d{9}", match =>
                {
                    return EncryptTagStart + Encrypt(match) + EncryptTagEnd;
                });

                return message;
            }
            catch (Exception ex)
            {
                // 处理失败时，返回原始消息
                Console.WriteLine("处理敏感信息失败: " + ex.Message);
                return message;
            }
        }

        /// <summary>
        /// 处理字符串中的特定模式
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">正则表达式模式</param>
        /// <param name="processor">处理器函数</param>
        /// <returns>处理后的字符串</returns>
        private string ProcessPattern(string input, string pattern, Func<string, string> processor)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
                return regex.Replace(input, match => processor(match.Value));
            }
            catch (Exception)
            {
                // 正则表达式处理失败时，返回原始输入
                return input;
            }
        }

        /// <summary>
        /// 从日志消息中解密敏感信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <returns>解密后的日志消息</returns>
        public string DecryptSensitiveInfo(string message)
        {
            if (string.IsNullOrEmpty(message) || !message.Contains(EncryptTagStart) || !message.Contains(EncryptTagEnd))
                return message;

            try
            {
                int startIndex = message.IndexOf(EncryptTagStart);
                int endIndex = message.IndexOf(EncryptTagEnd);

                while (startIndex >= 0 && endIndex > startIndex)
                {
                    string encryptedContent = message.Substring(startIndex + EncryptTagStart.Length, endIndex - (startIndex + EncryptTagStart.Length));
                    string decryptedContent = Decrypt(encryptedContent);
                    
                    message = message.Substring(0, startIndex) + decryptedContent + message.Substring(endIndex + EncryptTagEnd.Length);
                    
                    // 查找下一个加密部分
                    startIndex = message.IndexOf(EncryptTagStart);
                    endIndex = message.IndexOf(EncryptTagEnd);
                }

                return message;
            }
            catch (Exception ex)
            {
                // 解密失败时，返回原始消息
                Console.WriteLine("解密敏感信息失败: " + ex.Message);
                return message;
            }
        }

        /// <summary>
        /// 检查字符串是否包含加密标记
        /// </summary>
        /// <param name="message">要检查的字符串</param>
        /// <returns>是否包含加密标记</returns>
        public bool ContainsEncryptedContent(string message)
        {
            return !string.IsNullOrEmpty(message) && message.Contains(EncryptTagStart) && message.Contains(EncryptTagEnd);
        }
    }
}