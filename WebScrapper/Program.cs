using CsvHelper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WebScrapper
{
    class Program
    {
        static readonly Regex trimmer = new Regex(@"\s\s+");
        static void Main(string[] args)
        {
            ArticleData articleList;
            var result = new List<ArticleData>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://kapital.kz");

            var articleClasses = "news-feed__item news-feed__item--main";
            var articleDateClasses = "information-article__date";
            var articleTitleClasses = "news-feed__name";
            var articleShortContentClasses = "news-feed__anons";
            var articleLongContentClasses = "article__body";

            var articles = doc.DocumentNode.SelectNodes("//article[@class='" + articleClasses + "']");
            var articleDates = doc.DocumentNode.SelectNodes("//article[@class='" + articleClasses + "']//time[@class='" + articleDateClasses + "']");
            var articleTitles = doc.DocumentNode.SelectNodes("//article[@class='" + articleClasses + "']//a[@class='" + articleTitleClasses + "']");
            var articleShortContents = doc.DocumentNode.SelectNodes("//article[@class='" + articleClasses + "']//p[@class='" + articleShortContentClasses + "']");

            for (var i = 0; i < articles.Count; i++)
            {
                var link = "https://kapital.kz" + articleTitles[i].GetAttributeValue("href", string.Empty);
                var rawDate = trimmer.Replace(articleDates[i].InnerText, " ");
                var date = rawDate.Replace("&middot;", "");
                var title = trimmer.Replace(articleTitles[i].InnerText, " ");
                var shortContent = trimmer.Replace(articleShortContents[i].InnerText, " ");

                HtmlDocument innerDoc = web.Load(link);
                var articleLongContent = innerDoc.DocumentNode.SelectNodes("//div[@class='" + articleLongContentClasses + "']");
                var longContent = trimmer.Replace(articleLongContent[0].InnerText, " ");

                articleList = new ArticleData(
                    link,
                    date,
                    title,
                    shortContent,
                    longContent
                    );

                result.Add(articleList);
            }

            using (var writer = new StreamWriter(new FileStream("C:\\Users\\MI\\Documents\\test.csv", FileMode.Create), Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(result);
            }
        }
    }
}
