using ProjectDiploma.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataStore.Entities
{
    public class User : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        ////        [Key]
        ////        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        ////        public int Id { get; set; }

        ////        [Required]
        ////        public string Login { get; set; }

        ////        [Required]
        ////        public string Password { get; set; }

        ////        [Required]
        ////        public string Email { get; set; }
    }
}
