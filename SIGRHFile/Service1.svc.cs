using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SIGRHFile
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class Service1 : IService1
    {
        public  string DefaultDirectory = "C:\\SIGRH\\Impression\\";
        public string[] ExtensionsAutorises = new string[] { ".pdf", ".docx", ".xlxs" };
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        /// <summary>
        /// Télécharger un fichier.
        /// </summary>
        /// <param name="chemin">le chemin du fichier</param>
        /// <param name="name">le nom du fichier</param>
        /// <param name="fichier"> le fichier en quelque sorte </param>
        /// <returns></returns>
        public  string UploadFile(string chemin, string name, IFormFile fichier)
        {
            using (var fileStream = new FileStream(Path.Combine(chemin, name), FileMode.Create, FileAccess.Write))
            {
                fichier.CopyTo(fileStream);
            }
            return Path.Combine(chemin, name);
        }
        /// <summary>
        /// Télecharger un fichier avec un dossier par défaut.
        /// </summary>
        /// <param name="fichier">le fichier à télécharger</param>
        public  string UploadFileDefault(FormFile fichier)
        {
            var ext = Path.GetExtension(fichier.FileName);
            // verif extension file
            if (ExtensionsAutorises.Contains(ext))
            {
                // verif is directory  exists
                if (Directory.Exists(DefaultDirectory))
                {
                    using (var fileStream = new FileStream(Path.Combine(DefaultDirectory, fichier.FileName), FileMode.Create, FileAccess.Write))
                    {
                        fichier.CopyTo(fileStream);
                    }
                    return Path.Combine(DefaultDirectory, fichier.FileName);
                }
                else
                {
                    Console.WriteLine("Directory not found ");
                    return null;
                }
            }
            return null;
        }
        /// <summary>
        /// Supprimer un fichier en donnant le chemin(path).
        /// </summary>
        /// <param name="chemin">le chemin du fichier</param>
        public  void DeleteFile(string chemin)
        {
            if (File.Exists(chemin))
            {
                File.Delete(chemin);
            }
        }
        // convert IFormFile by byte[]
        /// <summary>
        /// Convertir un fichier en tableau de bytes.
        /// </summary>
        /// <param name="file">le fichier</param>
        /// <returns>Une action résultant un tableau de bytes</returns>
        public  async Task<byte[]> ConvertToBytes(IFormFile file)
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
        /// <summary>
        /// Obtenir un tableau de bytes d'un fichier en connaissant son chemin.
        /// </summary>
        /// <param name="chemin">le chemin du fichier</param>
        /// <returns></returns>
        public  byte[] GetBytesByPath(string chemin)
        {
            try
            {
                var result = File.ReadAllBytes(chemin);
                return result;
            }
            catch (Exception ex)
            {
                var byteTest = new byte[0];
                return byteTest;
                //throw;
            }
        }
        public bool UploadToTempFolder(byte[] pFileBytes, string pFileName, string pathFolder)
        {
            bool isSuccess = false;
            FileStream fileStream = null;

            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }
                if (!string.IsNullOrEmpty(pathFolder))
                {
                    if (!string.IsNullOrEmpty(pFileName))
                    {
                        string strFileFullPath = pathFolder + "//" + pFileName;
                        fileStream = new FileStream(strFileFullPath, FileMode.OpenOrCreate);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(pFileBytes, 0, pFileBytes.Length);
                            isSuccess = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //throw ex;
                WriteLogSystem(ex.ToString(), "UploadToTempFolder");

            }

            return isSuccess;
        }

        public byte[] FichierVersTableauDeByte(string CheminFichier)
        {
            FileInfo MonFichier = new FileInfo(CheminFichier);
            try
            {
                if (MonFichier.Length > 0)
                {
                    FileStream MonFileStream = MonFichier.OpenRead();
                    byte[] TableauDeBytes = new byte[(int)MonFileStream.Length];
                    // On charge le fichier dans un tableau de byte
                    MonFileStream.Read(TableauDeBytes, 0, (int)MonFileStream.Length);
                    // On ferme le stream
                    MonFileStream.Close();
                    return TableauDeBytes;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        ///This method will give you the file content as a byte.  

        public byte[] GetFileFromFolder(string filename)
        {
            byte[] filedetails = new byte[0];
            string strTempFolderPath = System.Configuration.ConfigurationManager.AppSettings.Get("FileUploadPath");
            if (File.Exists(strTempFolderPath + filename))
            {
                return File.ReadAllBytes(strTempFolderPath + filename);
            }
            else return filedetails;
        }

        public bool fileExistOnFolder(string path, string idDossier)
        {
            bool rep = false;
            DirectoryInfo d = new DirectoryInfo(@"E:\ImpressionDocumentOrbus\ZONEDECHANGE");
            FileInfo[] Files = d.GetFiles("*.zip");
            foreach (FileInfo file in Files)
            {
                if (file.Name.StartsWith(idDossier.ToString()))
                {
                    rep = true;
                }
            }
            return rep;
        }

        //public RemoteFileInfo DownloadFile(DownloadRequest request)
        //{
        //    RemoteFileInfo result = new RemoteFileInfo();
        //    try
        //    {
        //        string filePath = System.IO.Path.Combine(@"c:\Uploadfiles", request.FileName);
        //        System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

        //        // check if exists
        //        if (!fileInfo.Exists)
        //            throw new System.IO.FileNotFoundException("File not found",
        //                                                      request.FileName);

        //        // open stream
        //        System.IO.FileStream stream = new System.IO.FileStream(filePath,
        //                  System.IO.FileMode.Open, System.IO.FileAccess.Read);

        //        // return result 
        //        result.FileName = request.FileName;
        //        result.Length = fileInfo.Length;
        //        result.FileByteStream = stream;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return result;
        //}

        //public void UploadFile(RemoteFileInfo request)
        //{
        //    FileStream targetStream = null;
        //    Stream sourceStream = request.FileByteStream;

        //    string uploadFolder = @"C:\upload\";

        //    string filePath = Path.Combine(uploadFolder, request.FileName);

        //    using (targetStream = new FileStream(filePath, FileMode.Create,
        //                          FileAccess.Write, FileShare.None))
        //    {
        //        //read from the input stream in 65000 byte chunks

        //        const int bufferLen = 65000;
        //        byte[] buffer = new byte[bufferLen];
        //        int count = 0;
        //        while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
        //        {
        //            // save to output stream
        //            targetStream.Write(buffer, 0, count);
        //        }
        //        targetStream.Close();
        //        sourceStream.Close();
        //    }

        //}
        public bool TableauDeByteVersFicher(string CheminRep, string CheminFichier, byte[] TableauDeByte)
        {


            bool resultOK = false;

            try
            {
                if (!Directory.Exists(CheminRep))
                {
                    Directory.CreateDirectory(CheminRep);
                }
                string filePath = CheminRep + CheminFichier;
                //if (File.Exists(CheminFichier))
                if (File.Exists(filePath))
                {

                }

                using (System.IO.FileStream MonFileStream = new System.IO.FileStream(CheminRep + "\\" + CheminFichier, System.IO.FileMode.Create))
                {
                    MonFileStream.Write(TableauDeByte, 0, TableauDeByte.Length - 1);
                    resultOK = true;
                };

            }
            catch (Exception ex)
            {

                WriteLogSystem(ex.ToString(), "TableauDeByteVersFicher");
            }

            return resultOK;

        } //' fonction qui enregistre un tableau de d'octet dans un fichier

        public static void WriteLogSystem(string erreur, string fonction)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "ServiceFileEndev";
                eventLog.WriteEntry(string.Format("date: {0}, fonction: {1}, description {2}", DateTime.Now, fonction, erreur), EventLogEntryType.Information, 101, 1);
            }
        }

        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="Message"></param>  
        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }

}