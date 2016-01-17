using ExpenseTracker.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Repository.Factories
{
    public class ExpenseFactory
    {
        
        public ExpenseFactory()
        {

        }

        public DTO.Expense CreateExpense(Expense expense)
        {
            return new DTO.Expense()
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseGroupId = expense.ExpenseGroupId,
                Id = expense.Id
            };
        }



        public Expense CreateExpense(DTO.Expense expense)
        {
            return new Expense()
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseGroupId = expense.ExpenseGroupId,
                Id = expense.Id
            };
        }
         
        public object CreateDataShapeObject(Expense expense, List<string> lstFields)
        {
            //entity to DTO
            return CreateDataShapeObject(CreateExpense(expense), lstFields);
        }

        public object CreateDataShapeObject(DTO.Expense expense, List<string> lstFields)
        {
            if (!lstFields.Any())
            {
                return expense;
            }
            else
            {
                ExpandoObject objToReturn = new ExpandoObject();
                foreach(var field in lstFields)
                {
                    var fieldValue = expense.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(expense, null);
                   
                    ((IDictionary<string, object>)objToReturn).Add(field, fieldValue);
                }

                return objToReturn;
            }
        }
    }
}
