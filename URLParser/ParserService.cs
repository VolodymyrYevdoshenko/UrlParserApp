using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace URLParser
{
    public class ParserService : IParserService
    {
        private int _depth;
        private string _startUrl;
        private string _startSource;
        private int _urlUsage;

        private List<ParsedLink> _parsedLinks;

        public ParserService()
        {
            _parsedLinks = new List<ParsedLink>();
        }
        public async Task<List<ParsedLink>> Parse(IParserQuery query)
        {
            _depth = query.Depth;
            _startUrl = query.StartUrl;
            _startSource = query.StartSource;

            if (_depth < 0)
            {
                Console.WriteLine("Maximum depth can not be negative!");
                return _parsedLinks;
            }

            if (_depth == 0)
            {
                Console.WriteLine("Maximum depth is reached!");
                return _parsedLinks;
            }

            await DownloadAndSaveAsync(_startUrl);

            return _parsedLinks;
        }

        private async Task DownloadAndSaveAsync(string url)
        {
            if (!url.StartsWith("http"))
            {
                url = string.Concat("http://", url);
            }

            if (_parsedLinks.Where(_ => _.Url == url).ToList().Count > 0) return;

            if (!(_urlUsage < _depth)) return;

            string content;
            using (var client = new WebClient())
            {
                try
                {
                    content = await client.DownloadStringTaskAsync(url);
                }
                catch (Exception e)
                {
                    content = e.Message;
                }   
            }

            string fileName;
            try
            {
                fileName = new Uri(url).Host;
            }
            catch (UriFormatException)
            {
                return;
            }

            _urlUsage++;

            var filePath = await SaveFileAsync(content, fileName);

            var parsedLink = new ParsedLink
            {
                Url = url,
                SourceDirectory = filePath
            };

            _parsedLinks.Add(parsedLink);

            var innerUrls = await FindUrlsAsync(content);

            foreach (var innerUrl in innerUrls)
            {
                await DownloadAndSaveAsync(innerUrl);
            }
        }

        private async Task<List<string>> FindUrlsAsync(string content)
        {
            return await Task.Run(()=>
            {
                return Regex.Matches(content, @"[http|https]://(.+?)/")
                    .Cast<Match>()
                    .Select(_ => _.Value.Substring(4).Split("\""[0]).First())
                    .Distinct()
                    .ToList();
            });
        }

        private async Task<string> SaveFileAsync(string content, string name)
        {
            return await Task.Run(() =>
            {
                if (!(_startSource.EndsWith("/") || _startSource.EndsWith("\\")))
                {
                    _startSource = string.Concat(_startSource, "/");
                }
                var path = string.Concat(_startSource,name);
                path = path.Replace('.', '_');
                while (path.EndsWith("/") || path.EndsWith("\\"))
                {
                    path=path.Remove(path.Length-1);
                }

                path=string.Concat(path, ".txt");

                using (var fileWriter = new StreamWriter(path))
                {
                    fileWriter.WriteLine(content);
                }
                return path;
            });
        }
    }
}
