using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper
{
    public class ArticleData
    {
        public string Link { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string LongContent { get; set; }

        public ArticleData(string link, string date, string title, string shortContent, string longContent)
        {
            Link = link;
            Date = date;
            Title = title;
            ShortContent = shortContent;
            LongContent = longContent;
        }
    }
}
