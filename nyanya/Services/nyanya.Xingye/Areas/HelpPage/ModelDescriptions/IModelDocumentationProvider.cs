// FileInformation: nyanya/nyanya.Xingye/IModelDocumentationProvider.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:46 AM

using System;
using System.Reflection;

namespace nyanya.Xingye.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}