using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JSONParser
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            var json = client.DownloadString("https://raw.githubusercontent.com/jdolan/quetoo/master/src/cgame/default/ui/settings/SystemViewController.json");
            MainView view = JsonConvert.DeserializeObject<MainView>(json);
            while (true)
            {
                Console.WriteLine("Please write the class, className, or identifier you wish to find views by (case sensitive).");
                string input = Console.ReadLine().Trim();
                List<string> matchingViews = new List<string>();
                matchingViews = parseViews(view.subviews, input);
                Console.WriteLine("Found " + matchingViews.Count + " matching subviews.");
                if (matchingViews.Count > 0)
                    Console.WriteLine("Here are the corresponding JSON strings:\n");
                foreach (var mv in matchingViews)
                {
                    Console.WriteLine(mv);
                    Console.WriteLine("\n");
                }
            }
        }

        public static List<string> parseViews(List<Subview> subviews, string selector)
        {
            List<string> newList = new List<string>();
            foreach (var sv in subviews)
            {
                if (sv.@class == selector || (sv.classNames != null && sv.classNames.Contains(selector)) || sv.identifier == selector || (sv.control != null && sv.control.identifier == selector))
                    newList.Add(JsonConvert.SerializeObject(sv, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));
                if (sv.subviews != null)
                    newList.AddRange(parseViews(sv.subviews, selector));
                if (sv.contentView != null)
                    newList.AddRange(parseViews(sv.contentView.subviews, selector));
            }
            return newList;
        }
    }

    public class Text
    {
        public string text { get; set; }
    }

    public class Label
    {
        public Text text { get; set; }
    }

    public class Text2
    {
        public string text { get; set; }
    }

    public class Label2
    {
        public Text2 text { get; set; }
    }

    public class Control
    {
        public string @class { get; set; }
        public string identifier { get; set; }
        public string var { get; set; }
        public double? min { get; set; }
        public int? max { get; set; }
        public int? step { get; set; }
        public bool? expectsStringValue { get; set; }
    }

    public class ContentView
    {
        public List<Subview> subviews { get; set; }
    }

    public class Title
    {
        public string text { get; set; }
    }

    public class Subview
    {
        public string @class { get; set; }
        public List<string> classNames { get; set; }
        public List<Subview> subviews { get; set; }
        public string identifier { get; set; }
        public Title title { get; set; }
        public Label2 label { get; set; }
        public Control control { get; set; }
        public ContentView contentView { get; set; }
    }

    public class MainView
    {
        public string identifier { get; set; }
        public List<Subview> subviews { get; set; }
    }
}