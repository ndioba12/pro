using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using SIGRHMetier.Model;


namespace SIGRHMetier.Model
{
	public class Utils
	{
		//Déclaration de la variable permettant d'accéder à la base de données
		BdJUSTICEEntities db = new BdJUSTICEEntities();

		/// <summary>
		/// rédiger les erreurs au niveau de la base de données
		/// </summary>
		/// <param name="page">la page de la fonction</param>
		/// <param name="fonction">la fonction provequant l'erreur</param>
		/// <param name="erreur">le message d'erreur</param>
		public void WriteDataError(string page, string fonction, string erreur)
		{
			try
			{
				TD_LogErreur log = new TD_LogErreur();
				log.Log_DateErreur = DateTime.Now;
				log.Log_Erreur = erreur.Length > 1000 ? erreur.Substring(0, 1000) : erreur;
				log.Log_Fonction = fonction;
				log.Log_Page = page;
				db.TD_LogErreur.Add(log);
				db.SaveChanges();
			}
			catch (Exception ex)
			{
				WriteLogSystem(ex.ToString(), "WriteDataError");
			}
		}

		/// <summary>
		/// Rédiger le message d'erreur dans un fichier
		/// </summary>
		/// <param name="message">le message d'erreur</param>
		public static void WriteFileError(string message)
		{
			try
			{
				string path = System.Web.HttpContext.Current.Server.MapPath("~/Error/erreur.txt");
				System.IO.TextWriter writeFile = new StreamWriter(path, true);
				writeFile.WriteLine("" + DateTime.Now);
				writeFile.WriteLine(message);
				writeFile.WriteLine("---------------------------------------------------------------------------------------");
				writeFile.Flush();
				writeFile.Close();
				writeFile = null;
			}
			catch (IOException e)
			{
				WriteLogSystem(e.ToString(), "WriteFileError");
			}
		}


		/// <summary>
		/// Permet de rédiger une liste d'erreur dans un fichier
		/// </summary>
		/// <param name="message">liste message d'erreur</param>
		/// <param name="theFile">nom du fichier</param>
		public void WriteErrorLoad(List<string> message, string theFile)
		{
			try
			{
				string path = System.Web.HttpContext.Current.Server.MapPath("~/Error/" + theFile + ".txt");
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				System.IO.TextWriter writeFile = new StreamWriter(path, true);

				writeFile.WriteLine("---------------------DEBUT----------------------");
				foreach (var item in message)
				{
					writeFile.WriteLine(item);
				}
				writeFile.WriteLine("----------------------FIN---------------------");
				writeFile.Flush();
				writeFile.Close();
				writeFile = null;
			}
			catch (IOException e)
			{
				WriteLogSystem(e.ToString(), "WriteErrorLoad");
			}
		}
		/// <summary>
		/// Ecrire un message d'erreur au niveau du Systéme
		/// </summary>
		/// <param name="erreur">Message d'erreur</param>
		/// <param name="libelle">titre du message</param>
		public static void WriteLogSystem(string erreur, string libelle)
		{
			//using (EventLog eventLog = new EventLog("Application"))
			//{
			//	eventLog.Source = "JUSTICE";
			//	eventLog.WriteEntry(string.Format("date: {0}, libelle: {1}, description {2}", DateTime.Now, libelle, erreur), EventLogEntryType.Information, 101, 1);
			//}
		}
    }
}