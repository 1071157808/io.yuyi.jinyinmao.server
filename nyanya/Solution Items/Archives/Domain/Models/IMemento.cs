// FileInformation: nyanya/Domain/IMemento.cs
// CreatedTime: 2014/06/04   12:47 PM
// LastUpdatedTime: 2014/06/04   12:57 PM

using System;

namespace Domain.Models
{
    public interface IMemento
    {
        Guid Guid { get; set; }

        int Version { get; set; }
    }
}