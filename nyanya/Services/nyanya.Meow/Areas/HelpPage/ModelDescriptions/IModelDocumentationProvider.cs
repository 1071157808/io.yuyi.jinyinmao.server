// FileInformation: nyanya/nyanya.Meow/IModelDocumentationProvider.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:50 AM

using System;
using System.Reflection;

namespace nyanya.Meow.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}