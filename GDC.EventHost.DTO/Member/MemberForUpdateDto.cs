using GDC.EventHost.DTO.ShoppingCart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GDC.EventHost.DTO.Member
{
    public class MemberForUpdateDto
    {
        public Guid Id { get; set; }

        [Display(Name = "UserId")]
        [Required(ErrorMessage = "You should enter a UserId.")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "The User Id must be a valid Guid.")]
        public string UserId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "You should enter a Username.")]
        [MaxLength(15, ErrorMessage = "The Username should not be longer than 15 characters.")]
        public string Username { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You should enter a Name.")]
        [MaxLength(150, ErrorMessage = "The title should not be longer than 150 characters.")]
        public string FullName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "You should enter an Email Address.")]
        [RegularExpression(@"^(\D)+(\w)*((\.(\w)+)?)+@(\D)+(\w)*((\.(\D)+(\w)*)+)?(\.)[a-z]{2,}$",
            ErrorMessage = "The Email Address must be a valid email address format.")]
        public string EmailAddress { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ShoppingCartDto ShoppingCart { get; set; }
    }
}
