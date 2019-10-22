using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace src
{
    [RPlotExporter, RankColumn]
    [HtmlExporter]
    [MemoryDiagnoser]
    public class TextFileReading
    {
        private string fileToRead = Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\LoremIpsum.txt");
        private string wordToLookFor = "Facilisis";

        [GlobalSetup]
        public void Setup()
        {
        } 

        [Benchmark]
        public bool ReadUsingFileReadAllText()
        {
            string fileText = File.ReadAllText(fileToRead);
            return fileText.Contains(wordToLookFor);
        }

        [Benchmark]
        public bool ReadUsingFileReadAllLines()
        {
            string[] fileText = File.ReadAllLines(fileToRead);
            bool containsWord = false;

            for(int lineIndex = 0; lineIndex < fileText.Length; lineIndex++)
            {
                if(fileText[lineIndex].Contains(wordToLookFor))
                {
                    containsWord = true;
                    break;
                }
            }
            return containsWord;
        }

        [Benchmark]
        public bool ReadUsingFileReadLine()
        {
            foreach(var line in File.ReadLines(fileToRead))
            {
                if(line.Contains(wordToLookFor))
                {
                    return true;
                }
            }
            return false;
        }
    }
}