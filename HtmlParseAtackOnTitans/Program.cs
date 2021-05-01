using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParseAtackOnTitans
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUrl = "https://attackontitan.fandom.com";
            string url = baseUrl + "/wiki/List_of_characters/Anime";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            int i = 1;
            List<HtmlKV> kvs = new List<HtmlKV>();
            foreach(var node in 
                doc.DocumentNode.SelectNodes("//div[@class='characterbox-container']"))
            {
                if (i <= 11)
                {
                    i++;
                    continue;
                }
                foreach(var a in node.Descendants("a"))
                {
                    HtmlKV kv = new HtmlKV()
                    {
                        Key = a.Attributes["href"].Value,
                        Value = a.Attributes["title"].Value
                    };
                    kvs.Add(kv);
                }
            }

            List<Character> characters = new List<Character>();
            foreach(var kv in kvs)
            {
                Character character = new Character()
                {
                    Name = kv.Value
                };
                var doc_c = web.Load(baseUrl + kv.Key);
                var aside = doc_c.DocumentNode.Descendants("aside").FirstOrDefault();
                foreach(var div in aside.SelectNodes("//div[@class='pi-item pi-data pi-item-spacing pi-border-color']"))
                {
                    var innerDiv = div.Descendants("div").FirstOrDefault();
                    var innerH3 = div.Descendants("h3").FirstOrDefault();
                    string divPlainText = HtmlUtilities.ConvertToPlainText(innerDiv.InnerText);
                    string h3PlainText = HtmlUtilities.ConvertToPlainText(innerH3.InnerText);
                    character.Args.Add(h3PlainText, divPlainText);
                }
                characters.Add(character);
            }

            using(var stream = new StreamWriter("titans.json"))
            {
                string result = JsonConvert.SerializeObject(characters);
                stream.WriteLine(result);
            }
        }
    }
}
