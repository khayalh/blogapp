using Blogge.Models.ViewModels;

namespace Blogge.Interfaces.Builders.Account
{
    public interface IAccountModelBuilder
    {
        RegisterViewModel BuildRegisterViewModel();
        ForgotPasswordViewModel BuildForgotPasswordViewModel();
    }
}