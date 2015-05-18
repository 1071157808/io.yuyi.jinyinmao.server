// FileInformation: nyanya/Domain/ReadModelWarehouse.cs
// CreatedTime: 2014/06/09   6:01 PM
// LastUpdatedTime: 2014/06/10   12:04 PM

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.ReadModel.Interface;
using Infrastructure.Lib.Extensions;

namespace Domain.ReadModel
{
    public class ReadModelWarehouse
    {
        private static readonly Dictionary<Type, IReadModelDataService> ReadModelDataServices;

        static ReadModelWarehouse()
        {
            ReadModelDataServices = new Dictionary<Type, IReadModelDataService>();
        }

        public static RegisterResult RegisterReadModelDataServices<T>(IReadModelDataService<T> dataService) where T : IReadModel
        {
            if (ReadModelDataServices.ContainsKey(typeof(T)))
            {
                return new RegisterResult { Message = "Data service for {0} has been registered.", Successed = false };
            }

            ReadModelDataServices.Add(typeof(T), dataService);
            return new RegisterResult { Message = "Registered Successfully.", Successed = true };
        }

        public Task<T> Query<T, T1>(T1 t) where T : IReadModel, IQueryableFromWarehouse<T1>
        {
            IReadModelDataService dateService;
            if (ReadModelDataServices.TryGetValue(typeof(T), out dateService))
            {
                if (dateService is IReadModelDataService<T, T1>)
                {
                    return (dateService as IReadModelDataService<T, T1>).GetReadModel(t);
                }
            }

            throw new NoSupportedDataServiceException("No supported data service for {0} with parameters [{1}]".FormatWith(typeof(T).AssemblyQualifiedName), typeof(T1).Name);
        }

        #region Nested type: RegisterResult

        public class RegisterResult
        {
            public string Message { get; set; }

            public bool Successed { get; set; }
        }

        #endregion Nested type: RegisterResult
    }
}