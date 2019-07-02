using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;



class Result
{
  // Returns the minimum path (up or down) to a disired song starting at the current song
  public static int playlist(List<string> songs, int currentSong, string requestedSong)
  {
    var downPresses = pressCalculator(
      songs,
      currentSong,
      requestedSong
    );

    songs.Reverse(); // reverse song order to traverse the array in the "up" direction
    var reverseOrderCurrentSong = songs.Count - currentSong - 1; // Optimization: IndexOf() should have been used.
    var upPresses = pressCalculator(
      songs,
      reverseOrderCurrentSong,
      requestedSong
    );

    return Math.Min(downPresses, upPresses);
  }

  // Calculates the minimum number of items between the currentSong and the requestedSong
  private static int pressCalculator(List<string> songs, int currentSong, string requestedSong) {
    var newSongList = songs.GetRange(currentSong, songs.Count - currentSong); // Build a new list where currentSong is the zero index
    newSongList.AddRange(
      songs.GetRange(0, currentSong)
    );

    return newSongList.IndexOf(requestedSong);
  }
}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int songsCount = Convert.ToInt32(Console.ReadLine().Trim());

        List<string> songs = new List<string>();

        for (int i = 0; i < songsCount; i++)
        {
            string songsItem = Console.ReadLine();
            songs.Add(songsItem);
        }

        int k = Convert.ToInt32(Console.ReadLine().Trim());

        string q = Console.ReadLine();

        int result = Result.playlist(songs, k, q);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}
