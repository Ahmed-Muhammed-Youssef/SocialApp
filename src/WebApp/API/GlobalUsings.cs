global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Mediator;

global using System.Text;
global using System.Text.Json;
global using System.Net;

global using Application.Features.Users;
global using Application.Features.Users.Get;
global using Application.Features.Users.List;
global using Application.Features.Users.Update;
global using Application.Features.Roles;
global using Application.Features.Roles.Create;
global using Application.Features.Roles.Delete;
global using Application.Features.Posts.Create;
global using Application.Features.Posts.GetById;
global using Application.Features.Posts.GetByOwnerId;
global using Application.Features.Messages.Delete;
global using Application.Features.Pictures;
global using Application.Features.Pictures.Delete;
global using Application.Features.Pictures.List;
global using Application.Features.Pictures.Create;
global using Application.Features.FriendRequests.List;
global using Application.Features.FriendRequests.Create;
global using Application.Features.FriendRequests.Delete;
global using API.Middleware;
global using API.SignalR;
global using API.Common.Headers;
global using API.Controllers.Users;
global using API.Controllers.Users.Requests;

global using Domain;
global using Domain.Entities;
global using Domain.Constants;

global using Infrastructure.Data;

global using Shared.Extensions;
global using Shared.Results;