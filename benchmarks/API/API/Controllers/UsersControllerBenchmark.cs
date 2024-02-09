using API.Application.DTOs;
using API.Benchmark.Helpers;
using API.Controllers;
using API.Domain.Enums;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Benchmark.Controllers
{
    [MemoryDiagnoser]
    public class UsersControllerBenchmark
    {
        private UsersController _userController;
        public UsersControllerBenchmark()
        {
            _userController = Initialize();
        }
        private UsersController Initialize()
        {
            var _unitOfWork = CommonLibrary.CreateUnitOfWork();
            var mapper = CommonLibrary.CreateAutoMapper();
            _userController = new UsersController(_unitOfWork, mapper);

            // Assign the created HttpContext to the controller
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = CommonLibrary.CreateControllerContext()
            };
            return _userController;
        }

        // | StdDev    | Mean     | Max      | Min      | Median   | Op/s  | Memory  |
        // | 0.0238 ms | 2.224 ms | 2.253 ms | 2.178 ms | 2.227 ms | 449.6 | 0.44 MB |

        [Benchmark]
        public async Task GetUsers()
        {
            _userController = Initialize();
            var userParams = new UserParams() { ItemsPerPage = 10, MaxAge = 25, MinAge = 20, OrderBy = OrderByOptions.LastActive, PageNumber = 1, Sex = SexOptions.Both };
            var actionResult = await _userController.GetUsers(userParams);

            // To display the result of the endpoint in a json format
            // CommonLibrary.PrintActionResult(actionResult);
        }
        // | StdDev    | Mean     | Max      | Min      | Median   | Op/s  | Memory  |
        // | 0.0462 ms | 1.743 ms | 1.850 ms | 1.681 ms | 1.742 ms | 573.9 | 0.42 MB |
        [Benchmark]
        public async Task GetUser()
        {
            _userController = Initialize();
            var actionResult = await _userController.GetUser(1);

            // To display the result of the endpoint in a json format
            // CommonLibrary.PrintActionResult(actionResult);
        }
    }
}
