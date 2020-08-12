using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class Inventory
    {
        
        [Key]
        public int ID { get; set; }

        [Display(Name = "Part Name")]
        public string PartName { get; set; }

        public string SKU { get; set; }

        [Display(Name = "Part Type")]
        public string PartType { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        public string Location { get; set; }

        public InventoryStatus Status { get; set; }


        public enum InventoryStatus
        {
            [Display(Name = "Submitted")]
            Submitted,
            [Display(Name = "Approved")]
            Approved,
            [Display(Name = "Rejected")]
            Rejected
        }


        public decimal TotalCost {
            get
            {
                return Quantity * UnitCost;
            }
        }
    }

    
}

