using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace Blogge.Extensions.Providers
{
    public static class SecretQuestionProvider
    {
        [ExcludeFromCodeCoverage]
        public static List<SelectListItem> GetSecurityQuestions()
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
