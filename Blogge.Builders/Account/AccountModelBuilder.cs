using System.Collections.Generic;
using System.Web.Mvc;
using Blogge.Interfaces.Builders.Account;
using Blogge.Models.ViewModels;

namespace Blogge.Builders.Account
{
    public class AccountModelBuilder : IAccountModelBuilder
    {
        public RegisterViewModel BuildRegisterViewModel()
        {
            var model = new RegisterViewModel()
            {
                SecurityQuestionList = new SelectList(GetQuestions(), "Value", "Text")
            };
            return model;
        }

        public ForgotPasswordViewModel BuildForgotPasswordViewModel()
        {
            var model = new ForgotPasswordViewModel()
            {
                SecurityQuestionList = new SelectList(GetQuestions(), "Value", "Text")
            };
            return model;
        }

        private  List<SelectListItem> GetQuestions()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem { Value = "What was your childhood nickname?", Text = "What was your childhood nickname?"},
                new SelectListItem { Value = "What is the name of your favorite childhood friend?", Text = "What is the name of your favorite childhood friend?"},
                new SelectListItem { Value = "What school did you attend for sixth grade?", Text = "What school did you attend for sixth grade?"},
                new SelectListItem { Value = "In what city or town did your mother and father meet?", Text = "In what city or town did your mother and father meet?"},
                new SelectListItem { Value = "What is the first name of the boy or girl that you first kissed?", Text = "What is the first name of the boy or girl that you first kissed?"},
                new SelectListItem { Value = "What is the name of a college you applied to but didn't attend?", Text = "What is the name of a college you applied to but didn't attend?"},
                new SelectListItem { Value = "Where were you when you first heard about 9/11?", Text = "Where were you when you first heard about 9/11?"},
                new SelectListItem { Value = "What is your grandmother's first name?", Text = "What is your grandmother's first name?"},
                new SelectListItem { Value = "What was the make and model of your first car?", Text = "What was the make and model of your first car?"},
                new SelectListItem { Value = "In what city and country do you want to retire?", Text = "In what city and country do you want to retire?"}
            };
            return list;
        }
    }
}
