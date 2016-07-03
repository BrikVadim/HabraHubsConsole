using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace HabraHubsConsole
{
    class Program
    {
        private static WebClient client;
        private static string xmlStr;
        private static XmlSerializer xml;
        private static RssClass res;
        private static int selected = 0;
        static void moweDown()
        {
            if (selected < 20) {
                Console.SetCursorPosition(0, selected);
                Console.MoveBufferArea(Console.CursorLeft + 2, Console.CursorTop, Console.BufferWidth - 2, 1, 0, Console.CursorTop);
                selected++;
                Console.SetCursorPosition(0, selected);
                Console.MoveBufferArea(Console.CursorLeft, Console.CursorTop, Console.BufferWidth - 2, 1, 2, Console.CursorTop);
                Console.Write("->");
            }
            else
            {
                Console.SetCursorPosition(0, selected);
                Console.Write("-");
                Console.SetCursorPosition(0, selected);
            }
        }
        
        static void moveUp()
        {
            if (selected > 1) { 
                Console.SetCursorPosition(0, selected);
                Console.MoveBufferArea(Console.CursorLeft + 2, Console.CursorTop, Console.BufferWidth - 2, 1, 0, Console.CursorTop);
                selected--;
                Console.SetCursorPosition(0, selected);
                Console.MoveBufferArea(Console.CursorLeft, Console.CursorTop, Console.BufferWidth - 2, 1, 2, Console.CursorTop);
                Console.Write("->");
            }
            else
            {
                Console.SetCursorPosition(0, selected);
                Console.Write("-");
                Console.SetCursorPosition(0, selected);
            }
        }

        static void openPost()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("   {0}   \n", res.channel.Items[selected - 1].Title);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            HtmlDocument htmlDocument = new HtmlDocument();
            xmlStr = client.DownloadString(res.channel.Items[selected - 1].Link);
            htmlDocument.LoadHtml(xmlStr);

            Console.Title = htmlDocument.DocumentNode.SelectSingleNode("//title").InnerText;
            Console.WriteLine(htmlDocument.DocumentNode.SelectSingleNode("//div[@class='content html_format']").InnerText);

            Console.SetCursorPosition(0,0);

            Console.ReadKey();
        }

        static void openHubs()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("   <Habra");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("/Hubs>   ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            client = new WebClient();
            client.Encoding = Encoding.UTF8;
            xmlStr = client.DownloadString("https://habrahabr.ru/rss/hubs/all/");
            xml = new XmlSerializer(typeof(RssClass));
            res = xml.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlStr))) as RssClass;

            Console.Title = " <Habra/Hubs> " + res.channel.Title;

            foreach (ItemRss item in res.channel.Items)
            {
                if (item.Title.Length > Console.BufferWidth - 6)
                    Console.WriteLine(" {0}...", item.Title.Remove(Console.BufferWidth - 6));
                else
                    Console.WriteLine(" {0}", item.Title);
            }

            for (int i = 0; i != Console.BufferWidth; i++)
                Console.Write("-");

            Console.WriteLine("Для перемещения используйте клавиши стрелок (Вверх/Вниз)");
        }

        static void Main(string[] args)
        {
            Console.Title = " <Habra/Hubs> Загрузка...";

            while (true)
            {
                selected = 0;
                openHubs();

                Console.SetCursorPosition(0, selected);
                Console.CursorVisible = false;

                moweDown();

                while (true)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.DownArrow:
                            moweDown();
                            continue;
                        case ConsoleKey.UpArrow:
                            moveUp();
                            continue;
                        case ConsoleKey.Enter:
                            openPost();
                            break;
                    }
                    break;
                }
            }
        }
    }
}
