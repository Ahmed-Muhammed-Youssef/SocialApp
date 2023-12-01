using API.Application.DTOs;
using API.Benchmark.Helpers;
using API.Controllers;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Benchmark.Controllers
{
    [MemoryDiagnoser]
    [Config(typeof(MyBenchmarkConfig))]
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

        // | StdDev    | Mean     | Max      | Min      | Median   | Op/s  |
        // | 0.0492 ms | 4.076 ms | 4.146 ms | 3.973 ms | 4.088 ms | 245.4 |

        [Benchmark]
        public async Task GetUsers()
        {
            _userController = Initialize();
            var userParams = new UserParams() { ItemsPerPage = 10, MaxAge = 25, MinAge = 20, OrderBy = "lastActive", PageNumber = 1, Sex = "b" };
            var actionResult = await _userController.GetUsers(userParams);

            // To display the result of the endpoint in a json format
            // CommonLibrary.PrintActionResult(actionResult);
        }
        // | StdDev    | Mean     | Max      | Min      | Median   | Op/s  |
        // | 0.0197 ms | 4.216 ms | 4.253 ms | 4.181 ms | 4.217 ms | 237.2 |
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
