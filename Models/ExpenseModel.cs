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
        //[DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)] - this date time format created a lot of issues in serialization/deserialization
        public DateTime ExpenseDate { get; set; } = DateTime.Now; // change to choose time/date
        //public Boolean Expired { get { return ((DateTime.Today > DueDate.Date) && IsCompleted == false); } }
        public Boolean Expired { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now + TimeSpan.FromDays(99999);

        public Boolean IsCompleted { get; set; } = true;

        // public Boolean RecursiveTransaction {get;set;}
    }
}
