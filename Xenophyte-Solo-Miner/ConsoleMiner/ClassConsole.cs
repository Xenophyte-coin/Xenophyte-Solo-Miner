﻿using System;
using System.Diagnostics;
using Xenophyte_Solo_Miner.Cache;
using Xenophyte_Solo_Miner.Mining;


namespace Xenophyte_Solo_Miner.ConsoleMiner
{
    public class ClassConsoleColorEnumeration
    {
        public const int ConsoleTextColorWhite = 0;
        public const int ConsoleTextColorGreen = 1;
        public const int ConsoleTextColorYellow = 2;
        public const int ConsoleTextColorRed = 3;
        public const int ConsoleTextColorMagenta = 4;
        public const int ConsoleTextColorBlue = 5;
        public const int ConsoleTextCyan = 6;
    }

    public class ClassConsoleKeyCommandEnumeration
    {
        public const string ConsoleCommandKeyHashrate = "h";
        public const string ConsoleCommandKeyDifficulty = "d";
        public const string ConsoleCommandKeyCache = "c";
        public const string ConsoleCommandKeyRange = "r";

    }

    public class ClassConsole
    {
        private static PerformanceCounter _ramCounter;

        /// <summary>
        ///     Replace WriteLine function with forecolor system.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="color"></param>
        public static void WriteLine(string log, int color = 0)
        {
            switch (color)
            {
                case ClassConsoleColorEnumeration.ConsoleTextColorWhite:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ClassConsoleColorEnumeration.ConsoleTextColorGreen:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ClassConsoleColorEnumeration.ConsoleTextColorYellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ClassConsoleColorEnumeration.ConsoleTextColorRed:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ClassConsoleColorEnumeration.ConsoleTextColorMagenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case ClassConsoleColorEnumeration.ConsoleTextColorBlue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case ClassConsoleColorEnumeration.ConsoleTextCyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break; ;
            }

            Console.WriteLine(DateTime.Now + " - " + log);
        }

        /// <summary>
        ///     Handle command line.
        /// </summary>
        /// <param name="command"></param>
        public static void CommandLine(string command)
        {
            switch (command.ToLower())
            {
                case ClassConsoleKeyCommandEnumeration.ConsoleCommandKeyHashrate:
                    if (Program.ClassMinerConfigObject.mining_show_calculation_speed)
                    {
                        WriteLine(ClassMiningStats.TotalHashrate + " H/s | " + ClassMiningStats.TotalCalculation + " C/s > ACCEPTED[" + ClassMiningStats.TotalBlockAccepted + "] REFUSED[" + ClassMiningStats.TotalBlockRefused + "]", ClassConsoleColorEnumeration.ConsoleTextColorMagenta);
                    }
                    else
                    {
                        WriteLine(ClassMiningStats.TotalHashrate + " H/s | ACCEPTED[" + ClassMiningStats.TotalBlockAccepted + "] REFUSED[" + ClassMiningStats.TotalBlockRefused + "]", ClassConsoleColorEnumeration.ConsoleTextColorMagenta);
                    }

                    break;
                case ClassConsoleKeyCommandEnumeration.ConsoleCommandKeyDifficulty:
                    WriteLine("Current Block ID: " + ClassMiningNetwork.CurrentBlockId + " | Difficulty: " + ClassMiningNetwork.CurrentBlockDifficulty);
                    break;
                case ClassConsoleKeyCommandEnumeration.ConsoleCommandKeyCache:
                    if (Program.ClassMinerConfigObject.mining_enable_cache)
                    {
                        var allocationInMb = Process.GetCurrentProcess().PrivateMemorySize64 / 1e+6;
                        float availbleRam;

                        if (Environment.OSVersion.Platform == PlatformID.Unix)
                        {
                            availbleRam = long.Parse(ClassMiningCache.RunCommandLineMemoryAvailable());
                        }
                        else
                        {
                            if (_ramCounter == null)
                            {
                                _ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
                            }

                            availbleRam = _ramCounter.NextValue();
                        }

                        WriteLine("Current math combinaisons cached: " +  ClassMining.DictionaryCacheMining.Count.ToString("F0") + " | RAM Used: " + allocationInMb + " MB(s) | RAM Available: "+availbleRam+" MB(s).");
                    }
                    else
                    {
                        WriteLine("Mining cache option is not enabled", ClassConsoleColorEnumeration.ConsoleTextColorYellow);
                    }

                    break;
                case ClassConsoleKeyCommandEnumeration.ConsoleCommandKeyRange:
                    WriteLine("Current Range: " + ClassMiningNetwork.CurrentBlockJob.Replace(ClassMiningBlockSplitEnumeration.MiningBlockJobSplit, "|"));
                    break;
            }
        }
    }
}