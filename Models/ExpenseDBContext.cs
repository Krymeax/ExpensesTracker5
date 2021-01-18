using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker5.Models
{
    public class ExpenseDBContext  : DbContext
    {
        public ExpenseDBContext(DbContextOptions<ExpenseDBContext> options) : base(options) 
        {

        }
        
        public DbSet<ExpenseModel> Expenses { get; set; }
    }
}
