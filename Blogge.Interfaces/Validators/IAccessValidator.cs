namespace Blogge.Interfaces.Validators
{
    public interface IAccessValidator
    {
        bool CanAccessPost(int id);
        bool CanAccessComment(int id);
        bool IsAdmin();
        bool IsModerator();
        bool IsAdministration();
        bool IsRedactor();
        bool IsUser();

    }
}