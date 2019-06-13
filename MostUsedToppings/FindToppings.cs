using System.Collections.Generic;
using System.Net;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

public class FindToppings
{
    private string _url { get; set; }
    private int _numToFind { get; set; }

    public FindToppings(string url, int numToFind)
    {
        _url = url;
        _numToFind = numToFind;
    }

    ///<summary>
    /// Read the URL and parse the json to retrieve all the toppings, count them and 
    /// return the top number of entries.
    ///</summary>
    public List<ToppingResult> RetrieveToppings()
    {
        var toppings = new List<ToppingResult>();

        try
        {
            using (WebClient wc = new WebClient())
            {
                var allToppings = new List<string>();

                var json = wc.DownloadString(_url);

                JArray jsonArray = JArray.Parse(json);

                foreach (var item in jsonArray)
                {
                    var toppingCombo = new List<string>();

                    foreach (var topping in item["toppings"])
                    {
                        toppingCombo.Add(topping.ToString());
                    }

                    allToppings.Add(String.Join(',', toppingCombo.OrderBy(x => x)));

                }

                var groupings = allToppings
                                .GroupBy(x => x)
                                .OrderByDescending(x => x.Count());

                for (var i = 0; i < _numToFind; i++)
                {
                    var currentCombo = groupings.Skip(i).FirstOrDefault();

                    toppings.Add(new ToppingResult()
                    {
                        Rank = i,
                        Toppings = currentCombo.Key,
                        NumRequests = currentCombo.Count()                    
                    });
                }

            }
        }
        catch (System.Exception)
        {
            //  Ideally, this should be logged but that's for people getting paid.
            throw;
        }

        return toppings;
    }
}