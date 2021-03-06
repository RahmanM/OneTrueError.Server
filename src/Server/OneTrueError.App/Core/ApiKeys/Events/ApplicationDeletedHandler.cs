﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetCqs;
using Griffin.Container;
using OneTrueError.Api.Core.Applications.Events;

namespace OneTrueError.App.Core.ApiKeys.Events
{
    /// <summary>
    ///     Will either delete an entire apikey (if the only association is with the given application) or just remove the
    ///     application mapping.
    /// </summary>
    [Component(RegisterAsSelf = true)]
    public class ApplicationDeletedHandler : IApplicationEventSubscriber<ApplicationDeleted>
    {
        private readonly IApiKeyRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="ApplicationDeletedHandler"/>.
        /// </summary>
        /// <param name="repository">repos</param>
        public ApplicationDeletedHandler(IApiKeyRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task HandleAsync(ApplicationDeleted e)
        {
            var apps = await _repository.GetForApplicationAsync(e.ApplicationId);
            foreach (var apiKey in apps)
            {
                if (apiKey.Claims.Length == 1)
                    await _repository.DeleteAsync(apiKey.Id);
                else
                    await _repository.DeleteApplicationMappingAsync(apiKey.Id, e.ApplicationId);
            }
        }
    }
}