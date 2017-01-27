using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace theWall.Models
{
    public class Msg: BaseEntity
    {
        [Key]
        public int id {get; set;}
        
        public Msg() {
            comments = new List<Com>();
        }
        [Required]
        [MinLength(2)]
        public string message { get; set; }

        [DataType(DataType.Date)]
        public DateTime created_at {get;set;}

        [DataType(DataType.Date)]
        public DateTime updated_at {get;set;}

        public int userid {get; set;}

        public User user {get;set;}
        public ICollection<Com> comments { get; set; }
    }
}