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

global using Application.Interfaces;
global using Application.Interfaces.Services;
global using Application.Authentication.Google;
global using Application.Authentication.GoogleModels;
global using System;
global using System.Text;
global using System.Text.Json;
global using System.Linq;
global using System.Collections.Generic;
global using System.Threading.Tasks;
global using System.Net;

global using Application.DTOs.User;
global using Application.DTOs.Role;
global using Application.DTOs.Post;
global using Application.DTOs.Message;
global using Application.DTOs.Picture;
global using Application.DTOs.Pagination;

global using API.Extensions;
global using API.Filters;
global using API.Middleware;
global using API.SignalR;
global using API.Errors;

global using Domain;
global using Domain.Entities;
global using Domain.Constants;

global using Infrastructure.Data;
global using Infrastructure.MappingProfiles;
global using Infrastructure.ExternalServices.Google;
global using Infrastructure.Identity;
global using Infrastructure.RealTime.Presence;

global using Shared.Extensions;
