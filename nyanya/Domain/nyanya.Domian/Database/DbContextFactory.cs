// FileInformation: nyanya/Domian/DbContextFactory.cs
// CreatedTime: 2014/07/04   1:40 PM
// LastUpdatedTime: 2014/07/04   1:46 PM

namespace Domian.Database
{
    /// <summary>
    ///     DBContextFactory
    /// </summary>
    public class DbContextFactory
    {
        public T Create<T>() where T : DbContextBase, new()
        {
            return new T();
        }
    }
}