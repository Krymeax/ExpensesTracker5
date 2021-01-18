using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker5.Models
{
    public class ExpenseModel
    {
        [Key]
        public int ExpenseId { get; set; }
        [Required]
        public string ExpenseName { get; set; }
        [Required]
        [Range(0.01,int.MaxValue,ErrorMessage ="The amount must be greater than 0.01")]
        public decimal Amount { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; } = DateTime.Now; // change to choose time/date
        public Boolean Expired { get; }
        public DateTime DueDate { get; set; }
        // public Boolean RecursiveTransaction {get;set;}
    }
}
