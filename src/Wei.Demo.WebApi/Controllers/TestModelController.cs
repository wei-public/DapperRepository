using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wei.DapperRepository;
using Wei.Demo.WebApi.DbFactories;
using Wei.Demo.WebApi.Repositories;

namespace Wei.Demo.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestModelController : ControllerBase
    {
        //泛型Repository
        private readonly IRepository<TestModelInt> _repository;

        //泛型Repository 指定DbFactory
        private readonly IRepository<TestModelMultipeKey, MySqlDbFactory> _multipeKeyRepository;

        //自定义Repository
        private readonly ITestModelRepository _testModelRepository;
        public TestModelController(IRepository<TestModelInt> repository,
            IRepository<TestModelMultipeKey, MySqlDbFactory> multipeKeyRepository,
            ITestModelRepository testModelRepository)
        {
            _repository = repository;
            _multipeKeyRepository = multipeKeyRepository;
            _testModelRepository = testModelRepository;
        }

        [HttpGet]
        public TestModelInt FirstOrDefault()
        {
            var result = _repository.FirstOrDefault();
            return result;
        }

        [HttpGet("custom-dbfactory")]
        public Task<IEnumerable<TestModelMultipeKey>> GetAllAsync()
        {
            return _multipeKeyRepository.GetAllAsync();
        }

        [HttpPut("custom-reposotry")]
        public Task<TestModelInt> UpdateResultAsync(int id, string result)
        {
            return _testModelRepository.UpdateResultAsync(id, result);
        }

        [HttpPost("transaction")]
        public Task<int> InsertWithTransaction(bool isThrowException)
        {
            return _testModelRepository.InsertWithTransaction(isThrowException);
        }
    }
}
