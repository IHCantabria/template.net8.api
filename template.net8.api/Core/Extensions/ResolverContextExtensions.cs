using HotChocolate.Language;
using HotChocolate.Resolvers;

namespace template.net8.api.Core.Extensions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class ResolverContextExtensions
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal static Dictionary<string, HashSet<string>> GetSelectedFieldsTree(this IResolverContext context)
    {
        var result = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        VisitSelection(context.Selection.SyntaxNode, result, string.Empty);
        return result;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static void VisitSelection(FieldNode fieldNode, Dictionary<string, HashSet<string>> tree, string parentPath)
    {
        var currentPath = string.IsNullOrEmpty(parentPath)
            ? fieldNode.Name.Value
            : $"{parentPath}.{fieldNode.Name.Value}";

        if (fieldNode.SelectionSet != null)
        {
            foreach (var selection in fieldNode.SelectionSet.Selections.OfType<FieldNode>())
                VisitSelection(selection, tree, currentPath);
        }
        else
        {
            if (!tree.ContainsKey(parentPath))
                tree[parentPath] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            tree[parentPath].Add(fieldNode.Name.Value);
        }
    }
}