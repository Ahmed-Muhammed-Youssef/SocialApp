global using Domain.FriendRequestAggregate;
global using Domain.DirectChatAggregate;
global using Domain.ApplicationUserAggregate;
global using Domain.MediaAggregate;
global using Domain.FriendAggregate;
global using Domain.AuthUserAggregate;
global using Domain.ReferenceData;

global using Shared.Pagination;
global using Shared.RepositoryBase;

global using Application.Common.Mappings;
global using Application.Common.Interfaces;
global using Application.Common.Interfaces.Identity;
global using Application.Common.Interfaces.Repositories;
global using Application.Common.Interfaces.Orchestration;
global using Application.Features.Auth;
global using Application.Features.Users;
global using Application.Features.Pictures;
global using Application.Features.Posts;
global using Application.Features.DirectChats;
global using Application.Features.DirectChats.Stores;

global using Infrastructure.Auth;
global using Infrastructure.Auth.Options;
global using Infrastructure.Data;
global using Infrastructure.Data.Identity;
global using Infrastructure.Data.Repositories;
global using Infrastructure.Data.Repositories.CachedRepositories;
global using Infrastructure.Media.Cloudinary;
global using Infrastructure.Users;
global using Infrastructure.DirectChats;
global using Infrastructure.Orchestration.UserProvisioning;

global using Shared.Constants;

global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Data.SqlClient;

global using System.Security.Claims;
global using System.Collections.Concurrent;
global using System.IdentityModel.Tokens.Jwt;
global using System.Text;
global using System.Data;

global using Bogus;
global using Dapper;
global using CloudinaryDotNet;
global using CloudinaryDotNet.Actions;