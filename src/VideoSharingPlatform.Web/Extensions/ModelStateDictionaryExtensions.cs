using Microsoft.AspNetCore.Mvc.ModelBinding;
using VideoSharingPlatform.Core.Common;

public static class ModelStateDictionaryExtensions {
    public static ModelStateDictionary AddModelErrors(this ModelStateDictionary modelStateDictionary, ExceptionBase[] exceptions) {
        foreach (var exception in exceptions) {
            modelStateDictionary.AddModelError(exception.PropertyName, exception.ErrorMessage);
        }

        return modelStateDictionary;
    }
}