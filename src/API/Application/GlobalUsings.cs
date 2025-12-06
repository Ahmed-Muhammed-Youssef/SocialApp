global using Mediator;
global using CloudinaryDotNet.Actions;

global using System.ComponentModel.DataAnnotations;
global using System.Linq.Expressions;

global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;

global using Application.Common.Mappings;
global using Application.Common.Interfaces;
global using Application.Common.Interfaces.Repositories;
global using Application.Features.Users;
global using Application.Features.Users.Specifications;
global using Application.Features.DirectChats;
global using Application.Features.DirectChats.Stores;
global using Application.Features.Pictures;
global using Application.Features.Auth.Login;
global using Application.Features.Posts;

global using Domain;
global using Domain.MediaAggregate;
global using Domain.FriendRequestAggregate;
global using Domain.FriendAggregate;
global using Domain.DirectChatAggregate;
global using Domain.ApplicationUserAggregate;
global using Domain.ApplicationUserAggregate.FilterSpecifications;

global using Shared.Constants;

global using Shared;
global using Shared.Results;
global using Shared.RepositoryBase;
global using Shared.Extensions;
global using Shared.Pagination;
global using Shared.Specification;
