using ExpenseTracker.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Repository.Helpers;

namespace ExpenseTracker.Repository.Factories
{
    public class ExpenseGroupFactory
    {
        ExpenseFactory expenseFactory = new ExpenseFactory();

        public ExpenseGroupFactory()
        {

        }

        public ExpenseGroup CreateExpenseGroup(DTO.ExpenseGroup expenseGroup)
        {
            return new ExpenseGroup()
            {
                Description = expenseGroup.Description,
                ExpenseGroupStatusId = expenseGroup.ExpenseGroupStatusId,
                Id = expenseGroup.Id,
                Title = expenseGroup.Title,
                UserId = expenseGroup.UserId,
                Expenses = expenseGroup.Expenses == null ? new List<Expense>() : expenseGroup.Expenses.Select(e => expenseFactory.CreateExpense(e)).ToList()
            };
        }


        public DTO.ExpenseGroup CreateExpenseGroup(ExpenseGroup expenseGroup)
        {
            return new DTO.ExpenseGroup()
            {
                Description = expenseGroup.Description,
                ExpenseGroupStatusId = expenseGroup.ExpenseGroupStatusId,
                Id = expenseGroup.Id,
                Title = expenseGroup.Title,
                UserId = expenseGroup.UserId,
                Expenses = expenseGroup.Expenses.Select(e => expenseFactory.CreateExpense(e)).ToList()
            };
        }

        public object CreateDataShapeObject(ExpenseGroup expenseGroup, List<string> lstFields)
        {
            //entity to DTO
            return CreateDataShapeObject(CreateExpenseGroup(expenseGroup), lstFields);
        }

        public object CreateDataShapeObject(DTO.ExpenseGroup expenseGroup, List<string> lstFields)
        {
            List<string> lstFieldsIncluded = new List<string>(lstFields);

            if (!lstFieldsIncluded.Any())
            {
                return expenseGroup;
            }
            else
            {
                var lstExpenseFields = lstFieldsIncluded.Where(s => s.Contains("expenses")).ToList();

                bool returnPartialExpense = lstFieldsIncluded.Any() && !lstFieldsIncluded.Contains("expenses");

                if (returnPartialExpense)
                {
                    lstFieldsIncluded.RemoveRange(lstExpenseFields);
                    lstExpenseFields = lstExpenseFields.Select(f => f.Substring(f.IndexOf(".") + 1)).ToList();
                }
                else
                {
                    lstExpenseFields.Remove("expenses");
                    lstFieldsIncluded.RemoveRange(lstExpenseFields);
                }


                ExpandoObject objToReturn = new ExpandoObject();
                foreach (var field in lstFieldsIncluded)
                {
                    var fieldValue = expenseGroup.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(expenseGroup, null);

                    ((IDictionary<string, object>)objToReturn).Add(field, fieldValue);
                }

                if(returnPartialExpense)
                {
                    List<object> expenses = new List<object>();
                    foreach(var expense in expenseGroup.Expenses)
                    {
                        expenses.Add(expenseFactory.CreateDataShapeObject(expense, lstExpenseFields));
                    }
                    ((IDictionary<string, object>)objToReturn).Add("expenses", expenses);
                }

                return objToReturn;
            }
        }

    }
}
