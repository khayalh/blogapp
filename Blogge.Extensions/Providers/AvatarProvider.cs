
namespace Blogge.Extensions.Providers
{
    using System;
    using System.Linq;
    using Blogge.Models.DB;

    public class AvatarProvider
    {
        public static string GetAvatar(string userID)
        {
            byte[] avatarInBytes;
            string avatarInString;
            using (var ctx = new ApplicationDbContext())
            {
                if (!ctx.Users.Where(x => x.Id == userID).Any())
                {
                    return string.Empty;
                }
                var currentUser = ctx.Users.Where(x => x.Id == userID).Single();

                if (currentUser.Avatar != null)
                {
                    avatarInBytes = currentUser.Avatar.AvatarInBytes;
                    if (avatarInBytes == null)
                    {
                        return string.Empty;
                    }
                     avatarInString = Convert.ToBase64String(avatarInBytes);
                    avatarInString = String.Format("data:image/gif;base64,{0}", avatarInString);
                }
                else
                {
                    avatarInString = null;
                }

                return avatarInString;
            }
        }
    }
}