using System.ComponentModel.DataAnnotations;
using System;

namespace theWall.Models
{
    public class Com: BaseEntity
    {
        [Key]
        public int id {get; set;}

        [Required]
        [MinLength(2)]
        public string comment { get; set; }

        [DataType(DataType.Date)]
        public DateTime created_at {get;set;}

        [DataType(DataType.Date)]
        public DateTime updated_at {get;set;}

        public int userid {get; set;}

        public User user {get;set;}

        public int msgid {get; set;}

        public Msg mssg {get;set;}
    }
}