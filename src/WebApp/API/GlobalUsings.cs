global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Microsoft.EntityFrameworkCore;

global using AutoMapper;
global using Mediator;

global using System;
global using System.Text;
global using System.Text.Json;
global using System.Linq;
global using System.Collections.Generic;
global using System.Threading.Tasks;
global using System.Net;

global using Application.Interfaces;
global using Application.Interfaces.Services;
global using Application.Authentication.Google;
global using Application.Authentication.GoogleModels;
global using Application.DTOs.User;
global using Application.DTOs.Pagination;
global using Application.MappingProfiles;
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
global using Application.Features.Account.Login;
global using Application.Features.Account.Register;
global using Application.Features.Messages.Delete;
global using Application.Features.Pictures;
global using Application.Features.Pictures.Delete;
global using Application.Features.Pictures.List;
global using Application.Features.Pictures.Create;
global using Application.Features.FriendRequests.List;

global using API.Extensions;
global using API.Filters;
global using API.Middleware;
global using API.SignalR;
global using API.Errors;
global using API.Common.Headers;
global using API.Controllers.Account.Requests;
global using API.Controllers.Account.Responses;
global using API.Controllers.Users;
global using API.Controllers.Users.Requests;

global using Domain;
global using Domain.Entities;
global using Domain.Constants;

global using Infrastructure.Data;
global using Infrastructure.ExternalServices.Google;
global using Infrastructure.Identity;
global using Infrastructure.RealTime.Presence;

global using Shared.Extensions;
global using Shared.Pagination;
global using Shared.Results;