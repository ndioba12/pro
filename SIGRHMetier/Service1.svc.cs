using SIGRHMetier.Application.Filters;
using SIGRHMetier.Application.ViewModel;
using SIGRHMetier.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static SIGRHMetier.Application.ViewModel.ActeAdminViewModel;

namespace SIGRHMetier
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class Service1 : IService1
    {
        BdJUSTICEEntities db = new BdJUSTICEEntities();
        Utils utils = new Utils();
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

        public void WriteDataError(string page, string fonction, string erreur)
        {
            utils.WriteDataError(page, fonction, erreur);
        }
        public bool UpdateProfil(string codeProfil, TP_Profil profil)
        {
            try
            {
                var profilExist = db.TP_Profil.Where(item => item.Pro_Code == codeProfil).FirstOrDefault();
                if (profilExist != null)
                {
                    profilExist.Pro_Libelle = profil.Pro_Libelle;
                    db.SaveChanges();
                    return true;
                }
                WriteDataError(this.GetType().Name, "UpdateProfil", "ce profil n'existe pas");
                return false;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "UpdateProfil", ex.ToString());
                return false;
                //throw;
            }
        }

        public TP_Profil GetProfilByCode(string codeProfil)
        {
            return db.TP_Profil.Where(item => item.Pro_Code == codeProfil).FirstOrDefault();
        }
        public TP_Profil GetProfilById(int id)
        {
            return db.TP_Profil.Where(item => item.Pro_Id == id).FirstOrDefault();
        }

        public TP_Profil ActiverOuDesactiverProfil(string codeProfil)
        {
            try
            {
                var profilExist = db.TP_Profil.Where(item => item.Pro_Code == codeProfil && item.Pro_Code != "AD").FirstOrDefault();
                if (profilExist != null)
                {
                    profilExist.Pro_ActifOuiNon = profilExist.Pro_ActifOuiNon == "1" ? "0" : "1";
                    db.SaveChanges();
                    return profilExist;
                }
                WriteDataError(this.GetType().Name, "ActiverOuDesactiverProfil", "ce profil n'existe pas");
                return null;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "ActiverOuDesactiverProfil", ex.ToString());
                return null;
                //throw;
            }
        }

        public bool DesactiverProfil(string codeProfil)
        {
            try
            {
                var profilExist = db.TP_Profil.Where(item => item.Pro_Code == codeProfil).FirstOrDefault();
                if (profilExist != null)
                {
                    profilExist.Pro_ActifOuiNon = "0";
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "DesactiverProfil", ex.ToString());
                return false;
                //throw;
            }
        }
        /// <summary>
        /// Get all profils.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TP_Profil> GetListProfils()
        {
            return db.TP_Profil.ToList().Where(o => o.Pro_Code != "AD" && o.Pro_Code != "REV");
        }
        /// <summary>
        /// Récupérer tous les menus à partir de son code profil.
        /// </summary>
        /// <param name="codeProfil"></param>
        /// <returns></returns>
        public ICollection<TD_MenuItem> GetAllMenu(string codeProfil)
        {
            var result = db.TD_MenuItem
                .Where(item => item.Men_CodeProfil.Equals(codeProfil, StringComparison.OrdinalIgnoreCase))
                .OrderBy(item => item.Men_Priorite)
                .ToList();

            return result;
        }
        /// <summary>
        /// Récupérer tous les compléments  d'utilisateurs de l'application.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserViewModel> GetAllUsers(GetAllUserFilter filter)
        {
            var queryProfil = db.TP_Profil.AsQueryable();
            var queryUser = db.TD_Utilisateur.AsQueryable();
            // filtres by user 
            if (filter.Nom != string.Empty && filter.Nom != null)
            {
                queryUser = queryUser.Where(item => item.Uti_Nom.Contains(filter.Nom));
            }
            if (filter.Prenom != string.Empty && filter.Prenom != null)
            {
                queryUser = queryUser.Where(item => item.Uti_Prenom.Contains(filter.Prenom));
            }
            if (filter.Profil != string.Empty && filter.Profil != null)
            {
                queryUser = queryUser.Where(item => item.Uti_Pro_Code.Equals(filter.Profil, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.Identifiant != string.Empty && filter.Identifiant != null)
            {
                queryUser = queryUser.Where(item => item.Uti_Login.Equals(filter.Identifiant, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.Statut != string.Empty && filter.Statut != null)
            {
                queryUser = queryUser.Where(item => item.Uti_ActifOuiNon.Equals(filter.Statut, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.DateCreation != null && filter.DateCreation != DateTime.MinValue)
            {
                queryUser = queryUser.Where(item => DbFunctions.TruncateTime(item.Uti_DateCreation) == filter.DateCreation.Date);
            }
            // profils and users
            var all = from user in queryUser
                      join profil in queryProfil on user.Uti_Pro_Code equals profil.Pro_Code
                      where user.Uti_Login != "adminsigrh"
                      orderby user.Uti_DateCreation descending
                      select new UserViewModel
                      {
                          Id = user.Uti_Id,
                          Prenom = user.Uti_Prenom,
                          Nom = user.Uti_Nom,
                          Adresse = user.Uti_Adresse,
                          Email = user.Uti_Email,
                          Login = user.Uti_Login,
                          Statut = user.Uti_ActifOuiNon,
                          Poste = user.Uti_Poste,
                          DateCreation = (DateTime)user.Uti_DateCreation,
                          IdUser = user.Uti_idUser,
                          Telephone = user.Uti_Telephone,
                          LibelleProfil = profil.Pro_Libelle,
                          CodeProfil = profil.Pro_Code,
                          IdProfil = profil.Pro_Id,
                      };

            return all;
        }
        /// <summary>
        /// Ajouter un nouveau utilisateur.
        /// </summary>
        /// <param name="user">utilisateur à ajouter</param>
        /// <returns></returns>
        public bool AddUser(TD_Utilisateur user)
        {
            try
            {
                // verif doublon
                var exist = db.TD_Utilisateur
                    .Where(item => item.Uti_Login.Equals(user.Uti_Login, StringComparison.OrdinalIgnoreCase) || item.Uti_Email.Equals(user.Uti_Email, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                if (exist != null) return false;
                // verif roles by users
                db.TD_Utilisateur.Add(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "AddUser", ex.ToString());
                return false;
                //throw;
            }
        }
        /// <summary>
        /// Mettre à jour un utilisateur.
        /// </summary>
        /// <param name="user">utilisateur à modifier</param>
        /// <param name="idUser">identifiant utilisateur</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool UpdateUser(int idUser, TD_Utilisateur user)
        {
            try
            {
                var userExist = db.TD_Utilisateur.Where(item => item.Uti_Id == idUser).FirstOrDefault();
                if (userExist != null)
                {
                    // mettre à jour
                    userExist.Uti_Telephone = user.Uti_Telephone;
                    userExist.Uti_Poste = user.Uti_Poste;
                    userExist.Uti_Prenom = user.Uti_Prenom;
                    userExist.Uti_Adresse = user.Uti_Adresse;
                    userExist.Uti_Nom = user.Uti_Nom;
                    if(!userExist.Uti_Pro_Code.Equals(user.Uti_Pro_Code))
                    {
                        userExist.Uti_Pro_Code = user.Uti_Pro_Code ?? userExist.Uti_Pro_Code;
                        userExist.Uti_ActifOuiNon = userExist.Uti_Pro_Code.Equals("REV") ? "0" : "1";
                        if (!userExist.Uti_Pro_Code.Equals("CP") && !userExist.Uti_Pro_Code.Equals("OPS") && !userExist.Uti_Pro_Code.Equals("REV"))
                        {
                            var userWithSameProfile = db.TD_Utilisateur.Where(item => item.Uti_Pro_Code == userExist.Uti_Pro_Code).FirstOrDefault();
                            if (userWithSameProfile != null && userWithSameProfile.Uti_Id != userExist.Uti_Id)
                            {
                                userWithSameProfile.Uti_Pro_Code = "REV";
                                userWithSameProfile.Uti_ActifOuiNon = "0";
                            }
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
                WriteDataError(this.GetType().Name, "UpdateUser", "cet utilisateur n'existe pas");
                return false;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "UpdateUser", ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Mettre à jour un utilisateur.
        /// </summary>
        /// <param name="user">utilisateur à modifier</param>
        /// <param name="idUser">identifiant utilisateur</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool UpdateUserByParent(string idUser, TD_Utilisateur user)
        {
            try
            {
                var userExist = db.TD_Utilisateur.Where(item => item.Uti_idUser == idUser).FirstOrDefault();
                if (userExist != null)
                {
                    // mettre à jour
                    userExist.Uti_Telephone = user.Uti_Telephone;
                    userExist.Uti_Poste = user.Uti_Poste;
                    userExist.Uti_Prenom = user.Uti_Prenom;
                    userExist.Uti_Adresse = user.Uti_Adresse;
                    userExist.Uti_Nom = user.Uti_Nom;
                    if (!userExist.Uti_Pro_Code.Equals(user.Uti_Pro_Code))
                    {
                        userExist.Uti_Pro_Code = user.Uti_Pro_Code ?? userExist.Uti_Pro_Code;
                        userExist.Uti_ActifOuiNon = userExist.Uti_Pro_Code.Equals("REV") ? "0" : "1";
                        if (!userExist.Uti_Pro_Code.Equals("CP") && !userExist.Uti_Pro_Code.Equals("OPS") && !userExist.Uti_Pro_Code.Equals("REV"))
                        {
                            var userWithSameProfile = db.TD_Utilisateur.Where(item => item.Uti_Pro_Code == userExist.Uti_Pro_Code).FirstOrDefault();
                            if (userWithSameProfile != null && userWithSameProfile.Uti_Id != userExist.Uti_Id)
                            {
                                userWithSameProfile.Uti_Pro_Code = "REV";
                                userWithSameProfile.Uti_ActifOuiNon = "0";
                            }
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
                WriteDataError(this.GetType().Name, "UpdateUser", "cet utilisateur n'existe pas");
                return false;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "UpdateUser", ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Supprimer un utilisateur par son identifiant.
        /// </summary>
        /// <param name="idUser">identifiant utilisateur</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool DeleteUser(int idUser)
        {
            var user = db.TD_Utilisateur.Where(item => item.Uti_Id == idUser).FirstOrDefault();
            if (user != null)
            {
                db.TD_Utilisateur.Remove(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Obtenir un utilisateur par son identifiant.
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TD_Utilisateur GetOneUser(int idUser)
        {
            return db.TD_Utilisateur.Where(item => item.Uti_Id == idUser).FirstOrDefault();
        }
        /// <summary>
        /// Obtenir le complément d'utilisateur et son profil à partir de son identifiant parent.
        /// </summary>
        /// <param name="idUserParent">identifiant utilisateur parent</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public UserViewModel GetOneUserViewByParent(string idUserParent)
        {
            try
            {
                var queryProfil = db.TP_Profil.AsQueryable();
                var queryUser = db.TD_Utilisateur.AsQueryable();

                var theUser = (from user in queryUser
                               join profil in queryProfil on user.Uti_Pro_Code equals profil.Pro_Code
                               where user.Uti_idUser == idUserParent
                               select new UserViewModel
                               {
                                   Id = user.Uti_Id,
                                   Prenom = user.Uti_Prenom,
                                   Nom = user.Uti_Nom,
                                   Adresse = user.Uti_Adresse,
                                   Email = user.Uti_Email,
                                   Login = user.Uti_Login,
                                   Statut = user.Uti_ActifOuiNon,
                                   Poste = user.Uti_Poste,
                                   DateCreation = (DateTime)user.Uti_DateCreation,
                                   IdUser = user.Uti_idUser,
                                   Telephone = user.Uti_Telephone,
                                   LibelleProfil = profil.Pro_Libelle,
                                   CodeProfil = profil.Pro_Code,
                                   IdProfil = profil.Pro_Id,
                               }).FirstOrDefault();

                return theUser;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetOneUserByParent", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Obtenir le complément d'utilisateur à partir de son identifiant parent.
        /// </summary>
        /// <param name="idUserParent">identifiant utilisateur parent</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TD_Utilisateur GetOneUserByParent(string idUserParent)
        {
            try
            {
                TD_Utilisateur user = db.TD_Utilisateur.Where(item => item.Uti_idUser == idUserParent).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetOneUserByParent", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Obtenir le complément d'utilisateur à partir de son identifiant parent.
        /// </summary>
        /// <param name="idUserParent">identifiant utilisateur parent</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TD_Utilisateur GetOneUserByLogin(string login)
        {
            try
            {
                TD_Utilisateur user = db.TD_Utilisateur.Where(item => item.Uti_Login.Equals(login, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetOneUserByParent", ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Cette méthode permet de désactiver ou d'activer un utilisateur.
        /// </summary>
        /// <param name="idUserParent">Identifiant du parent</param>
        /// <returns></returns>
        public TD_Utilisateur ActiverOuDesactiverUser(string idUserParent)
        {
            try
            {
                var userlExist = db.TD_Utilisateur.Where(item => item.Uti_idUser == idUserParent && item.Uti_Login != "adminsigrh").FirstOrDefault();
                if (userlExist != null)
                {
                    if (userlExist.Uti_Pro_Code == "REV" && userlExist.Uti_ActifOuiNon.Equals("0"))
                    {
                        return null;
                    }
                    userlExist.Uti_ActifOuiNon = userlExist.Uti_ActifOuiNon == "1" ? "0" : "1";
                    db.SaveChanges();
                    return userlExist;
                }
                WriteDataError(this.GetType().Name, "ActiverOuDesactiverUser", "Cet utilisateur n'existe pas");
                return null;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "ActiverOuDesactiverUser", ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Cette méthode permet de récupérer un utilisateur par son email.
        /// </summary>
        /// <param name="email">email utilisateur</param>
        /// <returns></returns>
        public UserViewModel GetUserByEmail(string email)
        {
            return db.TD_Utilisateur
            .Where(item => item.Uti_Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            .Select(item => new UserViewModel
            {
                Adresse = item.Uti_Adresse,
                CodeProfil = item.Uti_Pro_Code,
                DateCreation = (DateTime)item.Uti_DateCreation,
                Email = item.Uti_Email,
                Id = item.Uti_Id,
                IdUser = item.Uti_idUser,
                Nom = item.Uti_Nom,
                Prenom = item.Uti_Prenom,
                Statut = item.Uti_ActifOuiNon,
                Login = item.Uti_Login,
                Poste = item.Uti_Poste,
                Telephone = item.Uti_Telephone
            }).FirstOrDefault();
        }
        /// <summary>
        /// Vérifier s'il existe un utilisateur avec un profil donné.
        /// </summary>
        /// <param name="codeProfil">code profil</param>
        /// <returns></returns>
        /// 
                #region  Gestion Acte Administration
        //Liste Actes

        public IEnumerable<ActeAdminViewModel> GetListeActeAdministration(GetAllActesAdministrationFilter filter)

        {
            try
            {
               
                var queryActeAdmin = db.TD_ActeAdminstration.Where(a=>a.Ata_NumeroDecision!=null);
              /*  var t = db.TD_ActeAdminstration

               .Select(acte => new ActeAdminViewModel
               {
                   Id = acte.Ata_Id,
                   LibelleTypeDocument = db.TP_TypeDocument.Where(tp => tp.TyD_Code == acte.Ata_TyD_Code).FirstOrDefault().TyD_Libelle,
                   //  Grade = gradeM.Grm_Libelle!=null && !gradeM.Grm_Libelle.Equals("") ? gradeM.Grm_Libelle: gradeF.Grf_Libelle, 
                   //  Grade = graM.Where(g => g.Grm_Code == acte.Ata_Grm_Code) != null ? graM.Where(g => g.Grm_Code == acte.Ata_Grm_Code).FirstOrDefault().Grm_Libelle : graF.Where(g => g.Grf_Code == acte.Ata_Grf_Code).FirstOrDefault().Grf_Libelle,
                   Grade = graM.Where(g => g.Grm_Code == acte.Ata_Grm_Code).FirstOrDefault().Grm_Libelle,
                   LibelleGroupe = db.TP_Groupe.Where(g => g.Gro_Code == acte.Ata_Gro_Code).FirstOrDefault().Gro_Libelle,
                   LibelleEchelon = db.TP_Echelon.Where(c => c.Ech_Code == acte.Ata_Ech_Code).FirstOrDefault().Ech_Libelle,
                   ValeurIndice = acte.Ata_Ind_Valeur,
                   DateCreation = acte.Ata_DateCreation,
                   DateSoumission = acte.Ata_DateSoumission,
               }).OrderByDescending(d => d.DateCreation).ToList();*/

                /*   if ((filter.DateEntreeEnVigueurDebut != null && !filter.DateEntreeEnVigueurDebut.Equals(DateTime.MinValue)))
                   {
                       queryActeAdmin = queryActeAdmin.Where(item => item.Ata_DateValidation >= filter.DateEntreeEnVigueurDebut);
                   }
                   if ((filter.DateEntreeEnVigueurFin != null && !filter.DateEntreeEnVigueurFin.Equals(DateTime.MinValue)))
                   {
                       queryActeAdmin = queryActeAdmin.Where(item => item.Ata_DateValidation <= filter.DateEntreeEnVigueurFin);
                   }*/
                if (filter.CodeTypeDocument != null && !filter.CodeTypeDocument.Equals(string.Empty))
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_TyD_Code.Equals(filter.CodeTypeDocument));
                }
                if (filter.CodeGroupe != null && !filter.CodeGroupe.Equals(string.Empty))
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_Gro_Code.Equals(filter.CodeGroupe));
                }
                if (filter.ValeurIndice != 0)
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_Ind_Valeur == filter.ValeurIndice);
                }
                if (filter.CodeEchelon != null && !filter.CodeEchelon.Equals(string.Empty))
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_Ech_Code.Equals(filter.CodeEchelon));
                }
                if (filter.CodeGradeM != null && !filter.CodeGradeM.Equals(string.Empty))
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_Grm_Code.Equals(filter.CodeGradeM));
                }
                if (filter.CodeGradeF != null && !filter.CodeGradeF.Equals(string.Empty))
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_Grf_Code.Equals(filter.CodeGradeF));
                }
                if (filter.CodeNatureDecision != null && !filter.CodeNatureDecision.Equals(string.Empty))
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_Nad_Code.Equals(filter.CodeNatureDecision));
                }

                if ((filter.DateCreation != null && !filter.DateCreation.Equals(DateTime.MinValue)))
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_DateCreation == filter.DateCreation);
                }
                if ((filter.DateSoummission != null && !filter.DateSoummission.Equals(DateTime.MinValue)))
                {
                    queryActeAdmin = queryActeAdmin.Where(item => item.Ata_DateCreation == filter.DateSoummission);
                }
                var graM = db.TP_GradeMagistrat;
                var graF = db.TP_GradeFonctionnaireJustice;
                var result = queryActeAdmin.Select(acte => new ActeAdminViewModel
            {
                Id = acte.Ata_Id,
                LibelleTypeDocument = db.TP_TypeDocument.Where(tp => tp.TyD_Code == acte.Ata_TyD_Code).FirstOrDefault().TyD_Libelle,
                    // Grade = graM.Where(g => g.Grm_Code == acte.Ata_Grm_Code) != null ? graM.Where(g => g.Grm_Code == acte.Ata_Grm_Code).FirstOrDefault().Grm_Libelle : graF.Where(g => g.Grf_Code == acte.Ata_Grf_Code).FirstOrDefault().Grf_Libelle,
                    Grade = graM.Where(g => g.Grm_Code == acte.Ata_Grm_Code).FirstOrDefault().Grm_Libelle,
                    LibelleGroupe = db.TP_Groupe.Where(g => g.Gro_Code == acte.Ata_Gro_Code).FirstOrDefault().Gro_Libelle,
                LibelleEchelon = db.TP_Echelon.Where(c => c.Ech_Code == acte.Ata_Ech_Code).FirstOrDefault().Ech_Libelle,
                ValeurIndice = acte.Ata_Ind_Valeur,
                DateCreation = acte.Ata_DateCreation,
                DateSoumission = acte.Ata_DateSoumission,
            }).OrderByDescending(d => d.DateCreation).ToList();
                /*  var queryEchelon = db.TP_Echelon.AsQueryable();
                  var queryIndice = db.TP_Indice.AsQueryable();
                  var queryGroupe = db.TP_Groupe.AsQueryable();
                  var queryGradeMagistrat = db.TP_GradeMagistrat.AsQueryable();
                  var queryGradeFonc = db.TP_GradeFonctionnaireJustice.AsQueryable();
                  var queryTypeDocument = db.TP_TypeDocument.AsQueryable();
                  //var queryFichePersonnel = db.TD_FichePersonnelJudiciaire.AsQueryable();
                  var result = (from acte in queryActeAdmin
                                join typD in queryTypeDocument on acte.Ata_TyD_Code equals typD.TyD_Code
                                join echelon in queryEchelon on acte.Ata_Ech_Code equals echelon.Ech_Code
                               join groupe in queryGroupe on acte.Ata_Gro_Code equals groupe.Gro_Code
                               join indice in queryIndice on acte.Ata_Ind_Valeur equals indice.Ind_Valeur
                               join gradeM in queryGradeMagistrat on acte.Ata_Grm_Code equals gradeM.Grm_Code
                               join gradeF in queryGradeFonc on acte.Ata_Grf_Code equals gradeF.Grf_Code
                               select new ActeAdminViewModel
                               {
                                   Id = acte.Ata_Id,
                                   LibelleTypeDocument=typD.TyD_Libelle,
                                   //  Grade = gradeM.Grm_Libelle!=null && !gradeM.Grm_Libelle.Equals("") ? gradeM.Grm_Libelle: gradeF.Grf_Libelle, 
                                   Grade = gradeM.Grm_Libelle,
                                   LibelleGroupe = groupe.Gro_Libelle,
                                   LibelleEchelon = echelon.Ech_Libelle,
                                   ValeurIndice=indice.Ind_Valeur,
                                   DateCreation = acte.Ata_DateCreation,
                                   DateSoumission = acte.Ata_DateSoumission,

                               });*/

                return result.ToList();

            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetActeAdministration", ex.ToString());
                return null;
            }
        }
        //Ajouter Acte
        public bool AddActeAdministration(AddOrUpdatecteAdminViewModel acte)
        {
            /*try
            {
                // verif doublon
                var exist = db.TD_ActeAdminstration
                    .Where(item => item.Ata_NumeroDecision.Equals(acte.Ata_NumeroDecision, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                if (exist != null) return false;
                db.TD_ActeAdminstration.Add(acte);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "AddActe", ex.ToString());
                return false;
                //throw;
            }*/
            var tra = db.Database.BeginTransaction();
            try
            {
                var fpjExist = db.TD_FichePersonnelJudiciaire.Where(item => item.FPJ_Id == acte.IdFichePersonnelJudiciaire).FirstOrDefault();
                if (fpjExist == null)
                {
                    return false;
                }
                var idUser= db.TD_Utilisateur.Where(item => item.Uti_Id == acte.IdUtilisateurCreation).FirstOrDefault();
                var userExist = GetOneUserByParent(idUser.Uti_idUser);
                if (fpjExist == null)
                {
                    return false;
                }

                /*TD_Fichier fichier = new TD_Fichier();
                fichier.Fic_Chemin = acte.CheminFichier;
                fichier.Fic_Type = acte.TypeFichier;
                fichier.Fic_DateCreation = DateTime.Now;
                var result = db.TD_Fichier.Add(fichier);
                db.SaveChanges();*/


                TD_ActeAdminstration acteAdministration = new TD_ActeAdminstration();

                acteAdministration.Ata_FPJ_Id = acte.IdFichePersonnelJudiciaire;
                acteAdministration.Ata_NumeroDecision = acte.NumeroDecision;
                acteAdministration.Ata_UtiCreation_Id = userExist.Uti_Id;
              //  acteAdministration.Ata_DateSoumission = acte.D;
                acteAdministration.Ata_Nad_Code = acte.CodeNatureDecision;
                acteAdministration.Ata_Grm_Code = acte.CodeGradeM;
                acteAdministration.Ata_Grf_Code = acte.CodeGradeF;
                acteAdministration.Ata_Gro_Code = acte.CodeGroupe;
                acteAdministration.Ata_Ech_Code = acte.CodeEchelon;
                acteAdministration.Ata_Emp_Code = acte.CodeEmploiJudiciaire;
                acteAdministration.Ata_Fon_Code = acte.CodeFonctionJudiciaire;
                acteAdministration.Ata_Jur_Emp_Code = acte.CodeJuridictionEmploi;
                acteAdministration.Ata_Jur_Fon_Code = acte.CodeJuridictionFonction;
                acteAdministration.Ata_Residence = acte.Residence; 
                acteAdministration.Ata_TyD_Code = acte.CodeTypeDocument;
             //   acteAdministration.Ata_Fic_Id = result.Fic_Id;
                acteAdministration.Ata_DateCreation = DateTime.Now;
                acteAdministration.Ata_EstValide = 0;

                // saving changes
                db.TD_ActeAdminstration.Add(acteAdministration);
                db.SaveChanges();
                tra.Commit();
                tra.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                tra.Rollback();
                tra.Dispose();
                WriteDataError(this.GetType().Name, "Add", ex.ToString());
                return false;
                //throw;
            }
        }

        //Modificiaton d'un acte d'administration
        public bool UpdateActeAdministration(int id, AddOrUpdatecteAdminViewModel acte)
        {
            /* try
             {
                 var acteExist = db.TD_ActeAdminstration.Where(item => item.Ata_Id == id).FirstOrDefault();
                 if (acteExist != null)
                 {
                     // mettre à jour
                     acteExist.Ata_NumeroDecision = acte.Ata_NumeroDecision;
                     acteExist.Ata_Nad_Code = acte.Ata_Nad_Code;
                     acteExist.Ata_TyD_Code = acte.Ata_TyD_Code;
                     acteExist.Ata_Grm_Code = acte.Ata_Grm_Code;
                     acteExist.Ata_Grf_Code = acte.Ata_Grf_Code;
                     acteExist.Ata_Grf_Code = acte.Ata_Grf_Code;
                     acteExist.Ata_Ind_Valeur= acte.Ata_Ind_Valeur;
                     acteExist.Ata_Ech_Code = acte.Ata_Ech_Code;
                     // acteExist.Ata_Ind_Code = acte.Ata_Ind_Code;
                     db.SaveChanges();
                     return true;
                 }
                 WriteDataError(this.GetType().Name, "UpdateActe", "cet acte d'administration n'existe pas");
                 return false;
             }
             catch (Exception ex)
             {
                 WriteDataError(this.GetType().Name, "UpdateActe", ex.ToString());
                 return false;
             }*/

            var tra = db.Database.BeginTransaction();
            try
            {
/*
                TD_Fichier fichier = db.TD_Fichier.Where(item => item.Fic_Id.Equals(acte.IdFichier)).FirstOrDefault();
                if (fichier != null)
                {
                    fichier.Fic_Chemin = acte.CheminFichier;
                    fichier.Fic_DateCreation = DateTime.Now;
                }
                else
                {
                    tra.Dispose();
                    return false;
                }*/


                TD_ActeAdminstration acteExist = db.TD_ActeAdminstration.Where(item => item.Ata_Id.Equals(id)).FirstOrDefault();
                if (acteExist != null)
                {
                    // mettre à jour
                    acteExist.Ata_NumeroDecision = acte.NumeroDecision;
                    acteExist.Ata_Nad_Code = acte.CodeNatureDecision;
                    acteExist.Ata_TyD_Code = acte.CodeTypeDocument;
                    acteExist.Ata_Grm_Code = acte.CodeGradeM;
                    acteExist.Ata_Grf_Code = acte.CodeGradeF;
                    acteExist.Ata_Ind_Valeur = acte.ValeurIndice;
                    acteExist.Ata_Ech_Code = acte.CodeEchelon;
                    acteExist.Ata_Emp_Code = acte.CodeEmploiJudiciaire;
                    acteExist.Ata_Fon_Code = acte.CodeFonctionJudiciaire;
                    acteExist.Ata_Jur_Emp_Code = acte.CodeJuridictionEmploi;
                    acteExist.Ata_Jur_Fon_Code = acte.CodeJuridictionFonction;
                    //acteExist.Ata_Fic_Id = fichier.Fic_Id;
                    acteExist.Ata_DateModification = DateTime.Now;
                }
                else
                {
                    tra.Rollback();
                    tra.Dispose();
                    return false;
                }

                // saving changes
                db.SaveChanges();
                tra.Commit();
                tra.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                tra.Rollback();
                tra.Dispose();
                WriteDataError(this.GetType().Name, "Update", ex.ToString());
                return false;
                //throw;
            }

        }

        //Suppresion d'un acte d'administration
        public bool DeleteActeAdministration(int id)
        {
            var acte = db.TD_ActeAdminstration.Where(item => item.Ata_Id == id).FirstOrDefault();
            if (acte != null)
            {
                db.TD_ActeAdminstration.Remove(acte);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        //Detail d'un acte d'administration
        public TD_ActeAdminstration GetActeAdministrationByCode(int id)
        {
            return db.TD_ActeAdminstration.Where(a => a.Ata_Id == id).FirstOrDefault();
        }
        #endregion
        /// <summary>
        /// Cette méthode permet de vérifier si un utilisateur a déjà un profil unique.
        /// </summary>
        /// <param name="codeProfil">Profil à vérifier</param>
        /// <returns></returns>
        public bool VerifUserByProfil(string codeProfil)
        {
            var userProfilExist = db.TD_Utilisateur.FirstOrDefault(item => item.Uti_Pro_Code.Equals(codeProfil, StringComparison.OrdinalIgnoreCase));
            var profilActive = db.TP_Profil.Any(item => item.Pro_ActifOuiNon == "1" && item.Pro_Code.Equals(codeProfil, StringComparison.OrdinalIgnoreCase));
            var okCreate = userProfilExist != null && profilActive;
            if (okCreate)
            {
                revokeUser(userProfilExist);
                return false;
            }
            return true;
        }
        public List<RegistreViewModel> RegistrePersonnel(GetAllRegistreFilter filter)
        {
            string CodegradePers = "";

            List<RegistreViewModel> liste = new List<RegistreViewModel>();

            var queryPers = db.TD_FichePersonnelJudiciaire.ToList();

            if (filter.Nom != string.Empty && filter.Nom != null)
            {
                queryPers = queryPers.Where(item => item.FPJ_Nom.Contains(filter.Nom)).ToList();
            }
            if (filter.Prenom != string.Empty && filter.Prenom != null)
            {
                queryPers = queryPers.Where(item => item.FPJ_Prenom.Contains(filter.Prenom)).ToList();
            }
            if (filter.Matricule != string.Empty && filter.Matricule != null)
            {
                queryPers = queryPers.Where(item => item.FPJ_Matricule.Contains(filter.Matricule)).ToList();
            }
            if (filter.Grade != string.Empty && filter.Grade != null)
            {
                queryPers = queryPers.Where(item => item.FPJ_Grm_Code.Contains(filter.Grade)).ToList();
            }
            if (filter.TypePersonnel != string.Empty && filter.TypePersonnel != null)
            {
                queryPers = queryPers.Where(item => item.FPJ_TyP_Code.Contains(filter.TypePersonnel)).ToList();
            }
            foreach (var pers in queryPers)
            {
                if (pers.FPJ_TyP_Code.Equals("MAG"))
                {
                    CodegradePers = pers.FPJ_Grm_Code;

                }
                else if (pers.FPJ_TyP_Code.Equals("FOJ"))
                {
                    CodegradePers = pers.FPJ_Grf_Code;

                }
                RegistreViewModel personnel = new RegistreViewModel()
                {
                    Id = pers.FPJ_Id,
                    Prenom = pers.FPJ_Prenom,
                    Nom = pers.FPJ_Nom,
                    Grade = GetGradeByCode(CodegradePers, pers.FPJ_TyP_Code),
                    Juridiction = pers.FPJ_Jur_Fon_Code,
                    MatriculeSolde = pers.FPJ_Matricule,
                    Fonction = pers.FPJ_Fon_Code,
                    TypePersonnel = GetPersByCode(pers.FPJ_TyP_Code).TyP_Libelle
                };
                liste.Add(personnel);
            }

            return liste;
        }
        public string GetGradeByCode(string Code, string TypePersonnel)
        {
            String grade = null;
            if (TypePersonnel.Equals("MAG"))
            {

                grade = db.TP_GradeMagistrat.Where(c => c.Grm_Code.Equals(Code)).FirstOrDefault().Grm_Libelle;
            }

            else if (TypePersonnel.Equals("FOJ"))
            {
                grade = db.TP_GradeFonctionnaireJustice.Where(a => a.Grf_Code.Equals(Code)).FirstOrDefault().Grf_Libelle;

            }
            return grade;

        }

        public string GetJuridictionByCode(string CodeFonc)
        {
            var jur = db.TP_Juridiction.Where(a => a.Jur_Code.Equals(CodeFonc)).FirstOrDefault();
            return db.TP_TypeJuridiction.Where(a => a.TyJ_Code.Equals(jur.Jur_TyJ_Code)).FirstOrDefault().TyJ_Libelle + jur.Jur_Siege;
        }
        public TP_Fonction GetFoncByCode(string CodeFonction)
        {
            return db.TP_Fonction.Where(a => a.Fon_Code.Equals(CodeFonction)).FirstOrDefault();
        }

        public TP_TypePersonnel GetPersByCode(string codePers)
        {
            return db.TP_TypePersonnel.Where(a => a.TyP_Code.Equals(codePers)).FirstOrDefault();
        }

        public bool AddFicheMagistrat(PersonnelViewModel fiche)
        {
            try
            {
                var exist = db.TD_FichePersonnelJudiciaire
                    .Where(item => item.FPJ_Matricule.Equals(fiche.Matricule, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                if (exist != null)
                    return false;
                TD_FichePersonnelJudiciaire magistrat = new TD_FichePersonnelJudiciaire();
                magistrat.FPJ_Nom = fiche.Nom;
                magistrat.FPJ_Prenom = fiche.Prenom;
                magistrat.FPJ_DateDeNaissance = fiche.DateNaissance;
                magistrat.FPJ_LieuDeNaissance = fiche.LieuNaissance;
                magistrat.FPJ_Sexe = fiche.Sexe;
                magistrat.FPJ_SituationMatrimoniale = fiche.SituationMatrimoniale;
                magistrat.FPJ_NumeroCNI = fiche.NumeroCNI;
                magistrat.FPJ_Promotion = fiche.Promotion;
                magistrat.FPJ_DateEntreeEnFonction = fiche.DateEntreVigueur;
                magistrat.FPJ_RangExamen = fiche.RangExamen;
                magistrat.FPJ_Com_Code = fiche.commune;
                magistrat.FPJ_Qua_Code = fiche.Quartier;
                magistrat.FPJ_Telephone = fiche.Telephone;
                magistrat.FPJ_Email = fiche.Email;
                magistrat.FPJ_Matricule = fiche.Matricule;
                magistrat.FPJ_Emp_Code = fiche.Emploi;
                magistrat.FPJ_Jur_Fon_Code = fiche.JuridictionF;
                // magistrat.FPJ_Photo = fiche.Photo;
                magistrat.FPJ_Fon_Code = fiche.Fonction;
                magistrat.FPJ_Ech_Code = fiche.echelon;
                magistrat.FPJ_ConjointMagistratOuiNon = fiche.ConjointMagistratOuiNON;
                magistrat.FPJ_NomConjoint = fiche.NomConjoint;
                magistrat.FPJ_PrenomConjoint = fiche.PrenomConjoint;
                magistrat.FPJ_MatriculeConjoint = fiche.MatriculeConjoint;
                magistrat.FPJ_DateCreationFiche = DateTime.Now;
                magistrat.FPJ_Uti_Creation_Id = fiche.UserCreation;
                magistrat.FPJ_TyP_Code = fiche.TypePersonnel;
                magistrat.FPJ_Jur_Emp_Code = fiche.JuridictionE;
                magistrat.FPJ_Uti_Validation_Id = fiche.UserValidation;
                magistrat.FPJ_Positon = fiche.Position;
                magistrat.FPJ_Gro_Code = fiche.Groupe;
                magistrat.FPJ_Grm_Code = fiche.GradeM;
                magistrat.FPJ_Grf_Code = fiche.GradeF;
                magistrat.FPJ_Cju_Code = fiche.Corps;
                magistrat.FPJ_Dep_Code = fiche.Departement;
                magistrat.FPJ_Reg_Code = fiche.Region;
                db.TD_FichePersonnelJudiciaire.Add(magistrat);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "AddMagistrat", ex.ToString());
                return false;
                //throw;
            }
        }
        // 
        public bool AddFicheFonctionnaire(TD_FichePersonnelJudiciaire fiche)
        {
            try
            {
                fiche.FPJ_DateCreationFiche = DateTime.Now;
                db.TD_FichePersonnelJudiciaire.Add(fiche);
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                WriteDataError(this.GetType().Name, "Addfonctionnaire", ex.ToString());
                return false;
                //throw;
            }
            catch (DbUpdateException ex)
            {
                WriteDataError(this.GetType().Name, "Addfonctionnaire", ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "Addfonctionnaire", ex.ToString());
                return false;
                //throw;
            }
        }
        /// <summary>
        /// Cette méthode permet de révoquer un rôle spécifique à un utilisateur.
        /// </summary>
        /// <param name="user">utilisateur à révoquer</param>
        /// <returns></returns>
        public bool revokeUser(TD_Utilisateur user)
        {
            try
            {
                user.Uti_Pro_Code = "REV";
                user.Uti_ActifOuiNon = "0";
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "revokeuser", ex.ToString());
                return false;
            }
        }
        //public bool revokeUser(int idUser)
        //{
        //    try
        //    {
        //        var user =  db.TD_Utilisateur.Find(idUser);
        //        user.Uti_Pro_Code = "REV";
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteDataError(this.GetType().Name, "AddMagistrat", ex.ToString());
        //        return false;
        //    }
        //}

        //---------Natures Décision---------


        //Not implemented yet
        public bool AddNatureDecision(NatureDecisionViewModel natureDecisionView)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //Not implemented yet
        public bool UpdateNatureDecision(NatureDecisionViewModel natureDecisionView)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public NatureDecisionViewModel GetNatureDecision(int idNatureDecision)
        {
            try
            {
                var natureDecision = db.TP_NatureDecision.Where(item => item.NaD_Id == idNatureDecision).FirstOrDefault();

                var natureDecisionView = new NatureDecisionViewModel
                {
                    codeNatureDecision = natureDecision.NaD_Code,
                    libelleNatureDecision = natureDecision.NaD_Libelle
                };

                return natureDecisionView;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetActeGestion", ex.ToString());
                return null;
            }
        }

        public IEnumerable<NatureDecisionViewModel> GetAllNaturesDecision()
        {
            var queryNatureDecision = db.TP_NatureDecision.AsQueryable();
            try
            {
                var naturesDecision = (from acteGestion in queryNatureDecision
                                       select new NatureDecisionViewModel
                                       {
                                           codeNatureDecision = acteGestion.NaD_Code,
                                           libelleNatureDecision = acteGestion.NaD_Libelle
                                       });

                return naturesDecision;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetActeGestion", ex.ToString());
                return null;
            }
        }

        //not implemented yet
        public bool DeleteNatureDecision(int idNatureDecision)
        {
            return false;
        }

        //---------Types document---------


        //Not implemented yet
        public bool AddTypeDocument(TypeDocumentViewModel typeDocumentView)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //Not implemented yet
        public bool UpdateTypeDocument(TypeDocumentViewModel typeDocumentView)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public TypeDocumentViewModel GetTypeDocument(int idTypeDocument)
        {
            try
            {
                var typeDocument = db.TP_TypeDocument.Where(item => item.TyD_Id == idTypeDocument).FirstOrDefault();

                var typeDocumentView = new TypeDocumentViewModel
                {
                    codeTypeDocument = typeDocument.TyD_Code,
                    libelleTypeDocument = typeDocument.TyD_Libelle
                };

                return typeDocumentView;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetActeGestion", ex.ToString());
                return null;
            }
        }

        public IEnumerable<TypeDocumentViewModel> GetAllTypesDocument()
        {
            var queryTypeDocument = db.TP_TypeDocument.AsQueryable();
            try
            {
                var naturesDecision = (from acteGestion in queryTypeDocument
                                       select new TypeDocumentViewModel
                                       {
                                           codeTypeDocument = acteGestion.TyD_Code,
                                           libelleTypeDocument = acteGestion.TyD_Libelle
                                       });

                return naturesDecision;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetActeGestion", ex.ToString());
                return null;
            }
        }

        //not implemented yet
        public bool DeleteTypeDocument(int idTypeDocument)
        {
            return false;
        }


        //---------Actes de gestion---------
        public bool AddActeGestion(ActeGestionCreationViewModel acteGestionView)
        {
            var tra = db.Database.BeginTransaction();
            try
            {

                var acteExist = db.TD_ActeGestion.Where(item => item.Atg_NumeroDecision == acteGestionView.numeroDecision).FirstOrDefault();
                if(acteExist != null)
                {
                    return false;
                }

                var fpjExist = db.TD_FichePersonnelJudiciaire.Where(item => item.FPJ_Id == acteGestionView.idFichePersonnel).FirstOrDefault();
                if (fpjExist == null)
                {
                    return false;
                }

                var userExist = GetOneUserByParent(acteGestionView.idUtilisateurDeCreation);
                if (fpjExist == null)
                {
                    return false;
                }

                TD_Fichier fichier = new TD_Fichier();
                fichier.Fic_Chemin = acteGestionView.cheminFichier;
                fichier.Fic_Type = acteGestionView.typeFichier;
                fichier.Fic_DateCreation = DateTime.Now;
                var result = db.TD_Fichier.Add(fichier);
                db.SaveChanges();

                TD_ActeGestion acteGestion = new TD_ActeGestion();
                acteGestion.Atg_FPJ_Id = acteGestionView.idFichePersonnel;
                acteGestion.Atg_UtiCreation_Id = userExist.Uti_Id;
                acteGestion.Atg_DateEntreeVigueur = acteGestionView.dateEntreeEnVigueur;
                acteGestion.Atg_Nad_Code = acteGestionView.codeNatureDecision;
                acteGestion.Atg_NumeroDecision = acteGestionView.numeroDecision;
                acteGestion.Atg_TyD_Code = acteGestionView.codeTypeDocument;
                acteGestion.Atg_Fic_Id = result.Fic_Id;
                acteGestion.Atg_DateCreation = DateTime.Now;
                acteGestion.Atg_EstValide = 0;

                // saving changes
                db.TD_ActeGestion.Add(acteGestion);
                db.SaveChanges();
                tra.Commit();
                tra.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                tra.Rollback();
                tra.Dispose();
                WriteDataError(this.GetType().Name, "AddUser", ex.ToString());
                return false;
                //throw;
            }


        }
        public bool UpdateActeGestion(int idActeGestion, ActeGestionModificationViewModel acteGestionView)
        {
            var tra = db.Database.BeginTransaction();
            try
            {

                TD_Fichier fichier = db.TD_Fichier.Where(item => item.Fic_Id.Equals(acteGestionView.idFichier)).FirstOrDefault();
                if (fichier != null)
                {
                    fichier.Fic_Chemin = acteGestionView.cheminFichier;
                    fichier.Fic_DateCreation = DateTime.Now;
                }

                TD_ActeGestion acteGestion = db.TD_ActeGestion.Where(item => item.Atg_Id.Equals(idActeGestion)).FirstOrDefault();
                if (acteGestion != null)
                {
                    acteGestion.Atg_DateEntreeVigueur = acteGestionView.dateEntreeEnVigueur;
                    acteGestion.Atg_Nad_Code = acteGestionView.codeNatureDecision;
                    acteGestion.Atg_NumeroDecision = acteGestionView.numeroDecision;
                    acteGestion.Atg_TyD_Code = acteGestionView.codeTypeDocument;
                    if(fichier!=null)
                        acteGestion.Atg_Fic_Id = fichier.Fic_Id;
                    acteGestion.Atg_DateModification = DateTime.Now;
                }
                else
                {
                    tra.Rollback();
                    tra.Dispose();
                    return false;
                }

                // saving changes
                db.SaveChanges();
                tra.Commit();
                tra.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                tra.Rollback();
                tra.Dispose();
                WriteDataError(this.GetType().Name, "AddUser", ex.ToString());
                return false;
                //throw;
            }
        }
        public ActeGestionConsultationViewModel GetActeGestion(int idActeGestion)
        {
            try
            {
                var queryActeGestion = db.TD_ActeGestion.AsQueryable();

                var queryFiche = db.TD_FichePersonnelJudiciaire.AsQueryable();
                var queryTypePersonnel = db.TP_TypePersonnel.AsQueryable();
                var queryNatureDecision = db.TP_NatureDecision.AsQueryable();
                var queryTypeDocument = db.TP_TypeDocument.AsQueryable();
                var queryFichier = db.TD_Fichier.AsQueryable();
                var queryUser = db.TD_Utilisateur.AsQueryable();

                var acte = (from acteGestion in queryActeGestion

                               join fiche in queryFiche on acteGestion.Atg_FPJ_Id equals fiche.FPJ_Id
                               join typePersonnel in queryTypePersonnel on fiche.FPJ_TyP_Code equals typePersonnel.TyP_Code
                                join natureDecision in queryNatureDecision on acteGestion.Atg_Nad_Code equals natureDecision.NaD_Code
                                join typeDocument in queryTypeDocument on acteGestion.Atg_TyD_Code equals typeDocument.TyD_Code
                                join fichier in queryFichier on acteGestion.Atg_Fic_Id equals fichier.Fic_Id
                                join userCreation in queryUser on acteGestion.Atg_UtiCreation_Id equals userCreation.Uti_Id
                                join userValidation in queryUser on acteGestion.Atg_UtiValidation_Id equals userValidation.Uti_Id into userValidationGroup
                                from userValidation in userValidationGroup.DefaultIfEmpty()
                                where acteGestion.Atg_Id == idActeGestion
                               select new ActeGestionConsultationViewModel
                               {
                                    id = acteGestion.Atg_Id,
                                    idFichePersonnel = fiche.FPJ_Id,
                                    NomFichePersonnel = fiche.FPJ_Nom,
                                    PrenomFichePersonnel = fiche.FPJ_Prenom,
                                    TypeFichePersonnel = typePersonnel.TyP_Libelle,
                                    idUtilisateurDeCreation = userCreation.Uti_Id,
                                    EmailUtilisateurDeCreation = userCreation.Uti_Email,
                                    NomUtilisateurDeCreation = userCreation.Uti_Nom,
                                    PrenomUtilisateurDeCreation = userCreation.Uti_Prenom,
                                    idUtilisateurDeValidation = userValidation == null? 0 : userValidation.Uti_Id,
                                    EmailUtilisateurDeValidation = userValidation == null ? null : userValidation.Uti_Email,
                                    NomUtilisateurDeValidation = userValidation == null ? null : userValidation.Uti_Nom,
                                    PrenomUtilisateurDeValidation = userValidation.Uti_Prenom,
                                    dateCreation = acteGestion.Atg_DateCreation,
                                    dateModification = acteGestion.Atg_DateModification,
                                    dateSoumission = acteGestion.Atg_DateSoumission,
                                    dateEntreeEnVigueur = acteGestion.Atg_DateEntreeVigueur,
                                    dateValidation = acteGestion.Atg_DateValidation,
                                    estValide = acteGestion.Atg_EstValide,
                                    numeroDecision = acteGestion.Atg_NumeroDecision,
                                    codeNatureDecision = acteGestion.Atg_Nad_Code,
                                    libelleNatureDecision = natureDecision.NaD_Libelle,
                                    codeTypeDocument = acteGestion.Atg_TyD_Code,
                                    libelleTypeDocument = typeDocument.TyD_Libelle,
                                    cheminFichier = fichier.Fic_Chemin,
                                    typeFichier = fichier.Fic_Type,
                                    idFichier = fichier.Fic_Id
                               }).FirstOrDefault();


                return acte;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetActeGestion", ex.ToString());
                return null;
            }
        }

        public IEnumerable<ActeGestionConsultationViewModel> GetAllActesGestion(GetAllActesGestionFilter filter)
        {
            try
            {
                var queryActeGestion = db.TD_ActeGestion.AsQueryable();

                if (filter.numeroDecision != null && !filter.numeroDecision.Equals(string.Empty))
                {
                    queryActeGestion = queryActeGestion.Where(item => item.Atg_NumeroDecision.Contains(filter.numeroDecision));
                }
                if ((filter.dateEntreeEnVigueurMin != null && !filter.dateEntreeEnVigueurMin.Equals(DateTime.MinValue)))
                {
                    queryActeGestion = queryActeGestion.Where(item => item.Atg_DateEntreeVigueur >= filter.dateEntreeEnVigueurMin);
                }
                if ((filter.dateEntreeEnVigueurMax != null && !filter.dateEntreeEnVigueurMax.Equals(DateTime.MinValue)))
                {
                    queryActeGestion = queryActeGestion.Where(item => item.Atg_DateEntreeVigueur <= filter.dateEntreeEnVigueurMax);
                }
                if ((filter.dateCreationMin != null && !filter.dateCreationMin.Equals(DateTime.MinValue)))
                {
                    queryActeGestion = queryActeGestion.Where(item => item.Atg_DateCreation >= filter.dateCreationMin);
                }
                if ((filter.dateCreationMax != null && !filter.dateCreationMax.Equals(DateTime.MinValue)))
                {
                    queryActeGestion = queryActeGestion.Where(item => item.Atg_DateCreation <= filter.dateCreationMax);
                }
                if (filter.estValide != null && filter.estValide.Equals(string.Empty))
                {
                    queryActeGestion = queryActeGestion.Where(item => item.Atg_EstValide == int.Parse(filter.estValide));
                }
                if (filter.codeNatureDecision != null && !filter.codeNatureDecision.Equals(string.Empty))
                {
                    queryActeGestion = queryActeGestion.Where(item => item.Atg_Nad_Code.Equals(filter.codeNatureDecision));
                }
                if (filter.codeTypeDocument != null && !filter.codeTypeDocument.Equals(string.Empty))
                {
                    queryActeGestion = queryActeGestion.Where(item => item.Atg_TyD_Code.Equals(filter.codeTypeDocument));
                }

                var queryFiche = db.TD_FichePersonnelJudiciaire.AsQueryable();

                if (filter.idFichePersonnel != null && !filter.idFichePersonnel.Equals(string.Empty))
                {
                    queryFiche = queryFiche.Where(item => item.FPJ_Id == int.Parse(filter.idFichePersonnel));
                }
                if (filter.nomFichePersonnel != null && !filter.nomFichePersonnel.Equals(string.Empty))
                {
                    queryFiche = queryFiche.Where(item => item.FPJ_Nom.Contains(filter.nomFichePersonnel));
                }
                if (filter.prenomFichePersonnel != null && !filter.prenomFichePersonnel.Equals(string.Empty))
                {
                    queryFiche = queryFiche.Where(item => item.FPJ_Prenom.Contains(filter.prenomFichePersonnel));
                }
                if (filter.codeTypeFichePersonnel != null && !filter.codeTypeFichePersonnel.Equals(string.Empty))
                {
                    queryFiche = queryFiche.Where(item => item.FPJ_TyP_Code.Equals(filter.codeTypeFichePersonnel));
                }


                var queryTypePersonnel = db.TP_TypePersonnel.AsQueryable();
                var queryNatureDecision = db.TP_NatureDecision.AsQueryable();
                var queryTypeDocument = db.TP_TypeDocument.AsQueryable();
                var queryFichier = db.TD_Fichier.AsQueryable();
                var queryUser = db.TD_Utilisateur.AsQueryable();

                var acte = (from acteGestion in queryActeGestion
                            join fiche in queryFiche on acteGestion.Atg_FPJ_Id equals fiche.FPJ_Id
                            join typePersonnel in queryTypePersonnel on fiche.FPJ_TyP_Code equals typePersonnel.TyP_Code
                            join natureDecision in queryNatureDecision on acteGestion.Atg_Nad_Code equals natureDecision.NaD_Code //
                            join typeDocument in queryTypeDocument on acteGestion.Atg_TyD_Code equals typeDocument.TyD_Code //
                            join fichier in queryFichier on acteGestion.Atg_Fic_Id equals fichier.Fic_Id //
                            select new ActeGestionConsultationViewModel
                            {
                                id = acteGestion.Atg_Id,
                                idFichePersonnel = fiche.FPJ_Id,
                                NomFichePersonnel = fiche.FPJ_Nom,
                                PrenomFichePersonnel = fiche.FPJ_Prenom,
                                TypeFichePersonnel = typePersonnel.TyP_Libelle,
                                dateCreation = acteGestion.Atg_DateCreation,
                                dateModification = acteGestion.Atg_DateModification,
                                dateSoumission = acteGestion.Atg_DateSoumission,
                                dateEntreeEnVigueur = acteGestion.Atg_DateEntreeVigueur,
                                estValide = acteGestion.Atg_EstValide,
                                codeNatureDecision = acteGestion.Atg_Nad_Code,
                                libelleNatureDecision = natureDecision.NaD_Libelle,
                                numeroDecision = acteGestion.Atg_NumeroDecision,
                                codeTypeDocument = acteGestion.Atg_TyD_Code,
                                libelleTypeDocument = typeDocument.TyD_Libelle,
                                idFichier = fichier.Fic_Id
                            });

                return acte.ToList();
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetActeGestion", ex.ToString());
                return null;
            }
        }

        //not implemented yet
        public bool DeleteActeGestion(int idActeGestion)
        {
            return false;
        }

        public bool ValiderActeGestion(int idActeGestion, int idUtiValidation)
        {
            try
            {
                TD_ActeGestion acteGestion = db.TD_ActeGestion.Where(item => item.Atg_Id.Equals(idActeGestion)).FirstOrDefault();
                if (acteGestion != null)
                {
                    acteGestion.Atg_DateValidation = DateTime.Now;
                    acteGestion.Atg_UtiValidation_Id = idUtiValidation;
                    acteGestion.Atg_EstValide = 1;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "ValiderActeGestion", ex.ToString());
                return false;
            }
        }

        public bool InvaliderActeGestion(int idActeGestion, int idUtiInvalidation)
        {
            try
            {
                TD_ActeGestion acteGestion = db.TD_ActeGestion.Where(item => item.Atg_Id.Equals(idActeGestion)).FirstOrDefault();
                if (acteGestion != null)
                {
                    acteGestion.Atg_DateValidation = null;
                    acteGestion.Atg_UtiValidation_Id = idUtiInvalidation;
                    acteGestion.Atg_EstValide = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "InvaliderActeGestion", ex.ToString());
                return false;
            }
        }
        public bool SoumettreActeGestion(int idActeGestion)
        {
            try
            {
                TD_ActeGestion acteGestion = db.TD_ActeGestion.Where(item => item.Atg_Id.Equals(idActeGestion)).FirstOrDefault();
                if (acteGestion != null)
                {
                    acteGestion.Atg_DateSoumission = DateTime.Now;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "SoumettreActeGestion", ex.ToString());
                return false;
            }
        }


        //----------Fichiers-------------

        public TD_Fichier GetFichier(int idFichier)
        {
            try
            {
                var fichier = db.TD_Fichier.Where(item => item.Fic_Id == idFichier).FirstOrDefault();

                return fichier;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "GetActeGestion", ex.ToString());
                return null;
            }
        }


        #region Tables de parametrages (TP)
        public IEnumerable<TP_CorpsJudiciaire> GetAllCorpsJudiciaire(GetAllCorpsFilter filter)
        {
            var query = db.TP_CorpsJudiciaire.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Cju_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Cju_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_Region> GetAllRegion(GetAllRegionFilter filter)
        {
            var query = db.TP_Region.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Reg_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Reg_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_Departement> GetAllDepartement(GetAllDepartementFilter filter)
        {
            var query = db.TP_Departement.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Dep_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Dep_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeRegion != string.Empty && filter.CodeRegion != null)
            {
                query = query.Where(item => item.Dep_Reg_Code.Equals(filter.CodeRegion, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_Commune> GetAllCommune(GetAllCommuneFilter filter)
        {
            var query = db.TP_Commune.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Com_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Com_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeDepartement != string.Empty && filter.CodeDepartement != null)
            {
                query = query.Where(item => item.Com_Dep_Code.Equals(filter.CodeDepartement, StringComparison.OrdinalIgnoreCase));
            }

            return query.ToList();
        }

        public IEnumerable<JuridictionViewModel> GetAllJuridiction(GetAllJuridictionFilter filter)
        {
            var queryTypeJuridiction = db.TP_TypeJuridiction.AsQueryable();
            var queryClasseJuridiction = db.TP_ClasseJuridiction.AsQueryable();
            var query = db.TP_Juridiction.AsQueryable();

            // filtres 
            if (filter.Siege != string.Empty && filter.Siege != null)
            {
                query = query.Where(item => item.Jur_Siege.Contains(filter.Siege));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Jur_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeTypeJuridiction != string.Empty && filter.CodeTypeJuridiction != null)
            {
                query = query.Where(item => item.Jur_TyJ_Code.Equals(filter.CodeTypeJuridiction, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeClasseJuridiction != string.Empty && filter.CodeClasseJuridiction != null)
            {
                query = query.Where(item => item.Jur_Clj_Code.Equals(filter.CodeClasseJuridiction, StringComparison.OrdinalIgnoreCase));
            }

            var all = from jur in query
                      join typeJur in queryTypeJuridiction on jur.Jur_TyJ_Code equals typeJur.TyJ_Code
                      join classeJur in queryClasseJuridiction on jur.Jur_Clj_Code equals classeJur.Clj_Code
                      select new JuridictionViewModel
                      {
                          Id = jur.Jur_Id,
                          Code = jur.Jur_Code,
                          Siege = jur.Jur_Siege,
                          ClasseJuridictionCode = classeJur.Clj_Code,
                          ClasseJuridictionLibelle = classeJur.Clj_Libelle,
                          TypeJuridictionCode = typeJur.TyJ_Code,
                          TypeJuridictionLibelle = typeJur.TyJ_Libelle,
                          ClasseJuridictionAncienLibelle = typeJur.TyJ_LibelleAnc
                      };
            return all.ToList();
        }

        public IEnumerable<TP_Quartier> GetAllQuartier(GetAllQuartierFilter filter)
        {
            var query = db.TP_Quartier.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Qua_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Qua_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeCommune != string.Empty && filter.CodeCommune != null)
            {
                query = query.Where(item => item.Qua_Com_Code.Equals(filter.CodeCommune, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }
        public IEnumerable<TP_ClasseJuridiction> GetAllClasseJuridiction(GetAllClasseJuridictionFilter filter)
        {
            var query = db.TP_ClasseJuridiction.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Clj_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Clj_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_FonctionFonctionnaireJustice> GetAllFonctionFonctionnaire(GetAllFonctionFonctionnaireFilter filter)
        {
            var query = db.TP_FonctionFonctionnaireJustice.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.FFJ_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.FFJ_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }


        public IEnumerable<TP_Echelon> GetAllEchelon(GetAllEchelonFilter filter)
        {
            var query = db.TP_Echelon.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Ech_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Ech_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeGrade != string.Empty && filter.CodeGrade != null)
            {
                query = query.Where(item => item.Ech_Grf_Code.Equals(filter.CodeGrade, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeGroupe != string.Empty && filter.CodeGroupe != null)
            {
                query = query.Where(item => item.Ech_Gro_Code.Equals(filter.CodeGroupe, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeModePassage != string.Empty && filter.CodeModePassage != null)
            {
                query = query.Where(item => item.Ech_MoP_Code.Equals(filter.CodeModePassage, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeTypePersonnel != string.Empty && filter.CodeTypePersonnel != null)
            {
                query = query.Where(item => item.Ech_Typ_Code.Equals(filter.CodeTypePersonnel, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.Duree != default(Nullable<int>) && filter.Duree != null)
            {
                query = query.Where(item => item.Ech_Duree == filter.Duree);
            }
            if (filter.IdIndice != default(Nullable<int>) && filter.IdIndice != null)
            {
                query = query.Where(item => item.Ech_Ind_Id == filter.IdIndice);
            }
            return query.ToList();
        }

        public IEnumerable<TP_ModePassage> GetAllModePassage(GetAllModePassageFilter filter)
        {
            var query = db.TP_ModePassage.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.MoP_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.MoP_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_NatureDecision> GetAllNatureDecision(GetAllNatureDecisionFilter filter)
        {
            var query = db.TP_NatureDecision.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.NaD_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.NaD_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_TypePersonnel> GetAllTypePersonnel(GetAllTypePersonnelFilter filter)
        {
            var query = db.TP_TypePersonnel.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.TyP_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.TyP_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_EmploiJudiciaire> GetAllEmploiJudiciaire(GetAllEmploiJudiciaireFilter filter)
        {
            var query = db.TP_EmploiJudiciaire.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Emp_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Emp_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_GradeFonctionnaireJustice> GetAllGradeFoncJustice(GetAllGradeFoncJusticeFilter filter)
        {
            var query = db.TP_GradeFonctionnaireJustice.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Grf_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Grf_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeCorpsJudiciaire != string.Empty && filter.CodeCorpsJudiciaire != null)
            {
                query = query.Where(item => item.Grf_Cju_Code.Equals(filter.CodeCorpsJudiciaire, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_GradeMagistrat> GetAllGradeMagistrat(GetAllGradeMagistratFilter filter)
        {
            var query = db.TP_GradeMagistrat.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Grm_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Grm_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_Groupe> GetAllGroupe(GetAllGroupeFilter filter)
        {
            var query = db.TP_Groupe.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Gro_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Gro_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.CodeGradeMagistrat != string.Empty && filter.CodeGradeMagistrat != null)
            {
                query = query.Where(item => item.Gro_Grm_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_Indice> GetAllIndice(GetAllIndiceFilter filter)
        {
            var query = db.TP_Indice.AsQueryable();
            // filtres 
            if (filter.IdIndice != default(int))
            {
                query = query.Where(item => item.Ind_Id == filter.IdIndice);
            }
            if (filter.Valeur != default(Nullable<long>))
            {
                query = query.Where(item => item.Ind_Valeur == filter.Valeur);
            }
            return query.ToList();
        }

        public IEnumerable<TP_TypeDocument> GetAllTypeDocument(GetAllTypeDocumentFilter filter)
        {
            var query = db.TP_TypeDocument.AsQueryable();
            //filtres
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.TyD_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.TyD_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.Echeance != default(Nullable<int>) && filter.Echeance != null)
            {
                query = query.Where(item => item.TyD_Echeance == filter.Echeance);
            }
            if (filter.AlerteRouge != default(Nullable<int>) && filter.AlerteRouge != null)
            {
                query = query.Where(item => item.TyD_AlerteRouge == filter.AlerteRouge);
            }
            if (filter.AlerteJaune != default(Nullable<int>) && filter.AlerteRouge != null)
            {
                query = query.Where(item => item.TyD_AlerteJaune == filter.AlerteRouge);
            }
            if (filter.CodeTypeActe != string.Empty && filter.CodeTypeActe != null)
            {
                query = query.Where(item => item.TyD_TyA_Code.Equals(filter.CodeTypeActe, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.Mouvement != string.Empty && filter.Mouvement != null)
            {
                query = query.Where(item => item.TyD_Mouvement.Equals(filter.Mouvement, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.MagistratOuiNon != string.Empty && filter.MagistratOuiNon != null)
            {
                query = query.Where(item => item.TyD_MagistratOuiNon.Equals(filter.MagistratOuiNon, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.FonctionnaireJudiciaireOuiNon != string.Empty && filter.FonctionnaireJudiciaireOuiNon != null)
            {
                query = query.Where(item => item.TyD_FonctionnaireJudiciaireOuiNon.Equals(filter.FonctionnaireJudiciaireOuiNon, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_TypeJuridiction> GetAllTypeJuridiction(GetAllTypeJuridctionFilter filter)
        {
            var query = db.TP_TypeJuridiction.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.TyJ_Libelle.Contains(filter.Libelle));
            }
            if (filter.LibelleAncien != string.Empty && filter.LibelleAncien != null)
            {
                query = query.Where(item => item.TyJ_LibelleAnc.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.TyJ_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }

        public IEnumerable<TP_Fonction> GetAllFonction(GetAllFonctionFilter filter)
        {
            var query = db.TP_Fonction.AsQueryable();
            // filtres 
            if (filter.Libelle != string.Empty && filter.Libelle != null)
            {
                query = query.Where(item => item.Fon_Libelle.Contains(filter.Libelle));
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(item => item.Fon_Code.Equals(filter.Code, StringComparison.OrdinalIgnoreCase));
            }
            return query.ToList();
        }
        #endregion

        public bool UpdateFiche(int idFiche, TD_FichePersonnelJudiciaire fiche)
        {
            var ficheExist = db.TD_FichePersonnelJudiciaire.Where(item => item.FPJ_Id == idFiche).FirstOrDefault();
            if (ficheExist != null)
            {
                ficheExist.FPJ_Nom = fiche.FPJ_Nom;
                ficheExist.FPJ_Prenom = fiche.FPJ_Prenom;
                ficheExist.FPJ_DateDeNaissance = fiche.FPJ_DateDeNaissance;
                ficheExist.FPJ_LieuDeNaissance = fiche.FPJ_LieuDeNaissance;
                ficheExist.FPJ_Sexe = fiche.FPJ_Sexe;
                ficheExist.FPJ_SituationMatrimoniale = fiche.FPJ_SituationMatrimoniale;
                ficheExist.FPJ_NumeroCNI = fiche.FPJ_NumeroCNI;
                ficheExist.FPJ_Promotion = fiche.FPJ_Promotion;
                ficheExist.FPJ_DateEntreeEnFonction = fiche.FPJ_DateEntreeEnFonction;
                ficheExist.FPJ_RangExamen = fiche.FPJ_RangExamen;
                ficheExist.FPJ_Com_Code = fiche.FPJ_Com_Code;
                ficheExist.FPJ_Qua_Code = fiche.FPJ_Qua_Code;
                ficheExist.FPJ_Telephone = fiche.FPJ_Telephone;
                ficheExist.FPJ_Email = fiche.FPJ_Email;
                ficheExist.FPJ_Matricule = fiche.FPJ_Matricule;
                ficheExist.FPJ_Emp_Code = fiche.FPJ_Emp_Code;
                ficheExist.FPJ_Jur_Fon_Code = fiche.FPJ_Jur_Fon_Code;

                ficheExist.FPJ_SIP_Id = fiche.FPJ_SIP_Id;

                ficheExist.FPJ_Fon_Code = fiche.FPJ_Fon_Code;
                ficheExist.FPJ_Ech_Code = fiche.FPJ_Ech_Code;
                ficheExist.FPJ_ConjointMagistratOuiNon = fiche.FPJ_ConjointMagistratOuiNon;
                ficheExist.FPJ_NomConjoint = fiche.FPJ_NomConjoint;
                ficheExist.FPJ_Grf_Code = fiche.FPJ_Grf_Code;
                ficheExist.FPJ_Grm_Code = fiche.FPJ_Grm_Code;
                ficheExist.FPJ_Gro_Code = fiche.FPJ_Gro_Code;
                //ficheExist.FPJ_TyP_Code = fiche.FPJ_TyP_Code;
                ficheExist.FPJ_Uti_Creation_Id = fiche.FPJ_Uti_Creation_Id;
                ficheExist.FPJ_Uti_Validation_Id = fiche.FPJ_Uti_Validation_Id;
                ficheExist.FPJ_DateValidation = fiche.FPJ_DateValidation;
                ficheExist.FPJ_EstValide = fiche.FPJ_EstValide;
                ficheExist.FPJ_Reference = fiche.FPJ_Reference;
                ficheExist.FPJ_Reg_Code = fiche.FPJ_Reg_Code;
                ficheExist.FPJ_Dep_Code = fiche.FPJ_Dep_Code;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ValiderFiche(int idFiche, int idUser)
        {
            var fiche = db.TD_FichePersonnelJudiciaire.Where(item => item.FPJ_Id == idFiche).FirstOrDefault();
            if (fiche != null)
            {
                fiche.FPJ_Uti_Validation_Id = idUser;
                fiche.FPJ_DateValidation = DateTime.Now;
                fiche.FPJ_EstValide = 1;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public TD_FichePersonnelJudiciaire GetOneFicheMagistrat(int idFiche)
        {
            var queryFiche = db.TD_FichePersonnelJudiciaire.AsQueryable();
            var queryEchelon = db.TP_Echelon.AsQueryable();
            var queryIndice = db.TP_Indice.AsQueryable();
            var queryGroupe = db.TP_Groupe.AsQueryable();
            var queryGradeMagistrat = db.TP_GradeMagistrat.AsQueryable();
            var queryGradeFonc = db.TP_GradeFonctionnaireJustice.AsQueryable();
            var queryFichePersonnel = db.TD_FichePersonnelJudiciaire.AsQueryable();
            var queryUser = db.TD_Utilisateur.AsQueryable();


            var result = from fiche in queryFiche
                         join echelon in queryEchelon on fiche.FPJ_Ech_Code equals echelon.Ech_Code
                         join groupe in queryGroupe on fiche.FPJ_Gro_Code equals groupe.Gro_Code
                         join userCreation in queryUser on fiche.FPJ_Uti_Creation_Id equals userCreation.Uti_Id
                         join userValidation in queryUser on fiche.FPJ_Uti_Validation_Id equals userValidation.Uti_Id
                         join gradeM in queryGradeMagistrat on fiche.FPJ_Grm_Code equals gradeM.Grm_Code
                         join gradeF in queryGradeFonc on fiche.FPJ_Grf_Code equals gradeF.Grf_Code
                         select new TD_FichePersonnelJudiciaire
                         {
                             FPJ_Id = fiche.FPJ_Id,
                             FPJ_Nom = fiche.FPJ_Nom,
                             FPJ_Prenom = fiche.FPJ_Prenom,
                             FPJ_DateDeNaissance = fiche.FPJ_DateDeNaissance,
                             FPJ_LieuDeNaissance = fiche.FPJ_LieuDeNaissance,
                             FPJ_Sexe = fiche.FPJ_Sexe,
                             FPJ_SituationMatrimoniale = fiche.FPJ_SituationMatrimoniale,
                             FPJ_NumeroCNI = fiche.FPJ_NumeroCNI,
                             FPJ_Promotion = fiche.FPJ_Promotion,
                             FPJ_DateEntreeEnFonction = fiche.FPJ_DateEntreeEnFonction,
                             FPJ_RangExamen = fiche.FPJ_RangExamen,
                             FPJ_Com_Code = fiche.FPJ_Com_Code,
                             FPJ_Qua_Code = fiche.FPJ_Qua_Code,
                             FPJ_Telephone = fiche.FPJ_Telephone,
                             FPJ_Email = fiche.FPJ_Email,
                             FPJ_Matricule = fiche.FPJ_Matricule,
                             FPJ_Emp_Code = fiche.FPJ_Emp_Code,
                             FPJ_Jur_Fon_Code = fiche.FPJ_Jur_Fon_Code,
                             //FPJ_Photo = fiche.FPJ_Photo,
                             FPJ_Fon_Code = fiche.FPJ_Fon_Code,
                             FPJ_Ech_Code = fiche.FPJ_Ech_Code,
                             FPJ_ConjointMagistratOuiNon = fiche.FPJ_ConjointMagistratOuiNon,
                             FPJ_NomConjoint = fiche.FPJ_NomConjoint,
                         };
            return result.FirstOrDefault();
        }
        /// <summary>
        /// Obtenir une fiche de par son identifiant.
        /// </summary>
        /// <param name="idFiche">identifiant</param>
        /// <returns>fiche personnel</returns>
        public TD_FichePersonnelJudiciaire GetOneFiche(int idFiche)
        {
            return db.TD_FichePersonnelJudiciaire.Find(idFiche);
        }
        /// <summary>
        /// Vérifier si une fiche existe avec une matricule.
        /// </summary>
        /// <param name="matricule">matricule</param>
        /// <returns></returns>
        public bool VerifFicheByMatricule(string matricule)
        {
            return db.TD_FichePersonnelJudiciaire.Where(item => item.FPJ_Matricule.Equals(matricule, StringComparison.OrdinalIgnoreCase)).Any();
        }
        /// <summary>
        /// Vérifier si une fiche existe avec une numéro de téléphone.
        /// </summary>
        /// <param name="telephone">numero de telephone</param>
        /// <returns></returns>
        public bool VerifFicheByTelephone(string telephone)
        {
            return db.TD_FichePersonnelJudiciaire.Where(item => item.FPJ_Telephone.Equals(telephone, StringComparison.OrdinalIgnoreCase)).Any();
        }
        /// <summary>
        /// Vérifier si une fiche existe avec une numéro de téléphone.
        /// </summary>
        /// <param name="email">adresse electronique</param>
        /// <returns>Bool</returns>
        public bool VerifFicheByEmail(string email)
        {
            return db.TD_FichePersonnelJudiciaire.Where(item => item.FPJ_Email.Equals(email, StringComparison.OrdinalIgnoreCase)).Any();
        }
        /// <summary>
        /// Cette méthode nous permet de vérifier si une fiche existe déjà avec ce cni.
        /// </summary>
        /// <param name="cni">cni</param>
        /// <returns>Bool</returns>
        public bool VerifFicheByCni(string cni)
        {
            return db.TD_FichePersonnelJudiciaire.Where(item => item.FPJ_NumeroCNI.Equals(cni, StringComparison.OrdinalIgnoreCase)).Any();
        }
        /// <summary>
        /// Cette méthode nous permet de supprimer une fiche.
        /// </summary>
        /// <param name="id">identifiant</param>
        /// <returns></returns>
        public bool DeleteFiche(int id)
        {
            var fiche = db.TD_FichePersonnelJudiciaire.Find(id);
            if (fiche == null || fiche.FPJ_EstValide == '1')
                return false;
            db.TD_FichePersonnelJudiciaire.Remove(fiche);
            db.SaveChanges();
            return true;
        }

        public TD_Signature_Photo AddSignaturePhoto(TD_Signature_Photo signaturePhoto)
        {
            try
            {
                db.TD_Signature_Photo.Add(signaturePhoto);
                db.SaveChanges();
                return signaturePhoto;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "AddSignaturePhoto", ex.ToString());
                return null;
            }
        }

        public bool UpdateSignaturePhoto(int id, TD_Signature_Photo signaturePhoto)
        {
            try
            {
                var item = db.TD_Signature_Photo.Find(id);
                if (item == null)
                    return false;
                item.SIP_FileSave = signaturePhoto.SIP_FileSave;
                item.SIP_Name = signaturePhoto.SIP_Name;
                item.SIP_contentype = signaturePhoto.SIP_contentype;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteDataError(this.GetType().Name, "UpdateSignaturePhoto", ex.ToString());
                return false;
            }

        }
        public bool DeleteSignaturePhoto(int id)
        {
            var item = db.TD_Signature_Photo.Find(id);
            if (item == null) return false;
            db.TD_Signature_Photo.Remove(item);
            db.SaveChanges();
            return true;
        }

        public FichePersonnelViewModel GetFonctionnaireDetail(int idFiche)
        {
            // queryables tables
            var queryFiche = db.TD_FichePersonnelJudiciaire.AsQueryable();
            //var queryEchelon = db.TP_Echelon.AsQueryable();
            //var queryFonction = db.TP_FonctionFonctionnaireJustice.AsQueryable();
            //var queryCorps = db.TP_CorpsJudiciaire.AsQueryable();
            //var queryGradeFonc = db.TP_GradeFonctionnaireJustice.AsQueryable();
            //var queryFichePersonnel = db.TD_FichePersonnelJudiciaire.AsQueryable();
            //var queryUser = db.TD_Utilisateur.AsQueryable();
            // condition
            queryFiche = queryFiche.Where(fiche => fiche.FPJ_Id == idFiche);
            // jointures
            var result = from fiche in queryFiche
                         select new FichePersonnelViewModel
                         {
                             FPJ_Id = fiche.FPJ_Id,
                             FPJ_Nom = fiche.FPJ_Nom,
                             FPJ_Prenom = fiche.FPJ_Prenom,
                             FPJ_DateDeNaissance = fiche.FPJ_DateDeNaissance,
                             FPJ_LieuDeNaissance = fiche.FPJ_LieuDeNaissance,
                             FPJ_Sexe = fiche.FPJ_Sexe,
                             FPJ_SituationMatrimoniale = fiche.FPJ_SituationMatrimoniale,
                             FPJ_NumeroCNI = fiche.FPJ_NumeroCNI,
                             FPJ_Promotion = fiche.FPJ_Promotion,
                             FPJ_DateEntreeEnFonction = fiche.FPJ_DateEntreeEnFonction,
                             FPJ_RangExamen = fiche.FPJ_RangExamen,
                             FPJ_Com_Code = fiche.FPJ_Com_Code,
                             FPJ_Qua_Code = fiche.FPJ_Qua_Code,
                             FPJ_Telephone = fiche.FPJ_Telephone,
                             FPJ_Email = fiche.FPJ_Email,
                             FPJ_SIP_Id = fiche.FPJ_SIP_Id,
                             FPJ_Matricule = fiche.FPJ_Matricule,
                             FPJ_Emp_Code = fiche.FPJ_Emp_Code,
                             FPJ_Jur_Fon_Code = fiche.FPJ_Jur_Fon_Code,
                             FPJ_Fon_Code = fiche.FPJ_Fon_Code,
                             FPJ_Ech_Code = fiche.FPJ_Ech_Code,
                             FPJ_ConjointMagistratOuiNon = fiche.FPJ_ConjointMagistratOuiNon,
                             FPJ_NomConjoint = fiche.FPJ_NomConjoint,
                         };
            var item = result.FirstOrDefault();
            // photo
            if (item == null)
                return null;
            var photo = db.TD_Signature_Photo.Find(item.FPJ_SIP_Id);
            if(photo != null)
            {
                item.FPJ_PhotoContent = Convert.ToBase64String(photo.SIP_FileSave);
                item.FPJ_PhotoType = photo.SIP_contentype;
            }
            return item;
        }
        public string GetStringBase64FromBytes(byte[] bytes)
        {
            var stringBase64 = Convert.ToBase64String(bytes);
            return stringBase64;
        }
    }
}
