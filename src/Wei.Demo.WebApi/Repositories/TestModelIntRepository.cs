using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wei.DapperRepository;

namespace Wei.Demo.WebApi.Repositories
{
    public interface ITestModelRepository : IRepository<TestModelInt>
    {
        Task<TestModelInt> UpdateResultAsync(int id, string result);

        Task<int> InsertWithTransaction(bool isThrowException);
    }

    public class TestModelRepository : DapperRepository<TestModelInt>, ITestModelRepository
    {
        public TestModelRepository(IEnumerable<IDbFactory> dbFactories) : base(dbFactories)
        {

        }


        public async Task<TestModelInt> UpdateResultAsync(int id, string result)
        {
            var isSuccess = await UpdateAsync(x => x.Id == id, x => x.Result = result);
            if (!isSuccess) return default;
            return await GetAsync(id);
        }

        public async Task<int> InsertWithTransaction(bool isThrowException)
        {
            try
            {
                BeginTrans();
                var entity = await InsertAsync(new TestModelInt { MethodName = "Transaction" });

                if (isThrowException)
                    throw new Exception();

                Commit();
                return entity.Id;
            }
            catch (Exception ex)
            {
                Rollback();
                return 0;
            }
        }
    }
}
