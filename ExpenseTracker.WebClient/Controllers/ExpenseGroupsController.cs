using ExpenseTracker.DTO;
using ExpenseTracker.WebClient.Helpers;
using ExpenseTracker.WebClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.WebClient.Controllers
{
    public class ExpenseGroupsController : Controller
    {

        public async Task<ActionResult> Index()
        {

            var client = ExpenseTrackerHttpClient.GetClient();

            var model = new ExpenseGroupViewModel();

            HttpResponseMessage egsResponse = await client.GetAsync("api/expensegroupstatuses");

            if (egsResponse.IsSuccessStatusCode)
            {
                string content = await egsResponse.Content.ReadAsStringAsync();
                model.ExpenseGroupStatuses = JsonConvert.DeserializeObject<IEnumerable<ExpenseGroupStatus>>(content);
            }
            else
            {
                return Content("An error occurred.");
            }

            HttpResponseMessage response = await client.GetAsync("api/expensegroups");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                model.ExpenseGroups = JsonConvert.DeserializeObject<IList<ExpenseGroup>>(content);
            }
            else
            {
                return Content("An error occurred.");
            }

            return View(model);

        }


        // GET: ExpenseGroups/Details/5
        public ActionResult Details(int id)
        {
            return Content("Not implemented yet.");
        }

        // GET: ExpenseGroups/Create
 
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpenseGroups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExpenseGroup expenseGroup)
        {
            try
            {
                expenseGroup.ExpenseGroupStatusId = 1;
                expenseGroup.UserId = @"https://expensetrackeridsrv3/embedded_1";

                var client = ExpenseTrackerHttpClient.GetClient();
                StringContent sc = new StringContent(JsonConvert.SerializeObject(expenseGroup),
                                        System.Text.Encoding.Unicode,
                                        "application/json");

                HttpResponseMessage response = await client.PostAsync("api/expensegroups", sc);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error occurred.");
                }

            }
            catch (Exception)
            {
                return Content("An error occurred.");
            }

        }

        // GET: ExpenseGroups/Edit/5
 
        public async Task<ActionResult> Edit(int id)
        {
            var client = ExpenseTrackerHttpClient.GetClient();
            HttpResponseMessage response = await client.GetAsync("api/expensegroups/" + id);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var eg = JsonConvert.DeserializeObject<ExpenseGroup>(content);
                return View(eg);
            }
            else
            {
                return Content("An error occurred.");
            }
        }

        // POST: ExpenseGroups/Edit/5   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ExpenseGroup expenseGroup)
        {
            try
            {
                var client = ExpenseTrackerHttpClient.GetClient();
                StringContent sc = new StringContent(JsonConvert.SerializeObject(expenseGroup),
                                        System.Text.Encoding.Unicode,
                                        "application/json");

                HttpResponseMessage response = await client.PutAsync("api/expensegroups/" + id, sc);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error occurred.");
                }

            }
            catch (Exception)
            {
                return Content("An error occurred.");
            }

        }


        // POST: ExpenseGroups/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var client = ExpenseTrackerHttpClient.GetClient();
            HttpResponseMessage response = await client.DeleteAsync("api/expensegroups/" + id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Content("An error occurred.");
            }
        }
    }
}
