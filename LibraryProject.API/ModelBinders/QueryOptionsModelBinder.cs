using Core.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryProject.ModelBinders;

public class QueryOptionsModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var pageValue = bindingContext.ValueProvider.GetValue("page").FirstValue;
        var sizeValue = bindingContext.ValueProvider.GetValue("size").FirstValue;
        var sortValue = bindingContext.ValueProvider.GetValue("sort").FirstValue;
        var descValue = bindingContext.ValueProvider.GetValue("desc").FirstValue;

        if (string.IsNullOrEmpty(pageValue))
            pageValue = bindingContext.ValueProvider.GetValue("pageNumber").FirstValue;
        
        if (string.IsNullOrEmpty(sizeValue))
            sizeValue = bindingContext.ValueProvider.GetValue("pageSize").FirstValue;
        
        if (string.IsNullOrEmpty(sortValue))
            sortValue = bindingContext.ValueProvider.GetValue("orderBy").FirstValue;
        
        if (string.IsNullOrEmpty(descValue))
            descValue = bindingContext.ValueProvider.GetValue("orderByDescending").FirstValue;

        if (!int.TryParse(pageValue, out int pageNumber))
            pageNumber = 1;
            
        if (!int.TryParse(sizeValue, out int pageSize))
            pageSize = 10;
            
        if (!bool.TryParse(descValue, out bool orderByDescending))
            orderByDescending = false;

        var queryOptions = new QueryOptions(pageNumber, pageSize, sortValue, orderByDescending);
        
        queryOptions.Normalize();

        bindingContext.Result = ModelBindingResult.Success(queryOptions);
        return Task.CompletedTask;
    }
}
