using ExpenseTracker.API.Helpers;
using ExpenseTracker.Repository;
using ExpenseTracker.Repository.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace ExpenseTracker.API.Controllers
{
    [RoutePrefix("api")]
    public class ExpensesController : ApiController
    {
        IExpenseTrackerRepository _repository;
        ExpenseFactory _expenseFactory = new ExpenseFactory();

        const int maxPageSize = 10;

        public ExpensesController()
        {
            _repository = new ExpenseTrackerEFRepository(new Repository.Entities.ExpenseTrackerContext());
        }

        public ExpensesController(IExpenseTrackerRepository rep)
        {
            _repository = rep;
        }

        [Route("expensegroups/{expenseGroupId}/expenses", Name = "ExpensesForGroup")]
        public IHttpActionResult Get(int expenseGroupId, string sort = "date", string fields = null,
            int page = 1, int pageSize = maxPageSize)
        {
            try
            {
                List<string> lstFields = new List<string>();
                if (fields != null)
                {
                    lstFields = fields.ToLower().Split(',').ToList();
                }


                var expenses = _repository.GetExpenses(expenseGroupId);
                if (expenses == null)
                   {
                    return NotFound();
                }

                if (pageSize > maxPageSize)
                {
                    pageSize = maxPageSize;
                }

                var totalCount = expenses.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                //Generate next/prev page links to send back on response
                var urlH = new UrlHelper(Request);
                var prevLink = page > 1 ? urlH.Link("ExpensesForGroup",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        sort = sort,
                        fields = fields
                    }) : "";

                var nextLink = page < totalPages ? urlH.Link("ExpensesForGroup",
                    new
                    {
                        page = page + 1,
                        pageSize = pageSize,
                        sort = sort,
                        fields = fields
                    }) : "";

                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPage = totalPages,
                    previousPageLink = prevLink,
                    nextPageLink = nextLink
                };

                HttpContext.Current.Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

                var expensesResult = expenses
                    .ApplySort(sort)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList()
                    .Select(exp => _expenseFactory.CreateDataShapeObject(exp, lstFields));

                return Ok(expensesResult);
            } 
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [VersionedRoute("expenses/{Id}", 1)]
        [VersionedRoute("expensegroups/{expenseGroupId}/expenses/{id}", 1)]
        public IHttpActionResult Get(int id, int? expenseGroupId = null)
        {
            try
            {
                Repository.Entities.Expense expense = null;

                if (expenseGroupId == null)
                {
                    expense = _repository.GetExpense(id);    
                }
                else
                {
                    var expensesForGroup = _repository.GetExpenses((int)expenseGroupId);
                    if (expensesForGroup != null)
                    {
                        expense = expensesForGroup.FirstOrDefault(eg => eg.Id == id);
                    }
                }

                if (expense != null)
                {
                    return Ok(_expenseFactory.CreateExpense(expense));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [VersionedRoute("expenses/{Id}", 2)]
        [VersionedRoute("expensegroups/{expenseGroupId}/expenses/{id}", 2)]
        public IHttpActionResult GetV2(int id, int? expenseGroupId = null)
        {
            try
            {
                Repository.Entities.Expense expense = null;

                if (expenseGroupId == null)
                {
                    expense = _repository.GetExpense(id);
                }
                else
                {
                    var expensesForGroup = _repository.GetExpenses((int)expenseGroupId);
                    if (expensesForGroup != null)
                    {
                        expense = expensesForGroup.FirstOrDefault(eg => eg.Id == id);
                    }
                }

                if (expense != null)
                {
                    return Ok(_expenseFactory.CreateExpense(expense));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        [Route("expenses/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var result = _repository.DeleteExpense(id);

                if (result.Status == RepositoryActionStatus.Deleted)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else if (result.Status == RepositoryActionStatus.NotFound)
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        [Route("expenses")]
        public IHttpActionResult Post([FromBody] DTO.Expense expense)
        {
            try
            {
                if (expense == null)
                {
                    return BadRequest();
                }

                var exp = _expenseFactory.CreateExpense(expense);

                var result = _repository.InsertExpense(exp);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var dtoExp = _expenseFactory.CreateExpense(result.Entity);
                    return Created<DTO.Expense>(Request.RequestUri + "/" + dtoExp.Id, dtoExp);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();                
            }
        }

        [Route("expenses/{id}")]
        public IHttpActionResult Put(int id, [FromBody] DTO.Expense expense)
        {
            try
            {
                if (expense == null)
                {
                    return BadRequest();
                }

                var exp = _expenseFactory.CreateExpense(expense);

                var result = _repository.UpdateExpense(exp);
                if (result.Status == RepositoryActionStatus.Updated)
                {
                    var dtoExp = _expenseFactory.CreateExpense(result.Entity);
                    return Ok(dtoExp);
                }
                else if (result.Status == RepositoryActionStatus.NotFound)
                {
                    return NotFound();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

    }
}
