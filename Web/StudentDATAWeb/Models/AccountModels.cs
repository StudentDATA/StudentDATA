﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using WebMatrix.WebData;


namespace StudentDATAWeb.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<RSSFlowsDatas> RSSFlowsDatasList { get; set; }
        public DbSet<UsersSettings> UsersSettingsList { get; set; }
        public DbSet<ProfessionalPosts> ProfessionalPostsList { get; set; }
    }
    [Table("Usersettings")]
    public class UsersSettings
    {
        [Key]
        [Required]
        public int UserKey { get; set; }
        public bool SemesterShow { get; set; }
        public bool FieldShow { get; set; }
        public bool PfhShow { get; set; }
    }
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //Code avec semestre et filière
        public string Code { get; set; }
        //Comprend le projet ou le poste et l'entreprise
        public string ActualActivity { get; set; }
        //TODO : [PICTURE] Not used in the first version 
        public string ProfilePicUrl { get; set; }
        public string MailAdress { get; set; }
        public PermissionEnum Permission { get; set; }

    }
    public class SortPostModel
    {
        [Display(Name = "Semestre")]
        public bool Semester { get; set; }
        [Display(Name = "Filière")]
        public bool Field { get; set; }
        [Display(Name = "PFH")]
        public bool Pfh { get; set; }
    }
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur :")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe :")]
        public string Password { get; set; }

        [Display(Name = "Connexion automatique :")]
        public bool RememberMe { get; set; }
    }

    public class EventModel
    {
        [Required]
        [Display(Name = "Titre de l'évènement :")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Professeur ou équipe en charge :")]
        public string Teacher { get; set; }

        [Required]
        [Display(Name = "Salle :")]
        public string Salle { get; set; }

        [Required]
        [Display(Name = "Date début (AAAA-MM-JJ HH:MM) :")]
        public string Begin { get; set; }

        [Required]
        [Display(Name = "Date fin (AAAA-MM-JJ HH:MM) :")]
        public string End { get; set; }
    }
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur :")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe :")]
        public string Password { get; set; }

		[Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe :")]
        [Compare("Password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }


        [Required]
        public PermissionEnum Role
        {
            get
            {
                return PermissionEnum.Student;
            }
            set
            {
            }
        }

    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    [Table("RSSFlows")]
    public class RSSFlowsDatas
    {
        [Key]
        [Required]
        public int FlowId { get; set; }
        [Column("FlowName", Order = 1, TypeName = "varchar")]
        [Required]
        public string FlowName { get; set; }
        [Column("URLAdress", Order = 2, TypeName = "varchar")]
        [Required]
        public string Adress { get; set; }


    }
    public class FlowPostModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Titre")]
        public string Title { get; set; }
        //[Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Contenu")]
        public string Content { get; set; }
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Url de l'article")]
        public string Url { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nom du flux :")]
        public string FlowUrl { get; set; }


    }
    public class ProfileModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nom d'utilisateur")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nom de famille")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Filière ")]
        public string StudyField { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Semestre ")]
        public string Semester { get; set; }

        [Required]
        [Display(Name = "Projet ou Job actuel")]
        [DataType(DataType.Text)]
        public string ActualActivity { get; set; }

        //[Required]
        [Display(Name = "Photo de profil")]
        public string ProfilePicUrl { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adresse mail")]
        public string MailAdress { get; set; }

        [Required]
        [Display(Name = "Compte")]
        public PermissionEnum Permission { get; set; }

        //public UserProfile GetUser(UsersContext db, string UserName)
        //{
        //    UserProfile user;
        //    user = db.UserProfiles.Find(WebSecurity.GetUserId(UserName));
        //    return user;
        //}
    }

    [Table("ProfesionalPosts")]
    public class ProfessionalPosts
    {
        [Key]
        [Required]
        public int PostId { get; set; }
        [Column("PostTitle", Order = 1, TypeName = "varchar")]
        [Required]
        public string PostTitle { get; set; }
        public string PostDescription { get; set; }
        [Column("PostDate", Order = 4, TypeName = "datetime2")]
        public DateTime PostDate { get; set; }
    }

}
