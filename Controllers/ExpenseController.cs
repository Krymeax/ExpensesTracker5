using ExpensesTracker5.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ExpensesTracker5.Controllers
{
    public class ExpenseController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ExpenseDBContext _db;

        public ExpenseController(ExpenseDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var objList = _db.Expenses.ToList();
            //Console.WriteLine(JsonConvert.SerializeObject(objList))
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
                return View("Index", expensesList);
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
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public IActionResult Create(ExpenseModel obj)
        {

            // Didn't check for validation because it was bugging out
            _db.Expenses.Add(obj);
            _db.SaveChanges();
            //Console.WriteLine(JsonConvert.SerializeObject(obj));
            return RedirectToAction("Index");
        }

        // GET EDIT
        public IActionResult Edit(int? id)
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

        // POST EDIT
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
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
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public IActionResult DeletePost(ExpenseModel obj)
        {

            _db.Expenses.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[Microsoft.AspNetCore.Mvc.HttpPost]
        //public Microsoft.AspNetCore.Mvc.JsonResult GetMonthlyExpense() // 30 days
        //{
        //        Dictionary<string, decimal> dictWeeklySum = new Dictionary<string, decimal>();

        //        decimal utilitiesSum = _db.Expenses.Where
        //            (cat => cat.Category == "Utilities" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
        //            .Select(cat => cat.Amount)
        //            .Sum();

        //        decimal payrollsSum = _db.Expenses.Where
        //           (cat => cat.Category == "Payrolls" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
        //           .Select(cat => cat.Amount)
        //           .Sum();

        //        decimal rentSum = _db.Expenses.Where
        //           (cat => cat.Category == "Rent" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
        //           .Select(cat => cat.Amount)
        //           .Sum();

        //        decimal maintenanceSum = _db.Expenses.Where
        //           (cat => cat.Category == "Maintenance" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))d
        //           .Select(cat => cat.Amount)
        //           .Sum();

        //        decimal bankSum = _db.Expenses.Where
        //        (cat => cat.Category == "Bank rates" && (cat.ExpenseDate > DateTime.Now.AddDays(-30)))
        //        .Select(cat => cat.Amount)
        //        .Sum();

        //        dictWeeklySum.Add("Utilities", utilitiesSum);
        //        dictWeeklySum.Add("Payrolls", payrollsSum);
        //        dictWeeklySum.Add("Rent", rentSum);
        //        dictWeeklySum.Add("Maintenance", maintenanceSum);
        //        dictWeeklySum.Add("Bank rates", bankSum);

        //    return Json(dictWeeklySum, JsonRequestBehavior.AllowGet);
        //}
        //[Microsoft.AspNetCore.Mvc.HttpPost]
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public Microsoft.AspNetCore.Mvc.JsonResult GetMonthlyExpense() // 30 days
        {
            List<ExpenseModel> list = new List<ExpenseModel>();
            list = _db.Expenses.Select(a => new ExpenseModel { ExpenseName = a.ExpenseName, Amount = a.Amount }).ToList();
            //Console.WriteLine(JsonConvert.SerializeObject(list));
            return Json(new { JSONList = list });
        }


        public Microsoft.AspNetCore.Mvc.JsonResult GetYearlyExpense() // last 12 months
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

            return new Microsoft.AspNetCore.Mvc.JsonResult(dictYearlySum, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<ExpenseModel> GetSearchResult(string searchString)
        {
            List<ExpenseModel> exp = new List<ExpenseModel>();
            IEnumerable<ExpenseModel> result = null;

            exp = _db.Expenses.ToList();
            result = exp.Where(x => x.ExpenseName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
            if (result.Any() == false)
                result = exp.Where(x => x.Category.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
            if (result.Any() == false)
            {
                IEnumerable<ExpenseModel> expenseDates = null;
                expenseDates = exp.Where(x => x.ExpenseDate.ToShortDateString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
                IEnumerable<ExpenseModel> dueDates = null;
                dueDates = exp.Where(x => x.DueDate.ToShortDateString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
                result = expenseDates.Concat(dueDates);
            }
            if (result.Any() == false && (searchString == "Expired" || searchString == "expired" || searchString == "EXPIRED"))
            {
                result = exp.Where(x => x.Expired == true);
            }
            if (result.Any() == false && (searchString == "due" || searchString == "Due" || searchString == "DUE"))
            {
                result = exp.Where(x => x.IsCompleted == false);
            }
            return result;
        }

    }
}
