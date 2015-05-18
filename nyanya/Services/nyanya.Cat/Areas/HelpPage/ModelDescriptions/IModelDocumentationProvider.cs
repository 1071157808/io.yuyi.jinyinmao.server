// FileInformation: nyanya/nyanya.Cat/IModelDocumentationProvider.cs
// CreatedTime: 2014/09/01   10:40 AM
// LastUpdatedTime: 2014/09/01   10:55 AM

using System;
using System.Reflection;

namespace nyanya.Cat.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}