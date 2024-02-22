using Microsoft.AspNetCore.Mvc.ModelBinding;

using VideoSharingPlatform.Core.Common;

public static class ModelStateDictionaryExtensions {
    public static ModelStateDictionary AddModelErrors(this ModelStateDictionary modelStateDictionary, IEnumerable<Error> errors) {
        foreach (var error in errors) {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return modelStateDictionary;
    }
}