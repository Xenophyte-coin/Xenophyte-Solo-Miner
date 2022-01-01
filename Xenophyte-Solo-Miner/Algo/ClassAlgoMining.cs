﻿using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xenophyte_Solo_Miner.Utility;


namespace Xenophyte_Solo_Miner.Algo
{


    public class ClassAlgoMining
    {
        public static ICryptoTransform[] CryptoTransformMining;

        public static AesManaged[] AesManagedMining;

        public static SHA512Managed[] Sha512ManagedMining;

        public static MemoryStream[] MemoryStreamMining;

        public static CryptoStream[] CryptoStreamMining;

        public static int[] TotalNonceMining;


        /// <summary>
        /// Encrypt the math calculation generated for the Exact Share Mining System.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="idThread"></param>
        /// <param name="useNextHexOption"></param>
        /// <returns></returns>
        public static string EncryptAesShare(string text, int idThread, bool useNextHexOption)
        {
            if (MemoryStreamMining[idThread] == null)
            {
                MemoryStreamMining[idThread] = new MemoryStream();
            }

            if (CryptoStreamMining[idThread] == null)
            {
                CryptoStreamMining[idThread] = new CryptoStream(MemoryStreamMining[idThread], CryptoTransformMining[idThread], CryptoStreamMode.Write);
            }

            #region Do mining work

            var textBytes = Encoding.UTF8.GetBytes(text);
            CryptoStreamMining[idThread].Write(textBytes, 0, textBytes.Length);

            #endregion

            #region Flush mining work process

            if (!CryptoStreamMining[idThread].HasFlushedFinalBlock)
            {
                CryptoStreamMining[idThread].FlushFinalBlock();
                CryptoStreamMining[idThread].Flush();
            }


            #endregion

            #region Translate Mining work

            string result = ClassUtility.GetHexStringFromByteArray(MemoryStreamMining[idThread].ToArray(), useNextHexOption);
            
            #endregion

            #region Cleanup work
            CryptoStreamMining[idThread] = new CryptoStream(MemoryStreamMining[idThread], CryptoTransformMining[idThread], CryptoStreamMode.Write);
            MemoryStreamMining[idThread].SetLength(0);
            Array.Clear(textBytes, 0, textBytes.Length);
            #endregion

            return result;
        }


        /// <summary>
        /// Encrypt share with XOR.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptXorShare(string text, string key)
        {
            char[] resultXor = new char[text.Length];

            int keyLength = key.Length;

            for (int c = 0; c < text.Length; c++)
            {
                resultXor[c] = (char)(text[c] ^ (uint)key[c % keyLength]);
            }
            return new string(resultXor, 0, resultXor.Length);
        }

        /// <summary>
        /// Generate a sha512 hash
        /// </summary>
        /// <param name="input"></param>
        /// <param name="idThread"></param>
        /// <returns></returns>
        public static string GenerateSha512FromString(string input, int idThread)
        {
            if (Sha512ManagedMining[idThread] == null)
            {
                Sha512ManagedMining[idThread] = new SHA512Managed();
            }

            return ClassUtility.GetHexStringFromByteArray2(Sha512ManagedMining[idThread].ComputeHash(Encoding.UTF8.GetBytes(input)));

        }

    }
}