
using System.ComponentModel.DataAnnotations;
using System;
using theWall.Models;
using System.Collections.Generic;

namespace theWall.Models
{
    public abstract class BaseEntity {}
    public class User: BaseEntity
    {
        public User(){
            msgs = new List<Msg>();
        }

        public ICollection<Msg> msgs {get;set;}
        [Key]
        public int id {get;set;}

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string fname { get; set;}

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string lname { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string pwd { get; set; }

        
        [DataType(DataType.Password)]      
        [Required]
        [Compare("pwd", ErrorMessage = "Passwords do not match")]
        public string cpwd { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$")]
        public string email { get; set; }

        [DataType(DataType.Date)]
        public DateTime created_at {get;set;}

        [DataType(DataType.Date)]
        public DateTime updated_at {get;set;}

        [Required]
        [RegularExpression(@"^[A-Za-z0-9]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string Pwd { get; set; }

    }
}