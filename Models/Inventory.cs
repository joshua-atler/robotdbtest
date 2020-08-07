using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class Inventory
    {
        
        [Key]
        public int ID { get; set; }

        public string PartName { get; set; }
        public string PartType { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }

        
    }
}

