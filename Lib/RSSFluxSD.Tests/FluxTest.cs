using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSSFluxSD;
using System.Threading;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace RSSFluxSD.Tests
{
	[TestClass]
	public class FluxTest
	{

		[TestMethod]
		public void TestReadWithManage()
		{
			//Faire methode pour lire une seule et unique rss
			RSSManage rssM = new RSSManage();
			//Verfier si on a pas internet;
			//RSS rss1 = rssM.readRSS("https://fr.news.yahoo.com/rss/world");
			//rssM.readRSS("https://fr.news.yahoo.com/rss/world");
			//rssM.readRSS("http://www.developpez.com/index/rss");

			RSS rss = rssM.readRSS("testAdd.xml");

			Console.WriteLine(rss.Tilte());
			foreach ( Article article in rss.GetAllArticle())
			{
				Console.WriteLine(article.Title);
				Console.WriteLine(article.Content);
			}

			List<Article> ListArticle = new List<Article>();
			ListArticle.Add(new Article("Test7", "Le test 7"));
			rss.AddArticle(ListArticle);
			Console.WriteLine(rssM.GetAllRSS().Count);
			//Assert.AreEqual(rssM.GetAllRSS().Count, 2);
			foreach( RSS rsst in rssM.GetAllRSS())
			{
				Console.WriteLine(rsst.GetAllArticle().Count);
			}
			Console.WriteLine();

			HelpTest.HelpReadWithManage(rssM);

		}

		[TestMethod]
		public void TestCreateAndAddXML()
		{
			RSSManage rssM = new RSSManage();
			string path = "test.xml";
			RSS rss = rssM.createRSS(path, "Test", "C'est un test", Helper.CategorieRSSEnum.Etudiant);
			RSS rss1 = rssM.createRSS("https://fr.news.yahoo.com/rss/world", "Test", "C'est un test", Helper.CategorieRSSEnum.Etudiant);

			if ( rssM.Msg_error != null)
			{
				Console.WriteLine(rssM.Msg_error);
			}
			rss.Save(Helper.FormatRSSEnum.RSS20);
			HelpTest.HelpReadWithManage(rssM);

			Assert.AreEqual(rss.Uri_RSS, "test.xml");
			Assert.AreEqual(rss.Tilte(), "Test");
			Assert.AreEqual(rss.Content(), "C'est un test");
			Assert.AreEqual(rss.Author(), "Student DATA RSS");
			Assert.AreEqual(rss.Categorie(), "Etudiant");

			Assert.AreEqual(Helper.TryFileExist(path), true);
			Assert.AreEqual(Helper.TryFileEmpty(path), false);
			//rss.RemoveRSS();

		}


		[TestMethod]
		public void TestAddWithManage()
		{
			RSSManage rssM = new RSSManage();
			List<Article> ListArticle = new List<Article>();

			for (int i = 0; i < 4; i++ )
			{
				ListArticle.Add(new Article("Test"+i, "Le test "+i));
			}
			string path = "testAdd.xml";
			//string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "testAddUpDel.xml");
			RSS rss = rssM.createRSS(path, "TestAddUpDel", "C'est un test d'ajout",Helper.CategorieRSSEnum.Etudiant);
			

			rss.AddArticle(ListArticle);
			
			//A finir
			//rssM.addArticle("testAddUpDel.xml", ListArticle);

			rss.Save(Helper.FormatRSS20());

			Console.WriteLine(rss.GetAllArticle().Count);
			HelpTest.HelpReadWithRSS(rss);

			//Assert.AreEqual(rss.GetAllArticle().Count, 4);
			int i2 = 0;
			foreach (Article article in rss.GetAllArticle())
			{
				if ( i2 == 4)
				{
					i2 = 0;
				}
				Assert.AreEqual(article.Title, "Test" + i2);

				string md5ID = article.Title + article.Content + article.Date;
				string idVar = "";
				using (MD5 md5Hash = MD5.Create())
				{
					idVar = Helper.mD5Hash(md5Hash, md5ID);
				}
				Assert.AreEqual(article.Id, idVar);
				Assert.AreEqual(article.Content, "Le test "+i2);
				Assert.AreEqual(article.Url, "http://www.google.com/");
				i2++;
			}
			Assert.AreEqual(Helper.TryFileExist(path), true);
			Assert.AreEqual(Helper.TryFileEmpty(path), false);
			rss.RemoveRSS();
		}

		[TestMethod]
		public void testDelWithManage()
		{
			RSSManage rssM = new RSSManage();
			RSS rss = rssM.createRSS("testDel.xml", "TestDel", "C'est un test suppréssion des articles", Helper.CategorieRSSEnum.Etudiant);
			List<Article> ListArticle = new List<Article>();

			for (int i = 0; i < 4; i++)
			{
				ListArticle.Add(new Article("Test" + i, "Le test " + i));
			}

			rss.AddArticle(ListArticle);

			rss.RemoveArticle();

			Assert.AreEqual(rss.GetAllArticle().Count, 0);

			rss.AddArticle(ListArticle);

			Assert.AreEqual(rss.GetAllArticle().Count, 4);

			Article articlevar = rss.GetAllArticle().Find(x => x.Title == "Test0");
			rss.RemoveArticle(rss.GetAllArticle().Find(x => x.Title == "Test1"));
			rss.RemoveArticle(articlevar.Id);

			rss.Save(Helper.FormatRSS20());

			Console.WriteLine(rss.GetAllArticle().Count);
			HelpTest.HelpReadWithRSS(rss);

			int i2 = 2;
			foreach( Article article in rss.GetAllArticle())
			{
				Assert.AreEqual(article.Title, "Test" + i2);
				string md5ID = article.Title + article.Content + article.Date;
				string idVar = "";
				using (MD5 md5Hash = MD5.Create())
				{
					idVar = Helper.mD5Hash(md5Hash, md5ID);
				}
				Assert.AreEqual(article.Id, idVar);
				Assert.AreEqual(article.Content, "Le test " + i2);
				Assert.AreEqual(article.Url, "http://www.google.com/");
				i2++;
			}

			Article articlevar2 = rss.GetAllArticle().Find(x => x.Title == "Test3");
			rss.RemoveArticle(articlevar2.Id);

			foreach ( Article article in rss.GetAllArticle())
			{
				Assert.AreEqual(article.Title, "Test2");
				string md5ID = article.Title + article.Content + article.Date;
				string idVar = "";
				using (MD5 md5Hash = MD5.Create())
				{
					idVar = Helper.mD5Hash(md5Hash, md5ID);
				}
				Assert.AreEqual(article.Id, idVar);
				Assert.AreEqual(article.Content, "Le test 2");
				Assert.AreEqual(article.Url, "http://www.google.com/");
			}

			rss.RemoveRSS();
		}

		[TestMethod]
		public void TestUpdateWithMange()
		{
			RSSManage rssM = new RSSManage();
			RSS rss = rssM.createRSS("testUp.xml", "TestUp", "C'est un test modification des articles", Helper.CategorieRSSEnum.Etudiant);
			List<Article> ListArticle = new List<Article>();

			for (int i = 0; i < 4; i++)
			{
				ListArticle.Add(new Article("Test" + i, "Le test " + i));
			}

			rss.AddArticle(ListArticle);

			Article articletest = rss.GetAllArticle().Find(x => x.Title == "Test0");
			Article articletest1 = rss.GetAllArticle().Find(x => x.Title == "Test1");
			Article articletest2 = rss.GetAllArticle().Find(x => x.Title == "Test2");

			rss.UpdateArticle(articletest.Id,"TestTest","tutit");
			rss.UpdateArticle(articletest1.Id, "TitleChanged", true);
			rss.UpdateArticle(articletest2.Id, "ContentChanged", false);

			rss.Save(Helper.FormatRSS20());

			Article article = rss.GetAllArticle().Find(x => x.Title == "TestTest");
			Article article1 = rss.GetAllArticle().Find(x => x.Title == "TitleChanged");
			Article article2 = rss.GetAllArticle().Find(x => x.Content == "ContentChanged");

			string md5ID = article.Title + article.Content + article.Date;
			string md5ID1 = article1.Title + article1.Content + article1.Date;
			string md5ID2 = article2.Title + article2.Content + article2.Date;

			string idVar = "";
			string idVar1 = "";
			string idVar2 = "";

			using (MD5 md5Hash = MD5.Create())
			{
				idVar = Helper.mD5Hash(md5Hash, md5ID);
				idVar1 = Helper.mD5Hash(md5Hash, md5ID1);
				idVar2 = Helper.mD5Hash(md5Hash, md5ID2);
			}

			Assert.AreEqual(article.Title, "TestTest");
			Assert.AreEqual(article.Content, "tutit");
			Assert.AreEqual(article.Id, idVar);

			Assert.AreEqual(article1.Title, "TitleChanged");
			Assert.AreEqual(article1.Content, "Le test 1");
			Assert.AreEqual(article1.Id, idVar1);

			Assert.AreEqual(article2.Title, "Test2");
			Assert.AreEqual(article2.Content, "ContentChanged");
			Assert.AreEqual(article2.Id, idVar2);

			Console.WriteLine(rss.GetAllArticle().Count);
			HelpTest.HelpReadWithRSS(rss);

			rss.RemoveRSS();
		}

		[TestMethod]
		public void TestReadRSS()
		{
			//TODO : Gerer exeption mauvais lien url
			string url = "http://www.developpez.com/index/rss";
			string url2 = "https://fr.news.yahoo.com/rss/world";
			string urlFail = "https://fr.news.yahoo.com/sitemap/";
			string url3 = "test.xml";

			RSS rss = new RSS(url);
			RSS rss2 = new RSS(url2);
			RSS rss3 = new RSS(url3);
			RSS rssFail = new RSS(urlFail);
			rss3.RemoveRSS();

			rss.ReadRSS();
			rss2.ReadRSS();
			rss3.ReadRSS();
			rssFail.ReadRSS();

			

			HelpTest.HelpRead(rss.Feed);
			Console.WriteLine("Other FEED WEB\n\n");
			HelpTest.HelpRead(rss2.Feed);
			Console.WriteLine("Other FEED FILE\n\n\n");
			HelpTest.HelpRead(rss3.Feed);
			Console.WriteLine("Other FEED FAIL\n\n\n");
			HelpTest.HelpRead(rssFail.Feed);
			rss3.RemoveRSS();
		}

		[TestMethod]
		public void TestCreateRSS()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);
			
			rss.InitRSSSingle();
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();

		}

		[TestMethod]
		public void TestAddArticle()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.AddArticleSingle();
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
			
		}

		[TestMethod]
		public void TestDeleteArticle()
		{
			string uri = "test.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.RemoveArticleSingle(8);
			rss.RemoveArticleSingle(id);
			rss.RemoveArticleSingle(id);
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
		}

		[TestMethod]
		public void TestUpdateArticle()
		{
			string uri = "test.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.UpdateArticleSingle(8);
			rss.UpdateArticleSingle(id);
			rss.RemoveArticleSingle(id);
			rss.UpdateArticleSingle(id);
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
		}

		[TestMethod]
		public void TestAllRSS()
		{
			string uri = "testRSS.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.AddArticleSingle();
			rss.RemoveArticleSingle(id);
			rss.UpdateArticleSingle(id);
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
		}


	}
}
