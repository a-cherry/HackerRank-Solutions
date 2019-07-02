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
  // Returns the max difference between two numbers in an array where max = arr[j] - arr[i] and j > i.
  public static int maxDifference(List<int> arr)
  {
    if (listIsDescending(arr)) {
      return -1;
    }

    return calculateMaxDifference(arr);
  }

  // Calculates the max difference by keeping track of both the maximum difference and the current minimum number.
  private static int calculateMaxDifference(List<int> arr) {
    var maxDiff = arr[1] - arr[0];
    var minItem = arr[0];

    for (int item = 1; item < arr.Count; item++) {
      if (arr[item] - minItem > maxDiff) {
        maxDiff = arr[item] - minItem;
      }
      if (arr[item] < minItem) {
        minItem = arr[item];
      }     
    }

    return maxDiff;
  }

  // Performs a check to verify if the list is already descending 
  private static bool listIsDescending(List<int> arr) {
    var descendingArr = arr.OrderByDescending(item => item).ToList();
    return arr.SequenceEqual(descendingArr);
  }
}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int arrCount = Convert.ToInt32(Console.ReadLine().Trim());

        List<int> arr = new List<int>();

        for (int i = 0; i < arrCount; i++)
        {
            int arrItem = Convert.ToInt32(Console.ReadLine().Trim());
            arr.Add(arrItem);
        }

        int result = Result.maxDifference(arr);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}
