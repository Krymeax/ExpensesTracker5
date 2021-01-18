using ExpensesTracker5.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker5.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ExpenseDBContext _db;

        public ExpenseController(ExpenseDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ExpenseModel> objList = _db.Expenses;
            return View(objList);
        }

        public IActionResult OpenCharts()
        {
            return View();
        }


        //Search View Index
        public IActionResult IndexSearch(string searchString)
        {
            IEnumerable<ExpenseModel> objList = _db.Expenses;
            if (!String.IsNullOrEmpty(searchString))
            {
                IEnumerable<ExpenseModel> expensesList = GetSearchResult(searchString).ToList();
                return View("Index",expensesList);
            }
            else return View("Index", objList);
        }

        // GET CREATE
        public IActionResult Create()
        {
            IEnumerable<ExpenseModel> objList = _db.Expenses;
            return View();
        }

        // POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExpenseModel obj)
        {
            // Didn't check for validation because it was bugging out
                _db.Expenses.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
        }

        // GET EDIT
        public IActionResult Edit(int? id)
        {
            if(id==null | id == 0)
            {
                return NotFound();
            }
            var obj = _db.Expenses.Find(id);
            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ExpenseModel obj)
        {
            // Didn't check for validation because it was bugging out
            _db.Expenses.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                return NotFound();
            }
            var obj = _db.Expenses.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(ExpenseModel obj)
        {
            //var obj = _db.Expenses.Find(id);
            //if (obj == null)
            //{
            //    return NotFound();
            //}
            _db.Expenses.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetMonthlyExpense() // 30 days
        {
                Dictionary<string, decimal> dictWeeklySum = new Dictionary<string, decimal>();

                decimal utilitiesSum = _db.Expenses.Where
                    (cat => cat.Category == "Utilities" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
                    .Select(cat => cat.Amount)
                    .Sum();

                decimal payrollsSum = _db.Expenses.Where
                   (cat => cat.Category == "Payrolls" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal rentSum = _db.Expenses.Where
                   (cat => cat.Category == "Rent" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal maintenanceSum = _db.Expenses.Where
                   (cat => cat.Category == "Maintenance" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
                   .Select(cat => cat.Amount)
                   .Sum();

                decimal bankSum = _db.Expenses.Where
                (cat => cat.Category == "Bank rates" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
                .Select(cat => cat.Amount)
                .Sum();

                dictWeeklySum.Add("Utilities", utilitiesSum);
                dictWeeklySum.Add("Payrolls", payrollsSum);
                dictWeeklySum.Add("Rent", rentSum);
                dictWeeklySum.Add("Maintenance", maintenanceSum);
                dictWeeklySum.Add("Bank rates", bankSum);

            return new JsonResult(dictWeeklySum);
        }

        public JsonResult GetYearlyExpense() // last 12 months
        {
            Dictionary<string, decimal> dictYearlySum = new Dictionary<string, decimal>();

            decimal utilitiesSum = _db.Expenses.Where
                (cat => cat.Category == "Utilities" && (cat.ExpenseDate > DateTime.Now.AddMonths(-12)))
                .Select(cat => cat.Amount)
                .Sum();

            decimal payrollsSum = _db.Expenses.Where
               (cat => cat.Category == "Payrolls" && (cat.ExpenseDate > DateTime.Now.AddMonths(-12)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal rentSum = _db.Expenses.Where
               (cat => cat.Category == "Rent" && (cat.ExpenseDate > DateTime.Now.AddMonths(-12)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal maintenanceSum = _db.Expenses.Where
               (cat => cat.Category == "Maintenance" && (cat.ExpenseDate > DateTime.Now.AddMonths(-12)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal bankSum = _db.Expenses.Where
            (cat => cat.Category == "Bank rates" && (cat.ExpenseDate > DateTime.Now.AddMonths(-12)))
            .Select(cat => cat.Amount)
            .Sum();

            dictYearlySum.Add("Utilities", utilitiesSum);
            dictYearlySum.Add("Payrolls", payrollsSum);
            dictYearlySum.Add("Rent", rentSum);
            dictYearlySum.Add("Maintenance", maintenanceSum);
            dictYearlySum.Add("Bank rates", bankSum);

            return new JsonResult(dictYearlySum);
        }

        public IEnumerable<ExpenseModel> GetSearchResult(string searchString)
        {
            List<ExpenseModel> exp = new List<ExpenseModel>();
            IEnumerable<ExpenseModel> result = exp;

            exp = _db.Expenses.ToList();
            result = exp.Where(x => x.ExpenseName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
            if (result == null)
                result = exp.Where(x => x.Category.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
            return result;
        }

    }
}
