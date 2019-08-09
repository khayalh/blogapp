using Blogge.Interfaces.Converters;
using Blogge.Models.EntityModels;

namespace Blogge.Core.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Blogge.Models;
    using Blogge.Interfaces.Repositories;
    using System.Drawing;
    using Blogge.Interfaces.Facades;
    using Blogge.Models.DB;
    using Blogge.Interfaces.Facades.DB;
    using System;
    using System.Web;

    public class ImageRepository : IImageRepository
    {
        private readonly IDataConverter _fileController;
        private readonly IDBContextFacade _dbContext;

        public ImageRepository(IDataConverter fileController, IDBContextFacade dbContext)
        {
            _fileController = fileController;
            _dbContext = dbContext;
        }
    
        //ADD
        public void AddImageToDb(string userID, HttpPostedFileBase file)
        {

            var image = _fileController.FileBaseToByteArray(file);

            var db = _dbContext.GetDBContext();
            var currentUser = db.Users.Where(x => x.Id == userID).Single();

            if (currentUser.Avatar == null)
            {
                currentUser.Avatar = new Picture() {  AvatarInBytes = image };
            }
            else
            {
                currentUser.Avatar.AvatarInBytes = image;
            }

            db.SaveChanges();
            db.Dispose();
        }

        public string GetImageInString(string userID)
        {
            byte[] avatarInBytes;
            string avatarInString;
            var db = _dbContext.GetDBContext();

            if (!db.Users.Where(x => x.Id == userID).Any())
                {
                    return string.Empty;
                }
                var currentUser = db.Users.Where(x => x.Id == userID).Single();

                if (currentUser.Avatar != null)
                {
                    avatarInBytes = currentUser.Avatar.AvatarInBytes;
                    avatarInString = _fileController.ConvertToString(avatarInBytes);
                }
                else
                {
                    avatarInString = null;
                }
            
            return avatarInString;
        }
    }

}