using SIGRHMetier.Application.Filters;
using SIGRHMetier.Application.ViewModel;
using SIGRHMetier.Model;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using static SIGRHMetier.Application.ViewModel.ActeAdminViewModel;

namespace SIGRHMetier
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);


        //---------Profils---------
        [OperationContract]
        bool UpdateProfil(string codeProfil, TP_Profil profil);
        [OperationContract]
        TP_Profil GetProfilByCode(string codeProfil);
        [OperationContract]
        TP_Profil GetProfilById(int id);
        [OperationContract]
        TP_Profil ActiverOuDesactiverProfil(string codeProfil);
        [OperationContract]
        IEnumerable<TP_Profil> GetListProfils();


        //---------menus---------
        [OperationContract]
        ICollection<TD_MenuItem> GetAllMenu(string codeProfil);


        //---------Utilisateurs---------
        [OperationContract]
        IEnumerable<UserViewModel> GetAllUsers(GetAllUserFilter filter);
        [OperationContract]
        bool AddUser(TD_Utilisateur user);
        [OperationContract]
        bool UpdateUser(int idUser, TD_Utilisateur user);
        [OperationContract]
        bool UpdateUserByParent(string idUser, TD_Utilisateur user);
        [OperationContract]
        bool DeleteUser(int idUser);
        [OperationContract]
        TD_Utilisateur GetOneUser(int idUser);
        [OperationContract]
        UserViewModel GetUserByEmail(string email);
        [OperationContract]
        TD_Utilisateur GetOneUserByLogin(string login);

        [OperationContract]
        TD_Utilisateur GetOneUserByParent(string idUserParent);
        [OperationContract]
        UserViewModel GetOneUserViewByParent(string idUserParent);
        [OperationContract]
        TD_Utilisateur ActiverOuDesactiverUser(string idUser);
        [OperationContract]
        bool VerifUserByProfil(string codeProfil);
        [OperationContract]
        bool revokeUser(TD_Utilisateur user);

        //---------Registre Personnel et echelonnage---------
        [OperationContract]
        List<RegistreViewModel> RegistrePersonnel(GetAllRegistreFilter filter);

        [OperationContract]
        string GetGradeByCode(string Code, string TypePersonnel);

        [OperationContract]
        string GetJuridictionByCode(string CodeFonc);

        [OperationContract]
        TP_Fonction GetFoncByCode(string CodeFonction);

        [OperationContract]
        TP_TypePersonnel GetPersByCode(string codePers);

        [OperationContract]
        bool AddFicheMagistrat(PersonnelViewModel fiche);

        [OperationContract]
        bool AddFicheFonctionnaire(TD_FichePersonnelJudiciaire fiche);
        [OperationContract]
        bool DeleteFiche(int id);

        [OperationContract]
        bool UpdateFiche(int idFiche, TD_FichePersonnelJudiciaire fiche);

        [OperationContract]
        bool ValiderFiche(int idFiche, int idUser);

        [OperationContract]
        bool VerifFicheByMatricule(string matricule);

        [OperationContract]
        bool VerifFicheByTelephone(string telephone);

        [OperationContract]
        bool VerifFicheByEmail(string email);
        [OperationContract]
        bool VerifFicheByCni(string cni);

        [OperationContract]
        TD_FichePersonnelJudiciaire GetOneFicheMagistrat(int idFiche);

        [OperationContract]
        TD_FichePersonnelJudiciaire GetOneFiche(int idFiche);
        [OperationContract]
        FichePersonnelViewModel GetFonctionnaireDetail(int idFiche);

        #region Gestion Acte Administration
        [OperationContract]
        IEnumerable<ActeAdminViewModel> GetListeActeAdministration(GetAllActesAdministrationFilter filter);
        [OperationContract]
        bool AddActeAdministration(AddOrUpdatecteAdminViewModel acte);


        [OperationContract]
        bool UpdateActeAdministration(int id, AddOrUpdatecteAdminViewModel acte);
        //  [OperationContract]
        // IEnumerable<ActeAdminViewModel> GetListeActeAdministration();

        [OperationContract]
        TD_ActeAdminstration GetActeAdministrationByCode(int id);

        [OperationContract]
        bool DeleteActeAdministration(int id);
        #endregion

        #region Table de parametrages (TP)
        [OperationContract]
        IEnumerable<TP_CorpsJudiciaire> GetAllCorpsJudiciaire(GetAllCorpsFilter filter);
        [OperationContract]
        IEnumerable<TP_FonctionFonctionnaireJustice> GetAllFonctionFonctionnaire(GetAllFonctionFonctionnaireFilter filter);
        [OperationContract]
        IEnumerable<TP_Region> GetAllRegion(GetAllRegionFilter filter);
        [OperationContract]
        IEnumerable<TP_Departement> GetAllDepartement(GetAllDepartementFilter filter);
        [OperationContract]
        IEnumerable<TP_Commune> GetAllCommune(GetAllCommuneFilter filter);
        [OperationContract]
        IEnumerable<JuridictionViewModel> GetAllJuridiction(GetAllJuridictionFilter filter);
        [OperationContract]
        IEnumerable<TP_Quartier> GetAllQuartier(GetAllQuartierFilter filter);
        [OperationContract]
        IEnumerable<TP_ClasseJuridiction> GetAllClasseJuridiction(GetAllClasseJuridictionFilter filter);
        [OperationContract]
        IEnumerable<TP_Echelon> GetAllEchelon(GetAllEchelonFilter filter);
        [OperationContract]
        IEnumerable<TP_ModePassage> GetAllModePassage(GetAllModePassageFilter filter);
        [OperationContract]
        IEnumerable<TP_NatureDecision> GetAllNatureDecision(GetAllNatureDecisionFilter filter);
        [OperationContract]
        IEnumerable<TP_TypePersonnel> GetAllTypePersonnel(GetAllTypePersonnelFilter filter);
        [OperationContract]
        IEnumerable<TP_EmploiJudiciaire> GetAllEmploiJudiciaire(GetAllEmploiJudiciaireFilter filter);
        [OperationContract]
        IEnumerable<TP_GradeFonctionnaireJustice> GetAllGradeFoncJustice(GetAllGradeFoncJusticeFilter filter);
        [OperationContract]
        IEnumerable<TP_GradeMagistrat> GetAllGradeMagistrat(GetAllGradeMagistratFilter filter);
        [OperationContract]
        IEnumerable<TP_Groupe> GetAllGroupe(GetAllGroupeFilter filter);
        [OperationContract]
        IEnumerable<TP_Indice> GetAllIndice(GetAllIndiceFilter filter);
        [OperationContract]
        IEnumerable<TP_TypeDocument> GetAllTypeDocument(GetAllTypeDocumentFilter filter);
        [OperationContract]
        IEnumerable<TP_TypeJuridiction> GetAllTypeJuridiction(GetAllTypeJuridctionFilter filter);
        [OperationContract]
        IEnumerable<TP_Fonction> GetAllFonction(GetAllFonctionFilter filter);
        #endregion

        //---------Natures décision---------
        [OperationContract]
        bool AddNatureDecision(NatureDecisionViewModel natureDecisionView);

        [OperationContract]
        bool UpdateNatureDecision(NatureDecisionViewModel natureDecisionView);

        [OperationContract]
        NatureDecisionViewModel GetNatureDecision(int idActeGestion);

        [OperationContract]
        IEnumerable<NatureDecisionViewModel> GetAllNaturesDecision();

        [OperationContract]
        bool DeleteNatureDecision(int idNatureDecision);

        //---------Type document---------
        [OperationContract]
        bool AddTypeDocument(TypeDocumentViewModel typeDocumentView);

        [OperationContract]
        bool UpdateTypeDocument(TypeDocumentViewModel typeDocumentView);

        [OperationContract]
        TypeDocumentViewModel GetTypeDocument(int idTypeDocument);

        [OperationContract]
        IEnumerable<TypeDocumentViewModel> GetAllTypesDocument();

        [OperationContract]
        bool DeleteTypeDocument(int idTypeDocument);

        //---------Actes de gestion---------
        [OperationContract]
        bool AddActeGestion(ActeGestionCreationViewModel acteGestion);

        [OperationContract]
        bool UpdateActeGestion(int idActeGestion, ActeGestionModificationViewModel acteGestion);

        [OperationContract]
        ActeGestionConsultationViewModel GetActeGestion(int idActeGestion);

        [OperationContract]
        IEnumerable<ActeGestionConsultationViewModel> GetAllActesGestion(GetAllActesGestionFilter filter);

        [OperationContract]
        bool DeleteActeGestion(int idActeGestion);

        [OperationContract]
        bool SoumettreActeGestion(int idActeGestion);

        [OperationContract]
        bool ValiderActeGestion(int idActeGestion, int utiValidation);

        [OperationContract]
        bool InvaliderActeGestion(int idActeGestion, int utiInvalidation);
        // *------------------- Signature_Photo ------------------*
        [OperationContract]
        TD_Signature_Photo AddSignaturePhoto(TD_Signature_Photo signaturePhoto);
        [OperationContract]
        bool UpdateSignaturePhoto(int id, TD_Signature_Photo signaturePhoto);
        [OperationContract]
        bool DeleteSignaturePhoto(int id);

        //---------Fichiers--------
        [OperationContract]
        TD_Fichier GetFichier(int idFichier);


        //---------Logging---------
        [OperationContract]
		void WriteDataError(string page, string fonction, string erreur); 


        // TODO: ajoutez vos opérations de service ici
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
