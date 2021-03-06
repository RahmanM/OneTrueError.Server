﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCqs;
using Griffin.Container;
using log4net;
using OneTrueError.Api.Core.Applications.Commands;
using OneTrueError.Infrastructure.Security;

namespace OneTrueError.App.Core.Applications.CommandHandlers
{
    /// <summary>
    ///     Remove a team member from an application
    /// </summary>
    [Component]
    public class RemoveTeamMemberHandler : ICommandHandler<RemoveTeamMember>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RemoveTeamMember));

        /// <summary>
        ///     Creates a new instance of <see cref="RemoveTeamMemberHandler" />.
        /// </summary>
        /// <param name="applicationRepository">To remove member</param>
        public RemoveTeamMemberHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(RemoveTeamMember command)
        {
            ClaimsPrincipal.Current.EnsureApplicationAdmin(command.ApplicationId);
            await _applicationRepository.RemoveTeamMemberAsync(command.ApplicationId, command.UserToRemove);
            _logger.Info("Removed " + command.UserToRemove + " from application " + command.ApplicationId);
        }
    }
}