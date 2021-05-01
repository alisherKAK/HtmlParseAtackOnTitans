using System.Collections.Generic;

namespace HtmlParseAtackOnTitans
{
    public class Character
    {
        public string Name { get; set; }
        public Dictionary<string, string> Args { get; set; } = new Dictionary<string, string>();
    }
}
