using API.Application.DTOs;
using API.Benchmark.Helpers;
using API.Controllers;
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
        // | 0.0096 ms | 3.841 ms | 3.858 ms | 3.822 ms | 3.843 ms | 260.3 | 0.55 MB |

        [Benchmark]
        public async Task GetUsers()
        {
            _userController = Initialize();
            var userParams = new UserParams() { ItemsPerPage = 10, MaxAge = 25, MinAge = 20, OrderBy = "lastActive", PageNumber = 1, Sex = "b" };
            var actionResult = await _userController.GetUsers(userParams);

            // To display the result of the endpoint in a json format
            // CommonLibrary.PrintActionResult(actionResult);
        }
        // | StdDev    | Mean     | Max      | Min      | Median   | Op/s  | Memory  |
        // | 0.0080 ms | 3.627 ms | 3.645 ms | 3.617 ms | 3.625 ms | 275.7 | 0.52 MB |
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
