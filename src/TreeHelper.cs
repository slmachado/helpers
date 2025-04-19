namespace Helpers;

public static class TreeHelper
{
    /// <summary>
    /// Return all nodes in a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="treeNode"></param>
    /// <param name="getChildren"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetAllNodes<T>(T treeNode, Func<T,IEnumerable<T>> getChildren )
    {
        List<T> result = new List<T> { treeNode };

        var children = getChildren(treeNode);
        foreach (T child in children)
        {
            result.AddRange(GetAllNodes(child, getChildren));
        }
        return result;
    }

    /// <summary>
    /// Do the action on each node if matching to the condition
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="treeNode"></param>
    /// <param name="getChildren"></param>
    /// <param name="condition"></param>
    /// <param name="action"></param>
    /// <param name="elseAction"></param>
    public static void DoActionOnCondition<T>(T treeNode,  Func<T,IEnumerable<T>> getChildren , Func<T, bool>? condition, Action<T>? action,  Action<T>? elseAction = null)
    {
        if (treeNode == null || condition == null || action == null) return;
        if (condition(treeNode))
        {
            action(treeNode);
        }
        else if (elseAction != null)
        {
            elseAction(treeNode);
        }

        var children = getChildren(treeNode);
        foreach (T child in children)
        {
            DoActionOnCondition(child, getChildren, condition, action, elseAction);
        }

    }


    /// <summary>
    /// Get site tree
    /// </summary>
    /// <typeparam name="TP"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="rootId"></param>
    /// <param name="items"></param>
    /// <param name="id"></param>
    /// <param name="parentId"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    public static T? GetTree<TP, T>(TP rootId, IEnumerable<T>? items, Func<T, TP> id, Func<T, TP?> parentId, Func<T, IList<T>> children) where T : class where TP : struct
    {
        if (items == null) return null;
        var enumerable = items as T[] ?? items.ToArray();
        if (!enumerable.Any()) return null;
        var assets = enumerable.ToDictionary(id);

        var assetsGrouped = assets.Values.GroupBy(parentId);
        foreach (var assetGroup in assetsGrouped)
        {
            if (!assetGroup.Key.HasValue) continue;
            var asset = assets[assetGroup.Key.Value];
            foreach (var child in assetGroup)
            {
                children(asset).Add(child);
            }
        }

        return assets.GetValueOrDefault(rootId);
    }
}