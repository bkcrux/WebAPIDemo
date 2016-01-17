using ExpenseTracker.Repository;
using ExpenseTracker.Repository.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpenseTracker.API.Controllers
{
    [RoutePrefix("api")]
    public class ExpensesController : ApiController
    {
        IExpenseTrackerRepository _repository;
        ExpenseFactory _expenseFactory = new ExpenseFactory();

        public ExpensesController()
        {
            _repository = new ExpenseTrackerEFRepository(new Repository.Entities.ExpenseTrackerContext());
        }

        public ExpensesController(IExpenseTrackerRepository rep)
        {
            _repository = rep;
        }

        [Route("expensegroups/{expenseGroupId}/expenses")]
        public IHttpActionResult Get(int expenseGroupId)
        {
            try
            {
                var expenses = _repository.GetExpenses(expenseGroupId);
                if (expenses == null)
                {
                    return NotFound();
                }

                var expensesResult = expenses
                    .ToList()
                    .Select(exp => _expenseFactory.CreateExpense(exp));

                return Ok(expensesResult);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("expenses/{Id}")]
        [Route("expensegroups/{expenseGroupId}/expenses/{id}")]
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
