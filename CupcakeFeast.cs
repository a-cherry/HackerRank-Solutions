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



// Contains individual elements of the transaction.
public class Transaction { //check if you can make private
  public int purchasedCupcakes { get; set; }
  public int remainder { get; set; } // currency not used in transaction

  public Transaction(int PurchasedCupcakes, int Remainder) {
    purchasedCupcakes = PurchasedCupcakes;
    remainder = Remainder;
  }
}

// Object for Result.maximumCupcakes input
public class Trip {
  public int unitExchange { get; set; }
  public int units { get; set; }
  public int wrapperExchange { get; set; }

  public Trip (string tripInfo) {
    var trip = tripInfo.Split(null);

    unitExchange = Int32.Parse(trip[1]);
    units = Int32.Parse(trip[0]);
    wrapperExchange = Int32.Parse(trip[2]);
  }
}

class Result
{
  // Prints total cupcakes per trip
  public static void maximumCupcakes(List<string> trips)
  {
    var tripList = parseTrips(trips);

    foreach (Trip trip in tripList) {
      Console.WriteLine(getCupcakes(trip)); //output for tests
    }
  }

  // Performs a serious of transactions and outputs the total number of cupcakes bought.
  private static int getCupcakes(Trip trip) {
    var unitCupcakes = unitTransaction( // initial cupcake purchase
      trip.units,
      trip.unitExchange
    );
    var wrapperCupcakes = wrapperTransaction( // recursive wrapper purchase
      unitCupcakes.purchasedCupcakes,
      trip.wrapperExchange
    );
    var leftoverWrapperCupcakes = wrapperTransaction(  // final wrapper purchase with total remainder
      wrapperCupcakes.remainder,
      trip.wrapperExchange
    );

    return unitCupcakes.purchasedCupcakes + wrapperCupcakes.purchasedCupcakes + leftoverWrapperCupcakes.purchasedCupcakes;
  }

  // Organizes the list of strings into a list of Trips
  private static List<Trip> parseTrips(List<string> trips) {
    return trips.Select(trip => new Trip(trip)).ToList();
  }

  // Performs transaction on an arbitrary unit
  private static Transaction unitTransaction(int units, int cost) {
    decimal decCupcakes = units / cost;
    int cupcakes = Decimal.ToInt32(Math.Floor(decCupcakes));
    int leftoverUnits = Decimal.ToInt32(units % cost);

    return new Transaction(cupcakes, leftoverUnits);
  }

  // Performs transaction specifically for wrappers.  Calculated recursively.
  private static Transaction wrapperTransaction(int wrappers, int cost) {
    var wrapperCupcakes = unitTransaction(wrappers, cost); //buy cupcakes

    if (wrapperCupcakes.purchasedCupcakes >= cost) { // perform another cupcake purchase
      var recursiveCupcakes = wrapperTransaction(
        wrapperCupcakes.purchasedCupcakes,
        cost
      );
      wrapperCupcakes.purchasedCupcakes += recursiveCupcakes.purchasedCupcakes;
      wrapperCupcakes.remainder += recursiveCupcakes.remainder;
    } else { // once recursion ends, the final wrapperCupcakes are added to the remainder for another potential purchase
      wrapperCupcakes.remainder += wrapperCupcakes.purchasedCupcakes;
    }

    return wrapperCupcakes;
  }
}

class Solution
{
    public static void Main(string[] args)
    {
        int tripsCount = Convert.ToInt32(Console.ReadLine().Trim());

        List<string> trips = new List<string>();

        for (int i = 0; i < tripsCount; i++)
        {
            string tripsItem = Console.ReadLine();
            trips.Add(tripsItem);
        }

        Result.maximumCupcakes(trips);
    }
}
