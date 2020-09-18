using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LearningAccess.Api.Interfaces;
using LearningAccess.Api.Models;
using LearningAccess.Api.Services;
using LearningAccess.DataAccess;
using LearningAccess.DataAccess.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace LearningAccess.Api
{
	public class Startup
	{
		public DBConnection dbDataConfiguration;

		public Startup(IConfiguration configuration)
		{
			dbDataConfiguration = configuration.Get<DBConnection>();
			DatabaseFactory.SetDatabase(() => new SqlDatabase(dbDataConfiguration.DefaultDB), GetDatabase, false);
			DatabaseFactory.SetDatabase(() => new SqlDatabase(dbDataConfiguration.DefaultanotherDB), GetDatabase, false);
			Configuration = configuration;
		}

		public Database GetDatabase(string name)
		{
			string sqlConnectionString = Configuration.GetSection(name).Value;
			if(!sqlConnectionString.ToLower().Contains("data source") || !sqlConnectionString.ToLower().Contains("initial catalog"))
			{
				if(Configuration.GetSection("UseCertificate:IsConfirm").Value.ToLower() == "yes")
				{
					IConfigurationSection certificateConfiguration = Configuration.GetSection("Certificate");
					Utilities util = new Utilities();
					//sqlConnectionString = util.DecryptText(sqlConnectionString, certificateConfiguration);
				}
				else
				{
					IConfigurationSection certificateConfiguration = Configuration.GetSection("Token");
					sqlConnectionString = GetDTokenValidateString(sqlConnectionString, certificateConfiguration);

				}
			}

			return new SqlDatabase(sqlConnectionString);
		}

		private static string GetDTokenValidateString(string sqlConnectionString, IConfigurationSection certificateConfiguration)
		{
			Utilities util = new Utilities();
			return util.DecryptText(sqlConnectionString);
			//return util.DecryptText(sqlConnectionString, certificateConfiguration.GetSection("Validate").Value;

		}

		private static string GetDecryptedConnectionString(string connectionString, IConfigurationSection config)
		{
			ServiceCollection services = new ServiceCollection();

			services.AddDataProtection()
				.SetApplicationName(config.GetSection("AppName").Value)
				.PersistKeysToFileSystem(new System.IO.DirectoryInfo(config.GetSection("PersistKeyPath").Value))
				.ProtectKeysWithCertificate(new X509Certificate2(config.GetSection("FilePath").Value, config.GetSection("Credentials").Value,
				X509KeyStorageFlags.PersistKeySet));

			IDataProtectionProvider dpProvider = services.BuildServiceProvider().GetDataProtectionProvider();

			return dpProvider.CreateProtector("ConfigDataProtection").Unprotect(connectionString);
		}

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			builder.AddEnvironmentVariables();
			IConfigurationRoot configurationRoot = builder.Build();
			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			if(Configuration.GetSection("UseCertificate:IsConfirm").Value == "Yes")
			{
				services.AddDataProtectionProvider(Configuration.GetSection("Certificate"));
			}

			services.Configure<DBConnection>(Configuration.GetSection("ConnectionStrings"));
			services.Configure<FirstAuthorizationSettings>(Configuration.GetSection("AuthenticationToken"));
			services.Configure<SecondAuthorizationSettings>(Configuration.GetSection("AuthenticationTokenTwo"));
			services.AddSingleton<ITokenService, FirstTokenService>();
			services.AddSingleton<ITokenService, SecondTokenService>();
			services.AddTransient<IApiService, DefaultApiService>();
			

			services.AddMvc(options =>
			{
				options.RespectBrowserAcceptHeader = true; // false by default
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			//services.AddTransient<IDefaultRepository, DefaultRepository>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
