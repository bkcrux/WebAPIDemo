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
    public class ExpenseGroupStatusesController : ApiController
    {

        IExpenseTrackerRepository _repository;
        ExpenseMasterDataFactory _expenseMasterDataFactory = new ExpenseMasterDataFactory();

        public ExpenseGroupStatusesController()
        {
            _repository = new ExpenseTrackerEFRepository(new
                            Repository.Entities.ExpenseTrackerContext());
        }

        public ExpenseGroupStatusesController(IExpenseTrackerRepository rep)
        {
            _repository = rep;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var expenseGroupStatuses = _repository.GetExpenseGroupStatusses();

                return Ok(expenseGroupStatuses.ToList()
                    .Select(egs => _expenseMasterDataFactory.CreateExpenseGroupStatus(egs)));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

    }
}
