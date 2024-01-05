using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SIGRHFile
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: ajoutez vos opérations de service ici
        [OperationContract]
        string UploadFile(string chemin, string name, IFormFile fichier);
        [OperationContract]
        string UploadFileDefault(FormFile fichier);
        [OperationContract]
        void DeleteFile(string chemin);
        [OperationContract]
        Task<byte[]> ConvertToBytes(IFormFile file);
        [OperationContract]
        byte[] GetBytesByPath(string chemin);
        [OperationContract]
        bool UploadToTempFolder(byte[] pFileBytes, string pFileName, string pathFolder);

        [OperationContract]
        byte[] FichierVersTableauDeByte(string CheminFichier);

        [OperationContract]
        bool TableauDeByteVersFicher(string CheminRep, string CheminFichier, byte[] TableauDeByte);

    }


    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
