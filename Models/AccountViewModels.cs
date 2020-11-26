using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace puceAsk_dev1.Models
{
    

public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        [Display(Name = "URL de retorno")]
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {

        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        [Display(Name = "Proveedor")]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Codigo")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "¿Recordar este navegador?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Recuérdame")]
        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string Nickname { get; set; }

        

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

        [Display(Name = "Usuario")]
        public ApplicationUser Usuario { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "Foto")]
        public byte[] Foto { get; set; }

        [Required]
        [Display(Name = "Nombre de Usuario")]
        [RegularExpression(@"^[A-Za-z\.\-_]+[A-Za-z0-9\.\-_]*", ErrorMessage = "El nombre de usuario no puede empezar con un número ni contener caracteres especiales (A excepción de guion (-), underscore(_) o punto (.)")]
        public string Nickname { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9+-]+\\.[a-zA-Z]+(\\.[a-zA-Z])*$", ErrorMessage = "El correo electrónico no es correcto")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }
        public bool Sexo { get; set; }

        

        [Required]
        [StringLength(100, ErrorMessage = "{0} debe contener almenos {2} carácteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmación de contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña nueva y de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} debe contener almenos {2} carácteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "La contraseña nueva y de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }



}
