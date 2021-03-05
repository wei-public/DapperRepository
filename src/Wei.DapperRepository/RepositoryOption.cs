using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wei.DapperRepository
{
    public class RepositoryOption
    {

        internal List<Action<IServiceCollection>> ServiceActions { get; private set; } = new List<Action<IServiceCollection>>();

        internal void AddService(Action<IServiceCollection> serviceAction) => ServiceActions.Add(serviceAction);
    }
}
