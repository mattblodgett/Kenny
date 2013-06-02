using CsQuery;
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
		public List<FoundLogin> CollectLogins(string authenticatedUrl)
		{
			List<FoundLogin> foundLogins = new List<FoundLogin>();

			for (int page = 1; page <= 10; page++)
			{
				List<FoundLogin> loginsForPage = CollectLogins(authenticatedUrl, page);

				foreach (FoundLogin login in loginsForPage)
				{
					if (!foundLogins.Any(fl => fl.Username == login.Username))
					{
						foundLogins.Add(login);
					}
				}
			}

			return foundLogins;
		}

		private List<FoundLogin> CollectLogins(string authenticatedUrl, int page)
		{
			List<FoundLogin> foundLogins = new List<FoundLogin>();

			const string SEARCH_URL_FORMAT = "https://encrypted.google.com/search?q=%22{0}%22&safe=off&start={1}";
			const string REGEX_FORMAT = @"http://(\w+):(\w+)@(www\.)?{0}";

			Regex regex = new Regex(string.Format(REGEX_FORMAT, authenticatedUrl));

			WebClient webClient = new WebClient();
			Stream stream = webClient.OpenRead(string.Format(SEARCH_URL_FORMAT, authenticatedUrl, (page - 1) * 10));
			StreamReader reader = new StreamReader(stream);
			string searchResults = reader.ReadToEnd();

			CQ dom = searchResults;
			CQ cqResultLIs = dom["li.g"];

			foreach (var resultLI in cqResultLIs)
			{
				CQ cqResultLI = resultLI.OuterHTML;
				string resultHref = cqResultLI.Find(".r a").Attr("href");
				string sourceUrl = HttpUtility.ParseQueryString(resultHref)[0];
				string regexable = cqResultLI.Find(".st").Text().Replace(" ", "");
				Match match = regex.Match(regexable);
				while (match.Success)
				{
					string username = match.Groups[1].Value;
					string password = match.Groups[2].Value;

					if (!foundLogins.Any(fl => fl.Username == username))
					{
						FoundLogin foundLogin = new FoundLogin();
						foundLogin.Username = username;
						foundLogin.Password = password;
						foundLogin.SourceUrl = sourceUrl;
						foundLogin.DateCollected = DateTime.Now;

						foundLogins.Add(foundLogin);
					}

					match = match.NextMatch();
				}
			}

			return foundLogins;
		}
	}
}