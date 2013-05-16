using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Kenny.Models
{
	public class FoundLoginCollector
	{
		public Dictionary<string, string> CollectLogins(string authenticatedUrl)
		{
			Dictionary<string, string> logins = new Dictionary<string, string>();

			for (int page = 1; page <= 10; page++)
			{
				var loginsForPage = CollectLogins(authenticatedUrl, page);

				foreach (var pair in loginsForPage)
				{
					if (!logins.ContainsKey(pair.Key))
					{
						logins.Add(pair.Key, pair.Value);
					}
				}
			}

			return logins;
		}

		private Dictionary<string, string> CollectLogins(string authenticatedUrl, int page)
		{
			Dictionary<string, string> logins = new Dictionary<string, string>();

			const string SEARCH_URL_FORMAT = "https://encrypted.google.com/search?q=%22{0}%22&safe=off&start={1}";
			const string REGEX_FORMAT = @"http://(\w+):(\w+)@{0}";

			WebClient webClient = new WebClient();
			Stream stream = webClient.OpenRead(string.Format(SEARCH_URL_FORMAT, authenticatedUrl, (page - 1) * 10));
			StreamReader reader = new StreamReader(stream);
			string searchResults = reader.ReadToEnd();

			List<string> removeThem = new List<string>
			{
				"<em>",
				"</em>",
				"<wbr>",
				"</wbr>",
				"<b>",
				"</b>",
				"<br>"
			};

			foreach (string removeIt in removeThem)
			{
				searchResults = searchResults.Replace(removeIt, string.Empty);
			}

			searchResults = Regex.Replace(searchResults, @"\s+", "");

			string loginItems = string.Empty;

			Regex regex = new Regex(string.Format(REGEX_FORMAT, authenticatedUrl));
			Match match = regex.Match(searchResults);
			while (match.Success)
			{
				string username = match.Groups[1].Value;
				string password = match.Groups[2].Value;

				if (!logins.ContainsKey(username))
				{
					logins.Add(username, password);
				}

				match = match.NextMatch();
			}

			return logins;
		}
	}
}