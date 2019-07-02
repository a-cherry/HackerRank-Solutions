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


// Class for inidividual Items
public class Item {
  public string name { get; set; }
  public int relevance { get; set; }
  public int price { get; set; }

  public Item(List<string> Item) {
    name = Item[0];
    relevance = Int32.Parse(Item[1]);
    price = Int32.Parse(Item[2]);
  }
}

// Class for page.  Contains a list of all of the itemNames on a given page.
public class Page {
  public List<string> itemNames { get; set; }

  public Page(List<string> ItemNames) {
    itemNames = ItemNames;
  }
}

class Result
{
  // Gets the specific items to display on the specified page
  public static List<string> fetchItemsToDisplay(List<List<string>> items, int sortParameter, int sortOrder, int itemsPerPage, int pageNumber)
  {
    var itemList = getItemList( // parse items into List<Item> and sorts them
      items,
      sortParameter,
      sortOrder
    );
    var pages = createPages( // builds out website pages using the itemList
      itemList,
      itemsPerPage
    );

    return pages[pageNumber].itemNames;
  }

  // Creates a list of Pages based on the the amount of items in a page.
  private static List<Page> createPages(List<Item> items, int itemsPerPage) {
    var pages = new List<Page>();
    decimal decNumPages = items.Count / itemsPerPage;
    var numPages = Decimal.ToInt32(
      Math.Floor(decNumPages)
    ) + 1;

    // iterates once per page
    for (int iterator = 0; iterator < numPages; iterator++) {
      var firstItemIndex = iterator * itemsPerPage;
      var itemsLeft = items.Count - firstItemIndex;
      var pageItems = items.GetRange(firstItemIndex, itemsLeft < itemsPerPage ? itemsLeft : itemsPerPage) // ternary checks if it's the last page
                           .Select(item => item.name)
                           .ToList();

      pages.Add(new Page(pageItems));
    }

    return pages;
  }

  // Parses the List<List<string>> of items into a correctly sorted List of Items
  private static List<Item> getItemList(List<List<string>>items, int sortParameter, int sortOrder) {
    var parsedItems = parseItems(items);
    var sortedItems = sortItems(
      parsedItems,
      sortParameter,
      sortOrder
    );

    return sortedItems;
  }

  // In order to sort the items dynamically based on the sortParameter, use Type.GetProperty() to get a PropertyInfo object.
  private static PropertyInfo getSortProperty(List<Item> items, int sortParameter) {
    string nameOfProperty;

    switch(sortParameter) {
      case 0:
        nameOfProperty = "name";
        break;
      case 1:
        nameOfProperty = "relevance";
        break;
      case 2:
        nameOfProperty = "price";
        break;
      default:
        nameOfProperty = "";
        break;
    }

    var sortProperty = items[0].GetType().GetProperty(nameOfProperty);

    return sortProperty;
  }

  // Parses the List<List<string>> of items into a list of Items
  private static List<Item> parseItems(List<List<string>> items) {
    return items.Select(item => new Item(item)).ToList();
  }

  // Sorts a list of Items based on a colum (sortParameter) and a given order.  The sortOrder variable is a binary where 0 = ascending and 1 = descending.
  private static List<Item> sortItems(List<Item> items, int sortParameter, int sortOrder) {
    var sorted = new List<Item>();
    var sortProperty = getSortProperty( // gets the property that the list will be sorted on
      items,
      sortParameter
    );

    if (sortOrder == 0) {
      sorted = items.OrderBy(
        item => sortProperty.GetValue(item)
      ).ToList();
    } else if (sortOrder == 1) {
      sorted = items.OrderByDescending(
        item => sortProperty.GetValue(item)
      ).ToList();
    }

    return sorted;
  }
}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int itemsRows = Convert.ToInt32(Console.ReadLine().Trim());
        int itemsColumns = Convert.ToInt32(Console.ReadLine().Trim());

        List<List<string>> items = new List<List<string>>();

        for (int i = 0; i < itemsRows; i++)
        {
            items.Add(Console.ReadLine().TrimEnd().Split(' ').ToList());
        }

        int sortParameter = Convert.ToInt32(Console.ReadLine().Trim());

        int sortOrder = Convert.ToInt32(Console.ReadLine().Trim());

        int itemPerPage = Convert.ToInt32(Console.ReadLine().Trim());

        int pageNumber = Convert.ToInt32(Console.ReadLine().Trim());

        List<string> result = Result.fetchItemsToDisplay(items, sortParameter, sortOrder, itemPerPage, pageNumber);

        textWriter.WriteLine(String.Join("\n", result));

        textWriter.Flush();
        textWriter.Close();
    }
}
