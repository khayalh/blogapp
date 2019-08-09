// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Blogge.Core.Converters;
using Blogge.Interfaces.Builders.Account;
using Blogge.Interfaces.Converters;
using Blogge.Models.EntityModels;

namespace Blogge.Mapper.DependencyResolution
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using Microsoft.Owin.Security;
    using System.Data.Entity;
    using System.Web;
    using Blogge.Models;
    using System;
    using Blogge.Interfaces.Repositories;
    using Blogge.Core.Repositories;
    using Blogge.Facades.DB;
    using Blogge.Models.DB;
    using Blogge.Facades.Identity;
    using Blogge.Interfaces.Facades.DB;
    using Blogge.Interfaces.Facades.Identity;
    using Blogge.Interfaces.Facades.Systems;
    using Blogge.Facades.Systems;
    using Blogge.Builders.Model;
    using Blogge.Interfaces.Builders.Model;
    using Blogge.Interfaces.Factories.Model;
    using Blogge.Factories.Model;
    using Blogge.Interfaces.Builders.Administration;
    using Blogge.Builders.Administration;
    using Blogge.Interfaces.Validators;
    using Blogge.Core.Validators;
    using System.Diagnostics.CodeAnalysis;
    using Blogge.Builders.Account;

    [ExcludeFromCodeCoverage]
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromPath(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString());
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
            For<DbContext>().Use(() => new ApplicationDbContext());
            For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<IDBContextFacade>().Use<DBContextFacade>();
            For<IBlogRepository>().Use<BlogRepository>();
            For<IDBContextFacade>().Use<DBContextFacade>();
            For<IIdentityFacade>().Use<IdentityFacade>();
            For<IImageRepository>().Use<ImageRepository>();
            For<IDataConverter>().Use<DataConverter>();
            For<IMemoryStreamFacade>().Use<MemoryStreamFacade>();
            For<IImageFacade>().Use<ImageFacade>();
            For<IPostBuilder>().Use<PostBuilder>();
            For<IPostFactory>().Use<PostFactory>();
            For<IAdminModelBuilder>().Use<AdminModelBuilder>();
            For<IAccessValidator>().Use<AccessValidator>();
            For<IUserRepository>().Use<UserRepository>();
            For<IDisplayValidator>().Use<DisplayValidator>();
            For<IUserManagerFacade>().Use<UserManagerFacade>();
            For<IAccountModelBuilder>().Use<AccountModelBuilder>();
            For<IFileTypeValidator>().Use<FileTypeValidator>();
        }

        #endregion
    }
}